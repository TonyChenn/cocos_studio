using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x0200000D RID: 13
	public abstract class UndoableTaskBase<T> : TaskBase<T>, IUndoableTask, ITask, IDisposable
	{
		// Token: 0x0600004B RID: 75 RVA: 0x000029CB File Offset: 0x00000BCB
		protected UndoableTaskBase()
		{
			base.Undoable = true;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600004C RID: 76 RVA: 0x000029E0 File Offset: 0x00000BE0
		// (remove) Token: 0x0600004D RID: 77 RVA: 0x00002A1C File Offset: 0x00000C1C
		protected event EventHandler<TaskEventArgs<T>> Undo;

		// Token: 0x0600004E RID: 78 RVA: 0x00002A58 File Offset: 0x00000C58
		private void OnUndo(TaskEventArgs<T> e)
		{
			EventHandler<TaskEventArgs<T>> undo = this.Undo;
			if (undo != null)
			{
				undo(this, e);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002A80 File Offset: 0x00000C80
		TaskResult IUndoableTask.Undo()
		{
			TaskEventArgs<T> taskEventArgs = new TaskEventArgs<T>(base.Argument);
			this.OnUndo(taskEventArgs);
			return taskEventArgs.TaskResult;
		}
	}
}
