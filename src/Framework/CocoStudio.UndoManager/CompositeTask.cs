using System;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager
{
	public class CompositeTask : IDisposable
	{
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

		private static CompositeTask CreateCompositeTask()
		{
			return new CompositeTask();
		}

		private bool isAlreadyOpen = false;

		private ITaskAction taskAction;
	}
}
