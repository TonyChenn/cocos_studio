using System;
using CocoStudio.UndoManager.TaskModel;

namespace CocoStudio.UndoManager.Recorder
{
	// Token: 0x0200001D RID: 29
	public class RecorderTaskEventArgs : UndoableTaskEventArgs<UndoTask>
	{
		// Token: 0x060000DB RID: 219 RVA: 0x00004A76 File Offset: 0x00002C76
		public RecorderTaskEventArgs(UndoTask undoTask) : base(undoTask)
		{
		}
	}
}
