using System;

namespace Gdk
{
	public static class Colors
	{
		public static global::Gdk.Color DrawingToGdkColor(global::System.Drawing.Color color)
		{
			return new global::Gdk.Color(color.R, color.G, color.B);
		}

		public static global::System.Drawing.Color GdkToDrawingColor(global::Gdk.Color color)
		{
			return global::System.Drawing.Color.FromArgb((int)(color.Red / 256), (int)(color.Green / 256), (int)(color.Blue / 256));
		}

		public static readonly global::Gdk.Color Red = new global::Gdk.Color(byte.MaxValue, 0, 0);

		public static readonly global::Gdk.Color Green = new global::Gdk.Color(0, byte.MaxValue, 0);

		public static readonly global::Gdk.Color Blue = new global::Gdk.Color(0, 0, byte.MaxValue);

		public static readonly global::Gdk.Color Black = new global::Gdk.Color(0, 0, 0);

		public static readonly global::Gdk.Color White = new global::Gdk.Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}
}
