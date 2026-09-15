using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.UndoManager.Recorder;
using CocoStudio.UndoManager.TaskModel;
using CocoStudio.UndoManager.TaskModel.UndoableTask;

namespace CocoStudio.UndoManager
{
	internal class CompositeTaskManager : ICompositeTaskService
	{
		public string CurrentCompositeTaskName { get; private set; }

		public bool IsRunningCompositeTask { get; private set; }

		public bool IsEmptyCompositeTask
		{
			get
			{
				if (!this.IsRunningCompositeTask)
				{
					throw new InvalidOperationException("Not in a composite task. Can only call when running composite task.");
				}
				return this.taskList.Count == 0;
			}
		}

		private CompositeTaskManager()
		{
			this.taskList = new List<UndoableTaskBase<object>>();
			this.taskService = TaskServiceSingleton.Instance;
		}

		public static ICompositeTaskService Instance
		{
			get
			{
				if (CompositeTaskManager.instance == null)
				{
					CompositeTaskManager.instance = new CompositeTaskManager();
				}
				return CompositeTaskManager.instance;
			}
		}

		private void PushCompositeTask(string taskName, List<UndoableTaskBase<object>> tasks)
		{
			if (tasks.Count <= 0)
			{
				LogConfig.Logger.Debug("There is no tasks in the composite task.");
			}
			else
			{
				SequentiallyCompositeUndoableTask<object> task = new SequentiallyCompositeUndoableTask<object>(tasks, taskName);
				this.PerformTask(task);
			}
		}

		private void PerformTask(UndoableTaskBase<object> task)
		{
			this.CheckCurrentDocument();
			this.taskService.PerformTask<object>(task, null, this.currentDocument);
			this.currentDocument.IsDirty = true;
		}

		private void CheckSubTask(UndoTask task)
		{
			this.CheckTaskGroupName(task, this.taskList.Count);
		}

		private void CheckTaskGroupName(UndoTask task, int listCount)
		{
			if (!string.IsNullOrEmpty(task.TaskGroupName))
			{
				throw new ArgumentException("Not support task group name now.");
			}
		}

		private void AddSubTask(UndoTask task)
		{
			this.taskList.Add(task);
		}

		private bool CheckCurrentDocument()
		{
			if (this.currentDocument == null)
			{
				string message = "Must open Document can have undo action.";
				LogConfig.Logger.Error(message);
				throw new InvalidOperationException(message);
			}
			return true;
		}

		internal void OnRedone(TaskServiceEventArgs e)
		{
			this.CheckCurrentDocument();
			this.currentDocument.IsDirty = true;
		}

		internal void OnUndone(TaskServiceEventArgs e)
		{
			this.CheckCurrentDocument();
			this.currentDocument.IsDirty = true;
		}

		public void AddRecord(UndoTask task)
		{
			if (this.taskService.Enable)
			{
				if (this.IsRunningCompositeTask)
				{
					this.CheckSubTask(task);
					this.AddSubTask(task);
				}
				else
				{
					if (!string.IsNullOrEmpty(task.TaskGroupName))
					{
						throw new ArgumentException("Not support task group name now.");
					}
					this.PerformTask(task);
				}
			}
		}

		public void RunAsCompositeTask(string taskName, Action aciton)
		{
			this.BeginCompositeTask(taskName);
			aciton();
			this.EndCompositeTask();
		}

		public void RunAsCompositeTask(string taskName, Action<object> aciton, object parameter)
		{
			this.RunAsCompositeTask(taskName, delegate()
			{
				aciton(parameter);
			});
		}

		public void BeginCompositeTask(string taskName)
		{
			if (this.IsRunningCompositeTask)
			{
				LogConfig.Logger.Error("Begin composite task twice.One composite task is running.");
			}
			else
			{
				this.IsRunningCompositeTask = true;
				this.CurrentCompositeTaskName = taskName;
			}
		}

		public void EndCompositeTask()
		{
			if (!this.IsRunningCompositeTask)
			{
				LogConfig.Logger.Error("Not running composite task. Cann't end composite task");
			}
			else
			{
				this.PushCompositeTask(this.CurrentCompositeTaskName, this.taskList);
				this.taskList.Clear();
				this.IsRunningCompositeTask = false;
			}
		}

		public void BeginSoleRecorder(BaseRecorder soleRecorder)
		{
			if (this.currentSoleRecorder != null)
			{
				throw new InvalidOperationException("Can only ome recorder begin sole mode.");
			}
			this.currentSoleRecorder = soleRecorder;
		}

		public void EndSoleRecorder()
		{
			if (this.currentSoleRecorder == null)
			{
				throw new InvalidOperationException("Should call BeginSoleRecorder before call this function.");
			}
			this.currentSoleRecorder = null;
		}

		public void SetCurrentDocument(IEditableDocument document)
		{
			this.currentDocument = document;
			this.taskService.SetMaximumUndoCount(100, document);
		}

		private const int maxTaskCount = 100;

		private IUndoManager taskService;

		private List<UndoableTaskBase<object>> taskList;

		private IEditableDocument currentDocument;

		private BaseRecorder currentSoleRecorder;

		private static CompositeTaskManager instance;
	}
}
