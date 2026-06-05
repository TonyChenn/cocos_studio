using System;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000010 RID: 16
	public interface ICompositeTaskService
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000064 RID: 100
		string CurrentCompositeTaskName { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000065 RID: 101
		bool IsRunningCompositeTask { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000066 RID: 102
		bool IsEmptyCompositeTask { get; }

		// Token: 0x06000067 RID: 103
		void RunAsCompositeTask(string taskName, Action aciton);

		// Token: 0x06000068 RID: 104
		void RunAsCompositeTask(string taskName, Action<object> aciton, object parameter);

		// Token: 0x06000069 RID: 105
		void BeginCompositeTask(string taskName);

		// Token: 0x0600006A RID: 106
		void EndCompositeTask();

		// Token: 0x0600006B RID: 107
		void AddRecord(UndoTask task);

		// Token: 0x0600006C RID: 108
		void SetCurrentDocument(IEditableDocument document);
	}
}
