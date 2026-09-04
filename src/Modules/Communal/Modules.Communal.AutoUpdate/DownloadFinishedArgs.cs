using System;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x02000003 RID: 3
	public class DownloadFinishedArgs : EventArgs
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000248F File Offset: 0x0000068F
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002497 File Offset: 0x00000697
		public bool IsSuccessed { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000024A0 File Offset: 0x000006A0
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000024A8 File Offset: 0x000006A8
		public string Output { get; private set; }

		// Token: 0x06000021 RID: 33 RVA: 0x000024B1 File Offset: 0x000006B1
		public DownloadFinishedArgs(bool isSuccessed, string output = "")
		{
			this.IsSuccessed = isSuccessed;
			this.Output = output;
		}
	}
}
