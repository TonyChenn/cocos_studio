using System;
using System.Drawing;
using Cairo;
using Gdk;

namespace Gtk
{
	// Token: 0x02000010 RID: 16
	public static class CairoContextExtend
	{
		// Token: 0x06000077 RID: 119 RVA: 0x000039D4 File Offset: 0x00001BD4
		public static void SetColor(this Context cr, System.Drawing.Color color)
		{
			cr.SetSourceRGBA((double)color.R / 255.0, (double)color.G / 255.0, (double)color.B / 255.0, (double)color.A / 255.0);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003A31 File Offset: 0x00001C31
		public static void SetColor(this Context cr, Gdk.Color color)
		{
			cr.SetSourceRGB((double)color.Red / 255.0, (double)color.Green / 255.0, (double)color.Blue / 255.0);
		}
	}
}
