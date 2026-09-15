using System;
using Gdk;

namespace Gtk
{
	public class CheckButtonEx : CheckButton
	{
		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			bool result;
			if (this.keyDown)
			{
				result = true;
			}
			else
			{
				this.keyDown = true;
				result = base.OnKeyPressEvent(evnt);
			}
			return result;
		}

		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			this.keyDown = false;
			return base.OnKeyReleaseEvent(evnt);
		}

		private bool keyDown = false;
	}
}
