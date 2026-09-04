using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.UndoManager.Recorder;
using CocoStudio.UndoManager.TaskModel;
using CocoStudio.UndoManager.TaskModel.UndoableTask;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000011 RID: 17
	internal class CompositeTaskManager : ICompositeTaskService
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002E08 File Offset: 0x00001008
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002E1F File Offset: 0x0000101F
		public string CurrentCompositeTaskName { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002E28 File Offset: 0x00001028
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00002E3F File Offset: 0x0000103F
		public bool IsRunningCompositeTask { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002E48 File Offset: 0x00001048
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

		// Token: 0x06000072 RID: 114 RVA: 0x00002E7D File Offset: 0x0000107D
		private CompositeTaskManager()
		{
			this.taskList = new List<UndoableTaskBase<object>>();
			this.taskService = TaskServiceSingleton.Instance;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002EA0 File Offset: 0x000010A0
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

		// Token: 0x06000074 RID: 116 RVA: 0x00002ED4 File Offset: 0x000010D4
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

		// Token: 0x06000075 RID: 117 RVA: 0x00002F12 File Offset: 0x00001112
		private void PerformTask(UndoableTaskBase<object> task)
		{
			this.CheckCurrentDocument();
			this.taskService.PerformTask<object>(task, null, this.currentDocument);
			this.currentDocument.IsDirty = true;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002F3D File Offset: 0x0000113D
		private void CheckSubTask(UndoTask task)
		{
			this.CheckTaskGroupName(task, this.taskList.Count);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002F54 File Offset: 0x00001154
		private void CheckTaskGroupName(UndoTask task, int listCount)
		{
			if (!string.IsNullOrEmpty(task.TaskGroupName))
			{
				throw new ArgumentException("Not support task group name now.");
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002F7C File Offset: 0x0000117C
		private void AddSubTask(UndoTask task)
		{
			this.taskList.Add(task);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002F8C File Offset: 0x0000118C
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

		// Token: 0x0600007A RID: 122 RVA: 0x00002FC9 File Offset: 0x000011C9
		internal void OnRedone(TaskServiceEventArgs e)
		{
			this.CheckCurrentDocument();
			this.currentDocument.IsDirty = true;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002FE0 File Offset: 0x000011E0
		internal void OnUndone(TaskServiceEventArgs e)
		{
			this.CheckCurrentDocument();
			this.currentDocument.IsDirty = true;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002FF8 File Offset: 0x000011F8
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

		// Token: 0x0600007D RID: 125 RVA: 0x0000305C File Offset: 0x0000125C
		public void RunAsCompositeTask(string taskName, Action aciton)
		{
			this.BeginCompositeTask(taskName);
			aciton();
			this.EndCompositeTask();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003094 File Offset: 0x00001294
		public void RunAsCompositeTask(string taskName, Action<object> aciton, object parameter)
		{
			this.RunAsCompositeTask(taskName, delegate()
			{
				aciton(parameter);
			});
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000030CC File Offset: 0x000012CC
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

		// Token: 0x06000080 RID: 128 RVA: 0x0000310C File Offset: 0x0000130C
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

		// Token: 0x06000081 RID: 129 RVA: 0x00003160 File Offset: 0x00001360
		public void BeginSoleRecorder(BaseRecorder soleRecorder)
		{
			if (this.currentSoleRecorder != null)
			{
				throw new InvalidOperationException("Can only ome recorder begin sole mode.");
			}
			this.currentSoleRecorder = soleRecorder;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003190 File Offset: 0x00001390
		public void EndSoleRecorder()
		{
			if (this.currentSoleRecorder == null)
			{
				throw new InvalidOperationException("Should call BeginSoleRecorder before call this function.");
			}
			this.currentSoleRecorder = null;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000031C0 File Offset: 0x000013C0
		public void SetCurrentDocument(IEditableDocument document)
		{
			this.currentDocument = document;
			this.taskService.SetMaximumUndoCount(100, document);
		}

		// Token: 0x04000016 RID: 22
		private const int maxTaskCount = 100;

		// Token: 0x04000017 RID: 23
		private IUndoManager taskService;

		// Token: 0x04000018 RID: 24
		private List<UndoableTaskBase<object>> taskList;

		// Token: 0x04000019 RID: 25
		private IEditableDocument currentDocument;

		// Token: 0x0400001A RID: 26
		private BaseRecorder currentSoleRecorder;

		// Token: 0x0400001B RID: 27
		private static CompositeTaskManager instance;
	}
}
