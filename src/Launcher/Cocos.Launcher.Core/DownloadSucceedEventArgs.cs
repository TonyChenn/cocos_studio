using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200002B RID: 43
	public class DownloadSucceedEventArgs : EventArgs
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00008478 File Offset: 0x00006678
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00008480 File Offset: 0x00006680
		public string TargetPath { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00008489 File Offset: 0x00006689
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00008491 File Offset: 0x00006691
		public string FileName { get; private set; }

		// Token: 0x06000182 RID: 386 RVA: 0x0000849A File Offset: 0x0000669A
		public DownloadSucceedEventArgs(string downloadPath, string filename)
		{
			this.TargetPath = downloadPath;
			this.FileName = filename;
		}
	}
}
