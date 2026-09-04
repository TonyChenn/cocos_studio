using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200003C RID: 60
	public class UpdateInfo
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000091A9 File Offset: 0x000073A9
		// (set) Token: 0x0600020B RID: 523 RVA: 0x000091B1 File Offset: 0x000073B1
		public string LocalTime { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600020C RID: 524 RVA: 0x000091BA File Offset: 0x000073BA
		// (set) Token: 0x0600020D RID: 525 RVA: 0x000091C2 File Offset: 0x000073C2
		public string RemoteTime { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600020E RID: 526 RVA: 0x000091CB File Offset: 0x000073CB
		public bool IsUpdate
		{
			get
			{
				return !string.Equals(this.LocalTime, this.RemoteTime);
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000091E1 File Offset: 0x000073E1
		public void Update()
		{
			this.LocalTime = this.RemoteTime;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000091EF File Offset: 0x000073EF
		public UpdateInfo(string localTime, string remoteTime)
		{
			this.LocalTime = localTime;
			this.RemoteTime = remoteTime;
		}
	}
}
