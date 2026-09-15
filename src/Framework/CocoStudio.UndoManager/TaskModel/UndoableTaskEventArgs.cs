using System;

namespace CocoStudio.UndoManager.TaskModel
{
	public class UndoableTaskEventArgs<TArgument> : TaskEventArgs<TArgument>
	{
		public UndoableTaskEventArgs(TArgument argument) : base(argument)
		{
		}

		internal UndoableTaskEventArgs(TArgument argument, TaskMode taskMode) : base(argument, taskMode)
		{
		}

		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		private bool enabled = true;
	}
}
