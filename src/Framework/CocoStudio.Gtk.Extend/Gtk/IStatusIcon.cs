using System;

namespace Gtk
{
	// Token: 0x0200006B RID: 107
	public interface IStatusIcon
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000256 RID: 598
		// (set) Token: 0x06000257 RID: 599
		object PopupMenu { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000258 RID: 600
		// (set) Token: 0x06000259 RID: 601
		string IconPath { get; set; }

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600025A RID: 602
		// (remove) Token: 0x0600025B RID: 603
		event EventHandler<EventArgs> Action;

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600025C RID: 604
		// (set) Token: 0x0600025D RID: 605
		string Tooltip { get; set; }

		// Token: 0x0600025E RID: 606
		void Dispose();
	}
}
