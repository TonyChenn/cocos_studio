using System;

namespace MonoDevelop.Core
{
	// Token: 0x02000010 RID: 16
	public interface IAsyncOperation
	{
		// Token: 0x0600006E RID: 110
		void Cancel();

		// Token: 0x0600006F RID: 111
		void WaitForCompleted();

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000070 RID: 112
		bool IsCompleted { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000071 RID: 113
		bool Success { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000072 RID: 114
		bool SuccessWithWarnings { get; }

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000073 RID: 115
		// (remove) Token: 0x06000074 RID: 116
		event OperationHandler Completed;
	}
}
