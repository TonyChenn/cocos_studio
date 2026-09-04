using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200003A RID: 58
	public interface IUpdateService
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001FF RID: 511
		UpdateInfo StoreUpdateInfo { get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000200 RID: 512
		UpdateInfo TutorialsUpdateInfo { get; }

		// Token: 0x06000201 RID: 513
		void Save();

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000202 RID: 514
		// (remove) Token: 0x06000203 RID: 515
		event EventHandler<EventArgs> UpdateChanged;
	}
}
