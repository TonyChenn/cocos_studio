using System;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000005 RID: 5
	public interface IEditableDocument
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000015 RID: 21
		// (set) Token: 0x06000016 RID: 22
		bool IsDirty { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000017 RID: 23
		string Name { get; }
	}
}
