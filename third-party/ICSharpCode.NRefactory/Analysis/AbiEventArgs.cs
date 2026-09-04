using System;

namespace ICSharpCode.NRefactory.Analysis
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public sealed class AbiEventArgs : EventArgs
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		// (set) Token: 0x06000002 RID: 2 RVA: 0x000020D8 File Offset: 0x000010D8
		public string Message { get; set; }

		// Token: 0x06000003 RID: 3 RVA: 0x000020E1 File Offset: 0x000010E1
		public AbiEventArgs(string message)
		{
			this.Message = message;
		}
	}
}
