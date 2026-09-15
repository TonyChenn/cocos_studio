using System;

namespace CocoStudio.UndoManager.Recorder
{
	public interface IRecordableCallback
	{
		void Redoing(RecorderTaskEventArgs args);

		void Undoing(RecorderTaskEventArgs args);

		void Redone(RecorderTaskEventArgs args);

		void Undone(RecorderTaskEventArgs args);
	}
}
