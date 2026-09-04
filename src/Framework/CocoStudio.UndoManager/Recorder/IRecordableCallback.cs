using System;

namespace CocoStudio.UndoManager.Recorder
{
	// Token: 0x02000019 RID: 25
	public interface IRecordableCallback
	{
		// Token: 0x060000C6 RID: 198
		void Redoing(RecorderTaskEventArgs args);

		// Token: 0x060000C7 RID: 199
		void Undoing(RecorderTaskEventArgs args);

		// Token: 0x060000C8 RID: 200
		void Redone(RecorderTaskEventArgs args);

		// Token: 0x060000C9 RID: 201
		void Undone(RecorderTaskEventArgs args);
	}
}
