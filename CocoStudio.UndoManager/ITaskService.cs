using System;
using System.Collections.Generic;
using CocoStudio.UndoManager.TaskModel;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000017 RID: 23
	public interface ITaskService
	{
		// Token: 0x060000AB RID: 171
		TaskResult PerformTask<T>(TaskBase<T> task, T argument, object contextKey = null);

		// Token: 0x060000AC RID: 172
		TaskResult PerformTask<T>(UndoableTaskBase<T> task, T argument, object ownerKey = null);

		// Token: 0x060000AD RID: 173
		bool CanUndo(object ownerKey = null);

		// Token: 0x060000AE RID: 174
		TaskResult Undo(object ownerKey = null);

		// Token: 0x060000AF RID: 175
		TaskResult Undo(int undoCount, object ownerKey = null);

		// Token: 0x060000B0 RID: 176
		bool CanRedo(object ownerKey = null);

		// Token: 0x060000B1 RID: 177
		TaskResult Redo(object ownerKey = null);

		// Token: 0x060000B2 RID: 178
		TaskResult Repeat(object ownerKey = null);

		// Token: 0x060000B3 RID: 179
		bool CanRepeat(object ownerKey = null);

		// Token: 0x060000B4 RID: 180
		IEnumerable<ITask> GetUndoableTasks(object ownerKey = null);

		// Token: 0x060000B5 RID: 181
		IEnumerable<ITask> GetRedoableTasks(object ownerKey = null);

		// Token: 0x060000B6 RID: 182
		IEnumerable<ITask> GetRepeatableTasks(object ownerKey = null);

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000B7 RID: 183
		// (remove) Token: 0x060000B8 RID: 184
		event EventHandler<TaskServiceEventArgs> Undone;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000B9 RID: 185
		// (remove) Token: 0x060000BA RID: 186
		event EventHandler<TaskServiceEventArgs> Redone;

		// Token: 0x060000BB RID: 187
		void Clear(object ownerKey = null);

		// Token: 0x060000BC RID: 188
		void SetMaximumUndoCount(int count, object ownerKey = null);

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000BD RID: 189
		// (set) Token: 0x060000BE RID: 190
		bool Enable { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BF RID: 191
		bool IsUndoing { get; }
	}
}
