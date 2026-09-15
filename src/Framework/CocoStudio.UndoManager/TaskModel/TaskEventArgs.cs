using System;

namespace CocoStudio.UndoManager.TaskModel
{
	public class TaskEventArgs<TArgument> : EventArgs
	{
		public TArgument Argument { get; set; }

		public TaskResult TaskResult { get; set; }

		public TaskEventArgs(TArgument argument)
		{
			this.Argument = argument;
		}

		internal TaskEventArgs(TArgument argument, TaskMode taskMode) : this(argument)
		{
			this.TaskMode = taskMode;
		}

		public TaskMode TaskMode { get; private set; }
	}
}
