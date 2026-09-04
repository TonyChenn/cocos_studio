using System;
using Gdk;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000C RID: 12
	internal class IconLoader
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002B94 File Offset: 0x00000D94
		public static Pixbuf LoadIcon(Widget widget, string name, IconSize size)
		{
			Pixbuf pixbuf = widget.RenderIcon(name, size, null);
			if (pixbuf != null)
			{
				return pixbuf;
			}
			int num;
			int num2;
			Icon.SizeLookup(size, out num, out num2);
			Pixbuf result;
			try
			{
				result = IconTheme.Default.LoadIcon(name, num, (IconLookupFlags)0);
			}
			catch (Exception)
			{
				if (name != "gtk-missing-image")
				{
					result = IconLoader.LoadIcon(widget, "gtk-missing-image", size);
				}
				else
				{
					Pixmap pixmap = new Pixmap(Screen.Default.RootWindow, num, num);
					Gdk.GC gc = new Gdk.GC(pixmap);
					gc.RgbFgColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
					pixmap.DrawRectangle(gc, true, 0, 0, num, num);
					gc.RgbFgColor = new Color(0, 0, 0);
					pixmap.DrawRectangle(gc, false, 0, 0, num - 1, num - 1);
					gc.SetLineAttributes(3, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
					gc.RgbFgColor = new Color(byte.MaxValue, 0, 0);
					pixmap.DrawLine(gc, num / 4, num / 4, num - 1 - num / 4, num - 1 - num / 4);
					pixmap.DrawLine(gc, num - 1 - num / 4, num / 4, num / 4, num - 1 - num / 4);
					result = Pixbuf.FromDrawable(pixmap, pixmap.Colormap, 0, 0, 0, 0, num, num);
				}
			}
			return result;
		}
	}
}
