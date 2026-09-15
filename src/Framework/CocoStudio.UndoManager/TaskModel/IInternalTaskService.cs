using System;

namespace CocoStudio.UndoManager.TaskModel
{
	internal interface IInternalTaskService
	{
		void NotifyTaskRepeatableChanged(IInternalTask task);
	}
}
