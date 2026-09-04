using System;
using Gdk;
using Xwt;
using Xwt.Drawing;
using Xwt.GtkBackend;

namespace Gtk
{
	// Token: 0x02000088 RID: 136
	public static class ImageExtend
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x0000C01C File Offset: 0x0000A21C
		public static Pixbuf GetPixbuf(this Xwt.Drawing.Image image)
		{
			BitmapImage obj = image.ToBitmap(ImageIcon.ScaleFactor, ImageFormat.ARGB32);
			GtkImage gtkImage = (GtkImage)Toolkit.GetBackend(obj);
			return gtkImage.Frames[0].Pixbuf;
		}
	}
}
