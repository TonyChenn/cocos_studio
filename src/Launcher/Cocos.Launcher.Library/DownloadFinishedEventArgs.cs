using System;

namespace Cocos.Launcher.Library
{
	// Token: 0x02000004 RID: 4
	public class DownloadFinishedEventArgs : EventArgs
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000027D2 File Offset: 0x000009D2
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000027DA File Offset: 0x000009DA
		public string Error { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000027E3 File Offset: 0x000009E3
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000027EB File Offset: 0x000009EB
		public bool IsSuccessed { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000027F4 File Offset: 0x000009F4
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000027FC File Offset: 0x000009FC
		public string DownloadPath { get; private set; }

		// Token: 0x06000022 RID: 34 RVA: 0x00002805 File Offset: 0x00000A05
		public DownloadFinishedEventArgs(bool isSuccessed, string error, string path)
		{
			this.IsSuccessed = isSuccessed;
			this.Error = error;
			this.DownloadPath = path;
		}
	}
}
