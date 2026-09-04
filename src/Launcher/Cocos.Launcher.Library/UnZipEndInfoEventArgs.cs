using System;

namespace Cocos.Launcher.Library
{
	// Token: 0x0200000B RID: 11
	public class UnZipEndInfoEventArgs : EventArgs
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000048 RID: 72 RVA: 0x0000361C File Offset: 0x0000181C
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00003624 File Offset: 0x00001824
		public string Error { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000362D File Offset: 0x0000182D
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00003635 File Offset: 0x00001835
		public bool IsSucceed { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000363E File Offset: 0x0000183E
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00003646 File Offset: 0x00001846
		public string ZipFilePath { get; private set; }

		// Token: 0x0600004E RID: 78 RVA: 0x0000364F File Offset: 0x0000184F
		public UnZipEndInfoEventArgs(bool isSucceed, string error, string targetPath)
		{
			this.IsSucceed = isSucceed;
			this.Error = error;
			this.ZipFilePath = targetPath;
		}
	}
}
