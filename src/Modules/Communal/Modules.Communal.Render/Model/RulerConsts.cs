using System;
using System.Drawing;
using Cairo;
using Gdk;

namespace Modules.Communal.Render.Model
{
	public static class RulerConsts
	{
		public static void SetColor(this Context cr, System.Drawing.Color color)
		{
			cr.SetSourceRGBA((double)color.R / 255.0, (double)color.G / 255.0, (double)color.B / 255.0, (double)color.A / 255.0);
		}

		public static void SetColor(this Context cr, Gdk.Color color)
		{
			cr.SetSourceRGB((double)color.Red / 255.0, (double)color.Green / 255.0, (double)color.Blue / 255.0);
		}

		public const int RulerHeight = 18;

		public const double LongLineHeight = 10.0;

		public const double CentreLineHeight = 7.0;

		public const double ShortLineHeight = 5.0;

		public static System.Drawing.Color Color_Background = System.Drawing.Color.FromArgb(255, 56, 55, 58);

		public static System.Drawing.Color Color_Line = System.Drawing.Color.FromArgb(255, 113, 113, 116);

		public static System.Drawing.Color Color_Text = System.Drawing.Color.FromArgb(255, 221, 221, 221);
	}
}
