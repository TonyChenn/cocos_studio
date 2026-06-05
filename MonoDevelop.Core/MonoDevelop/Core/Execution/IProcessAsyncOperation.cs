using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000011 RID: 17
	public interface IProcessAsyncOperation : IAsyncOperation, IDisposable
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000075 RID: 117
		int ExitCode { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000076 RID: 118
		int ProcessId { get; }
	}
}
