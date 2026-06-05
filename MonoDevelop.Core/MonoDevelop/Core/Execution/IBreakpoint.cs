using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200000A RID: 10
	public interface IBreakpoint
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000041 RID: 65
		string FileName { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000042 RID: 66
		int Line { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000043 RID: 67
		// (set) Token: 0x06000044 RID: 68
		bool Enabled { get; set; }
	}
}
