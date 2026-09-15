using System;
using Gdk;

namespace Gtk
{
	public class ColorSetEventArgs : EventArgs
	{
		public ColorSetEventArgs(Color color)
		{
			this.Color = color;
		}

		public Color Color;
	}
}
