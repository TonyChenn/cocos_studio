using System;

namespace Gdk
{
	// Token: 0x0200008D RID: 141
	public static class Colors
	{
		// Token: 0x06000300 RID: 768 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
		public static global::Gdk.Color DrawingToGdkColor(global::System.Drawing.Color color)
		{
			return new global::Gdk.Color(color.R, color.G, color.B);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000C620 File Offset: 0x0000A820
		public static global::System.Drawing.Color GdkToDrawingColor(global::Gdk.Color color)
		{
			return global::System.Drawing.Color.FromArgb((int)(color.Red / 256), (int)(color.Green / 256), (int)(color.Blue / 256));
		}

		// Token: 0x04000382 RID: 898
		public static readonly global::Gdk.Color Red = new global::Gdk.Color(byte.MaxValue, 0, 0);

		// Token: 0x04000383 RID: 899
		public static readonly global::Gdk.Color Green = new global::Gdk.Color(0, byte.MaxValue, 0);

		// Token: 0x04000384 RID: 900
		public static readonly global::Gdk.Color Blue = new global::Gdk.Color(0, 0, byte.MaxValue);

		// Token: 0x04000385 RID: 901
		public static readonly global::Gdk.Color Black = new global::Gdk.Color(0, 0, 0);

		// Token: 0x04000386 RID: 902
		public static readonly global::Gdk.Color White = new global::Gdk.Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}
}
