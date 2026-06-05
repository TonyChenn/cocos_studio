using System;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000004 RID: 4
	public class CompositeTask : IDisposable
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000023A4 File Offset: 0x000005A4
		public static CompositeTask Run(string taskName, ITaskAction taskAction = null)
		{
			CompositeTask compositeTask = CompositeTask.CreateCompositeTask();
			if (!TaskServiceSingleton.Instance.IsRunningCompositeTask)
			{
				TaskServiceSingleton.Instance.BeginCompositeTask(taskName);
				compositeTask.isAlreadyOpen = true;
				if (taskAction != null)
				{
					compositeTask.taskAction = taskAction;
					taskAction.BeginTask();
				}
			}
			else
			{
				string message = string.Format("Already runing composite task {0}, the new task name is {1}", TaskServiceSingleton.Instance.CurrentCompositeTaskName, taskName);
				LogConfig.Logger.Debug(message);
			}
			return compositeTask;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002420 File Offset: 0x00000620
		public void Dispose()
		{
			if (this.isAlreadyOpen)
			{
				if (this.taskAction != null)
				{
					this.taskAction.EndTask();
				}
				TaskServiceSingleton.Instance.EndCompositeTask();
				this.isAlreadyOpen = false;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000246C File Offset: 0x0000066C
		private static CompositeTask CreateCompositeTask()
		{
			return new CompositeTask();
		}

		// Token: 0x04000002 RID: 2
		private bool isAlreadyOpen = false;

		// Token: 0x04000003 RID: 3
		private ITaskAction taskAction;
	}
}
