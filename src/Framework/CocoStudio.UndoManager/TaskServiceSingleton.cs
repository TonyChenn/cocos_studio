using System;
using CocoStudio.UndoManager.TaskModel;

namespace CocoStudio.UndoManager
{
	public class TaskServiceSingleton : TaskService, IUndoManager, ITaskService
	{
		private TaskServiceSingleton()
		{
		}

		public static IUndoManager Instance
		{
			get
			{
				if (TaskServiceSingleton.instance == null)
				{
					TaskServiceSingleton.instance = new TaskServiceSingleton();
				}
				return TaskServiceSingleton.instance;
			}
		}

		public void BeginCompositeTask(string taskName)
		{
			CompositeTaskManager.Instance.BeginCompositeTask(taskName);
		}

		public void EndCompositeTask()
		{
			CompositeTaskManager.Instance.EndCompositeTask();
		}

		public bool IsRunningCompositeTask
		{
			get
			{
				return CompositeTaskManager.Instance.IsRunningCompositeTask;
			}
		}

		public string CurrentCompositeTaskName
		{
			get
			{
				return CompositeTaskManager.Instance.CurrentCompositeTaskName;
			}
		}

		public bool IsEmptyCompositeTask
		{
			get
			{
				return CompositeTaskManager.Instance.IsEmptyCompositeTask;
			}
		}

		public void SetCurrentDocument(IEditableDocument document)
		{
			CompositeTaskManager.Instance.SetCurrentDocument(document);
		}

		protected override void OnRedone(TaskServiceEventArgs e)
		{
			base.OnRedone(e);
			CompositeTaskManager compositeTaskManager = CompositeTaskManager.Instance as CompositeTaskManager;
			compositeTaskManager.OnRedone(e);
		}

		protected override void OnUndone(TaskServiceEventArgs e)
		{
			base.OnUndone(e);
			CompositeTaskManager compositeTaskManager = CompositeTaskManager.Instance as CompositeTaskManager;
			compositeTaskManager.OnUndone(e);
		}

		private static TaskServiceSingleton instance;
	}
}
