using System;

namespace CocoStudio.UndoManager.TaskModel
{
	internal interface IUndoableTask : ITask, IDisposable
	{
		TaskResult Undo();
	}
}
