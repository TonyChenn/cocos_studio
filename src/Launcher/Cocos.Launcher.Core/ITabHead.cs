using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200004C RID: 76
	public interface ITabHead
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600028E RID: 654
		// (set) Token: 0x0600028F RID: 655
		string HeadName { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000290 RID: 656
		// (set) Token: 0x06000291 RID: 657
		bool IsShowRed { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000292 RID: 658
		bool IsSelected { get; }

		// Token: 0x06000293 RID: 659
		void SetNumber(int number);

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000294 RID: 660
		// (remove) Token: 0x06000295 RID: 661
		event EventHandler<SelectedChangingEventArgs> SelectedChanging;
	}
}
