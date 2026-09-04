using System;

namespace Gtk
{
	// Token: 0x0200006E RID: 110
	public interface IDialogButton
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600027D RID: 637
		// (set) Token: 0x0600027E RID: 638
		string Text { get; set; }

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600027F RID: 639
		// (remove) Token: 0x06000280 RID: 640
		event EventHandler<ButtonReleaseEventArgs> Clicked;

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000281 RID: 641
		// (set) Token: 0x06000282 RID: 642
		EnumMainButton ButtonType { get; set; }

		// Token: 0x06000283 RID: 643
		Widget GetWidget();
	}
}
