using System;
using Gdk;
using Gtk;

namespace Stetic
{
	internal class IconLoader
	{
		public static Pixbuf LoadIcon(Widget widget, string name, IconSize size)
		{
			Pixbuf pixbuf = widget.RenderIcon(name, size, null);
			if (pixbuf != null)
			{
				return pixbuf;
			}
			Icon.SizeLookup(size, out var width, out var _);
			try
			{
				return IconTheme.Default.LoadIcon(name, width, (IconLookupFlags)0);
			}
			catch (Exception)
			{
				if (name != "gtk-missing-image")
				{
					return LoadIcon(widget, "gtk-missing-image", size);
				}
				Pixmap pixmap = new Pixmap(Screen.Default.RootWindow, width, width);
				Gdk.GC gC = new Gdk.GC(pixmap);
				gC.RgbFgColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				pixmap.DrawRectangle(gC, filled: true, 0, 0, width, width);
				gC.RgbFgColor = new Color(0, 0, 0);
				pixmap.DrawRectangle(gC, filled: false, 0, 0, width - 1, width - 1);
				gC.SetLineAttributes(3, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
				gC.RgbFgColor = new Color(byte.MaxValue, 0, 0);
				pixmap.DrawLine(gC, width / 4, width / 4, width - 1 - width / 4, width - 1 - width / 4);
				pixmap.DrawLine(gC, width - 1 - width / 4, width / 4, width / 4, width - 1 - width / 4);
				return Pixbuf.FromDrawable(pixmap, pixmap.Colormap, 0, 0, 0, 0, width, width);
			}
		}
	}
}
