using System;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000007 RID: 7
	public interface ITaskAction
	{
		// Token: 0x06000018 RID: 24
		void BeginTask();

		// Token: 0x06000019 RID: 25
		void EndTask();
	}
}
