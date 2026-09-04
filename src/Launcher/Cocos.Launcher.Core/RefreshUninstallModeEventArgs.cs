using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000023 RID: 35
	public class RefreshUninstallModeEventArgs : EventArgs
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00007AD7 File Offset: 0x00005CD7
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00007ADF File Offset: 0x00005CDF
		public UninstallModeEnum UninstallMode { get; private set; }

		// Token: 0x06000151 RID: 337 RVA: 0x00007AE8 File Offset: 0x00005CE8
		public RefreshUninstallModeEventArgs(UninstallModeEnum uninstallMode)
		{
			this.UninstallMode = uninstallMode;
		}
	}
}
