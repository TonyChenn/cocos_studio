using System;

namespace CocoStudio.UndoManager.TaskModel
{
	public abstract class UndoableTaskBase<T> : TaskBase<T>, IUndoableTask, ITask, IDisposable
	{
		protected UndoableTaskBase()
		{
			base.Undoable = true;
		}

		protected event EventHandler<TaskEventArgs<T>> Undo;

		private void OnUndo(TaskEventArgs<T> e)
		{
			EventHandler<TaskEventArgs<T>> undo = this.Undo;
			if (undo != null)
			{
				undo(this, e);
			}
		}

		TaskResult IUndoableTask.Undo()
		{
			TaskEventArgs<T> taskEventArgs = new TaskEventArgs<T>(base.Argument);
			this.OnUndo(taskEventArgs);
			return taskEventArgs.TaskResult;
		}
	}
}
