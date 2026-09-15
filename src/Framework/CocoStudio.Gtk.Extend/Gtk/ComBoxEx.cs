using System;
using System.ComponentModel;
using Gdk;

namespace Gtk
{
	[ToolboxItem(true)]
	public class ComBoxEx : ComboBox
	{
		protected override void OnFocusGrabbed()
		{
			if (this.isPress)
			{
				base.CanFocus = true;
			}
			else
			{
				base.CanFocus = false;
			}
			base.OnFocusGrabbed();
		}

		protected override bool OnScrollEvent(EventScroll evnt)
		{
			return base.IsFocus && base.OnScrollEvent(evnt);
		}

		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			if (evnt.Button == 1U)
			{
				this.isPress = true;
			}
			return base.OnButtonPressEvent(evnt);
		}

		protected override bool OnFocusOutEvent(EventFocus evnt)
		{
			this.isPress = false;
			return base.OnFocusOutEvent(evnt);
		}

		private bool isPress = false;
	}
}
