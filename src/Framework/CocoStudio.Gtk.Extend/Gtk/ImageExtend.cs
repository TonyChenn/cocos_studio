using System;
using Gdk;
using Xwt;
using Xwt.Drawing;
using Xwt.GtkBackend;

namespace Gtk
{
	public static class ImageExtend
	{
		public static Pixbuf GetPixbuf(this Xwt.Drawing.Image image)
		{
			BitmapImage obj = image.ToBitmap(ImageIcon.ScaleFactor, ImageFormat.ARGB32);
			GtkImage gtkImage = (GtkImage)Toolkit.GetBackend(obj);
			return gtkImage.Frames[0].Pixbuf;
		}
	}
}
