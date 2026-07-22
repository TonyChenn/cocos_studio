using System;
using System.Drawing;
using Cairo;
using Gdk;

namespace Modules.Communal.Render.Model
{
	// Token: 0x0200002E RID: 46
	public static class RulerConsts
	{
		// Token: 0x060001DB RID: 475 RVA: 0x0000AC70 File Offset: 0x00008E70
		public static void SetColor(this Context cr, System.Drawing.Color color)
		{
			cr.SetSourceRGBA((double)color.R / 255.0, (double)color.G / 255.0, (double)color.B / 255.0, (double)color.A / 255.0);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000ACCD File Offset: 0x00008ECD
		public static void SetColor(this Context cr, Gdk.Color color)
		{
			cr.SetSourceRGB((double)color.Red / 255.0, (double)color.Green / 255.0, (double)color.Blue / 255.0);
		}

		// Token: 0x0400007D RID: 125
		public const int RulerHeight = 18;

		// Token: 0x0400007E RID: 126
		public const double LongLineHeight = 10.0;

		// Token: 0x0400007F RID: 127
		public const double CentreLineHeight = 7.0;

		// Token: 0x04000080 RID: 128
		public const double ShortLineHeight = 5.0;

		// Token: 0x04000081 RID: 129
		public static System.Drawing.Color Color_Background = System.Drawing.Color.FromArgb(255, 56, 55, 58);

		// Token: 0x04000082 RID: 130
		public static System.Drawing.Color Color_Line = System.Drawing.Color.FromArgb(255, 113, 113, 116);

		// Token: 0x04000083 RID: 131
		public static System.Drawing.Color Color_Text = System.Drawing.Color.FromArgb(255, 221, 221, 221);
	}
}
