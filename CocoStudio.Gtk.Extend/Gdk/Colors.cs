using System;
using System.Drawing;

namespace Gdk
{
	// Token: 0x0200008D RID: 141
	public static class Colors
	{
		// Token: 0x06000300 RID: 768 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
		public static Color DrawingToGdkColor(Color color)
		{
			return new Color(color.R, color.G, color.B);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000C620 File Offset: 0x0000A820
		public static Color GdkToDrawingColor(Color color)
		{
			return Color.FromArgb((int)(color.Red / 256), (int)(color.Green / 256), (int)(color.Blue / 256));
		}

		// Token: 0x04000382 RID: 898
		public static readonly Color Red = new Color(byte.MaxValue, 0, 0);

		// Token: 0x04000383 RID: 899
		public static readonly Color Green = new Color(0, byte.MaxValue, 0);

		// Token: 0x04000384 RID: 900
		public static readonly Color Blue = new Color(0, 0, byte.MaxValue);

		// Token: 0x04000385 RID: 901
		public static readonly Color Black = new Color(0, 0, 0);

		// Token: 0x04000386 RID: 902
		public static readonly Color White = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}
}
