using System;

namespace CocoStudio.UndoManager.TaskModel
{
	public class CancellableTaskServiceEventArgs : TaskServiceEventArgs
	{
		public bool Cancel
		{
			get
			{
				return this.cancelled;
			}
			set
			{
				if (!this.cancelled)
				{
					this.cancelled = value;
				}
			}
		}

		internal object OwnerKey { get; set; }

		public CancellableTaskServiceEventArgs()
		{
		}

		public CancellableTaskServiceEventArgs(ITask task) : base(task)
		{
		}

		private bool cancelled;
	}
}
