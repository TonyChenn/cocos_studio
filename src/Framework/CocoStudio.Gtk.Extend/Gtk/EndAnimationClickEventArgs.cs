using System;

namespace Gtk
{
	public class EndAnimationClickEventArgs : EventArgs
	{
		public Window Window { get; private set; }

		public EndAnimationClickEventArgs(Window window)
		{
			this.Window = window;
		}
	}
}
