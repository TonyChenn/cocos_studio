using System;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager
{
	// Token: 0x0200002C RID: 44
	public class TaskServiceLock : IDisposable
	{
		// Token: 0x06000162 RID: 354 RVA: 0x0000737B File Offset: 0x0000557B
		private TaskServiceLock()
		{
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007390 File Offset: 0x00005590
		public static TaskServiceLock Lock()
		{
			TaskServiceLock taskServiceLock = new TaskServiceLock();
			if (TaskServiceSingleton.Instance.Enable)
			{
				taskServiceLock.isEnable = (TaskServiceSingleton.Instance.Enable = false);
			}
			else
			{
				string message = string.Format("Task service already closed.", new object[0]);
				LogConfig.Logger.Info(message, true);
			}
			return taskServiceLock;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000073F8 File Offset: 0x000055F8
		public void Dispose()
		{
			if (!this.isEnable)
			{
				this.isEnable = (TaskServiceSingleton.Instance.Enable = true);
			}
		}

		// Token: 0x04000061 RID: 97
		private bool isEnable = true;
	}
}
