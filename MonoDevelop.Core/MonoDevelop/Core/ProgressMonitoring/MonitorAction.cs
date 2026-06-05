using System;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x0200001B RID: 27
	[Flags]
	public enum MonitorAction
	{
		// Token: 0x0400005E RID: 94
		None = 0,
		// Token: 0x0400005F RID: 95
		WriteLog = 1,
		// Token: 0x04000060 RID: 96
		ReportError = 2,
		// Token: 0x04000061 RID: 97
		ReportWarning = 4,
		// Token: 0x04000062 RID: 98
		ReportSuccess = 8,
		// Token: 0x04000063 RID: 99
		Dispose = 16,
		// Token: 0x04000064 RID: 100
		Tasks = 32,
		// Token: 0x04000065 RID: 101
		Cancel = 64,
		// Token: 0x04000066 RID: 102
		SlaveCancel = 128,
		// Token: 0x04000067 RID: 103
		All = 255
	}
}
