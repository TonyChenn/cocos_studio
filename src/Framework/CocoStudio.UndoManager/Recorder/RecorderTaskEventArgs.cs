using System;
using CocoStudio.UndoManager.TaskModel;

namespace CocoStudio.UndoManager.Recorder
{
	public class RecorderTaskEventArgs : UndoableTaskEventArgs<UndoTask>
	{
		public RecorderTaskEventArgs(UndoTask undoTask) : base(undoTask)
		{
		}
	}
}
