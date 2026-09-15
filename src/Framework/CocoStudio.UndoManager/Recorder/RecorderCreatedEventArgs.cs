using System;

namespace CocoStudio.UndoManager.Recorder
{
	public class RecorderCreatedEventArgs : EventArgs
	{
		public INotifyStateChanged Item { get; private set; }

		public UndoTask Task { get; private set; }

		public RecorderCreatedEventArgs(INotifyStateChanged item, UndoTask task)
		{
			this.Item = item;
			this.Task = task;
		}
	}
}
