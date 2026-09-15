using System;

namespace CocoStudio.UndoManager.TaskModel
{
	public class TaskServiceEventArgs : EventArgs
	{
		public ITask Task { get; private set; }

		public TaskServiceEventArgs()
		{
		}

		public TaskServiceEventArgs(ITask task)
		{
			this.Task = task;
		}
	}
}
