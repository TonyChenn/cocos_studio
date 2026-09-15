using System;

namespace CocoStudio.UndoManager
{
	public interface ITaskAction
	{
		void BeginTask();

		void EndTask();
	}
}
