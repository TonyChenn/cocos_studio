using System;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000018 RID: 24
	public interface IUndoManager : ITaskService
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C0 RID: 192
		string CurrentCompositeTaskName { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000C1 RID: 193
		bool IsRunningCompositeTask { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000C2 RID: 194
		bool IsEmptyCompositeTask { get; }

		// Token: 0x060000C3 RID: 195
		void BeginCompositeTask(string taskName);

		// Token: 0x060000C4 RID: 196
		void EndCompositeTask();

		// Token: 0x060000C5 RID: 197
		void SetCurrentDocument(IEditableDocument document);
	}
}
