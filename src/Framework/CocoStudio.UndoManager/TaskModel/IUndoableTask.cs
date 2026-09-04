using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x0200000C RID: 12
	internal interface IUndoableTask : ITask, IDisposable
	{
		// Token: 0x0600004A RID: 74
		TaskResult Undo();
	}
}
