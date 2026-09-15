using System;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager
{
	public class TaskServiceLock : IDisposable
	{
		private TaskServiceLock()
		{
		}

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

		public void Dispose()
		{
			if (!this.isEnable)
			{
				this.isEnable = (TaskServiceSingleton.Instance.Enable = true);
			}
		}

		private bool isEnable = true;
	}
}
