using System;
using Gdk;

namespace Gtk
{
	// Token: 0x0200001A RID: 26
	public class EntryEx : Entry
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000B8 RID: 184 RVA: 0x00004B64 File Offset: 0x00002D64
		// (remove) Token: 0x060000B9 RID: 185 RVA: 0x00004BA0 File Offset: 0x00002DA0
		public event EventHandler EntryValueCommitChanged;

		// Token: 0x060000BA RID: 186 RVA: 0x00004BDC File Offset: 0x00002DDC
		public EntryEx()
		{
			base.Name = "PasswordEntry";
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004BF4 File Offset: 0x00002DF4
		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if ((evnt.Key == Key.Return || evnt.Key == Key.KP_Enter || evnt.Key == Key.ISO_Enter) && base.IsFocus)
			{
				if (this.EntryValueCommitChanged != null)
				{
					this.EntryValueCommitChanged(this, new EventArgs());
				}
			}
			return base.OnKeyReleaseEvent(evnt);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004C68 File Offset: 0x00002E68
		protected override bool OnFocusOutEvent(EventFocus evnt)
		{
			if (this.EntryValueCommitChanged != null)
			{
				this.EntryValueCommitChanged(this, new EventArgs());
			}
			base.SelectRegion(0, 0);
			return base.OnFocusOutEvent(evnt);
		}
	}
}
