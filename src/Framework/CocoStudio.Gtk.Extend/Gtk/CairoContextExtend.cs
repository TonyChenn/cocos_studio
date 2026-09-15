using System;
using System.Drawing;
using Cairo;
using Gdk;

namespace Gtk
{
	public static class CairoContextExtend
	{
		public static void SetColor(this Context cr, System.Drawing.Color color)
		{
			cr.SetSourceRGBA((double)color.R / 255.0, (double)color.G / 255.0, (double)color.B / 255.0, (double)color.A / 255.0);
		}

		public static void SetColor(this Context cr, Gdk.Color color)
		{
			cr.SetSourceRGB((double)color.Red / 255.0, (double)color.Green / 255.0, (double)color.Blue / 255.0);
		}
	}
}
