using System;

namespace CocoStudio.UndoManager.TaskModel
{
	internal interface IInternalTask : ITask, IDisposable
	{
		object Argument { get; }

		TaskResult PerformTask(object argument, TaskMode taskMode);
	}
}
