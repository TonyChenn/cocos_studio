using System;
using System.Drawing;

namespace Gtk.Controls
{
	public class ColorExEvent : EventArgs
	{
		public Color Color { get; set; }

		public ColorExEvent(Color color)
		{
			this.Color = color;
		}
	}
}
