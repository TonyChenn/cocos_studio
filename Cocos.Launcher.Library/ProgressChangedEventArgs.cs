using System;

namespace Cocos.Launcher.Library
{
	// Token: 0x02000003 RID: 3
	public class ProgressChangedEventArgs : EventArgs
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002769 File Offset: 0x00000969
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002771 File Offset: 0x00000971
		public float Fraction { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000277A File Offset: 0x0000097A
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002782 File Offset: 0x00000982
		public float FileSize { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000278B File Offset: 0x0000098B
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002793 File Offset: 0x00000993
		public float DownloadSpeed { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000279C File Offset: 0x0000099C
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000027A4 File Offset: 0x000009A4
		public long RemainTime { get; private set; }

		// Token: 0x0600001B RID: 27 RVA: 0x000027AD File Offset: 0x000009AD
		public ProgressChangedEventArgs(float fraction, float filesize = 0f, float downladSpeed = 0f, long remainTime = -1L)
		{
			this.Fraction = fraction;
			this.FileSize = filesize;
			this.DownloadSpeed = downladSpeed;
			this.RemainTime = remainTime;
		}
	}
}
