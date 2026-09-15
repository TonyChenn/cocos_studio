using System;
using System.Collections.Generic;
using CocoStudio.UndoManager.TaskModel;

namespace CocoStudio.UndoManager
{
	public interface ITaskService
	{
		TaskResult PerformTask<T>(TaskBase<T> task, T argument, object contextKey = null);

		TaskResult PerformTask<T>(UndoableTaskBase<T> task, T argument, object ownerKey = null);

		bool CanUndo(object ownerKey = null);

		TaskResult Undo(object ownerKey = null);

		TaskResult Undo(int undoCount, object ownerKey = null);

		bool CanRedo(object ownerKey = null);

		TaskResult Redo(object ownerKey = null);

		TaskResult Repeat(object ownerKey = null);

		bool CanRepeat(object ownerKey = null);

		IEnumerable<ITask> GetUndoableTasks(object ownerKey = null);

		IEnumerable<ITask> GetRedoableTasks(object ownerKey = null);

		IEnumerable<ITask> GetRepeatableTasks(object ownerKey = null);

		event EventHandler<TaskServiceEventArgs> Undone;

		event EventHandler<TaskServiceEventArgs> Redone;

		void Clear(object ownerKey = null);

		void SetMaximumUndoCount(int count, object ownerKey = null);

		bool Enable { get; set; }

		bool IsUndoing { get; }
	}
}
