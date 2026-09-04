using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000022 RID: 34
	public class RefreshRunModeEventArgs : EventArgs
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00007AB7 File Offset: 0x00005CB7
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00007ABF File Offset: 0x00005CBF
		public RunModeEnum RunMode { get; private set; }

		// Token: 0x0600014E RID: 334 RVA: 0x00007AC8 File Offset: 0x00005CC8
		public RefreshRunModeEventArgs(RunModeEnum runMode)
		{
			this.RunMode = runMode;
		}
	}
}
