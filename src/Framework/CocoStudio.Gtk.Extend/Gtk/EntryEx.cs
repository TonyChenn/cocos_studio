using System;
using Gdk;

namespace Gtk
{
	public class EntryEx : Entry
	{
		public event EventHandler EntryValueCommitChanged;

		public EntryEx()
		{
			base.Name = "PasswordEntry";
		}

		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if ((evnt.Key == Gdk.Key.Return || evnt.Key == Gdk.Key.KP_Enter || evnt.Key == Gdk.Key.ISO_Enter) && base.IsFocus)
			{
				if (this.EntryValueCommitChanged != null)
				{
					this.EntryValueCommitChanged(this, new EventArgs());
				}
			}
			return base.OnKeyReleaseEvent(evnt);
		}

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
