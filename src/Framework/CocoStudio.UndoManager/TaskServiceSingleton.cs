using System;
using CocoStudio.UndoManager.TaskModel;

namespace CocoStudio.UndoManager
{
	// Token: 0x0200002D RID: 45
	public class TaskServiceSingleton : TaskService, IUndoManager, ITaskService
	{
		// Token: 0x06000165 RID: 357 RVA: 0x00007427 File Offset: 0x00005627
		private TaskServiceSingleton()
		{
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007434 File Offset: 0x00005634
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

		// Token: 0x06000167 RID: 359 RVA: 0x00007466 File Offset: 0x00005666
		public void BeginCompositeTask(string taskName)
		{
			CompositeTaskManager.Instance.BeginCompositeTask(taskName);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007475 File Offset: 0x00005675
		public void EndCompositeTask()
		{
			CompositeTaskManager.Instance.EndCompositeTask();
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00007484 File Offset: 0x00005684
		public bool IsRunningCompositeTask
		{
			get
			{
				return CompositeTaskManager.Instance.IsRunningCompositeTask;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600016A RID: 362 RVA: 0x000074A0 File Offset: 0x000056A0
		public string CurrentCompositeTaskName
		{
			get
			{
				return CompositeTaskManager.Instance.CurrentCompositeTaskName;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600016B RID: 363 RVA: 0x000074BC File Offset: 0x000056BC
		public bool IsEmptyCompositeTask
		{
			get
			{
				return CompositeTaskManager.Instance.IsEmptyCompositeTask;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000074D8 File Offset: 0x000056D8
		public void SetCurrentDocument(IEditableDocument document)
		{
			CompositeTaskManager.Instance.SetCurrentDocument(document);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000074E8 File Offset: 0x000056E8
		protected override void OnRedone(TaskServiceEventArgs e)
		{
			base.OnRedone(e);
			CompositeTaskManager compositeTaskManager = CompositeTaskManager.Instance as CompositeTaskManager;
			compositeTaskManager.OnRedone(e);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007514 File Offset: 0x00005714
		protected override void OnUndone(TaskServiceEventArgs e)
		{
			base.OnUndone(e);
			CompositeTaskManager compositeTaskManager = CompositeTaskManager.Instance as CompositeTaskManager;
			compositeTaskManager.OnUndone(e);
		}

		// Token: 0x04000062 RID: 98
		private static TaskServiceSingleton instance;
	}
}
