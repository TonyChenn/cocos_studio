using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x0200000A RID: 10
	internal interface IInternalTask : ITask, IDisposable
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000034 RID: 52
		object Argument { get; }

		// Token: 0x06000035 RID: 53
		TaskResult PerformTask(object argument, TaskMode taskMode);
	}
}
