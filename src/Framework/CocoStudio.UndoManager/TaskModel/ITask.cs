using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x02000009 RID: 9
	public interface ITask : IDisposable
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000031 RID: 49
		string DescriptionForUser { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000032 RID: 50
		bool Undoable { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000033 RID: 51
		bool Repeatable { get; }
	}
}
