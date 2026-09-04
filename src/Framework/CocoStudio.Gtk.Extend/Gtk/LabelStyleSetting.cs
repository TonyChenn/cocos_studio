using System;
using Gdk;
using Pango;

namespace Gtk
{
	// Token: 0x0200008F RID: 143
	public class LabelStyleSetting
	{
		private const double PangoScaleFactor = 1024.0;

		// Token: 0x0400038A RID: 906
		public static Gdk.Color LabelHoverColor = new Gdk.Color(53, 165, byte.MaxValue);

		// Token: 0x0400038B RID: 907
		public static Gdk.Color LabelNormalColor = new Gdk.Color(20, 148, byte.MaxValue);

		// Token: 0x0400038C RID: 908
		public static Gdk.Color LabelPressedColor = new Gdk.Color(14, 131, 230);

		// Token: 0x0400038D RID: 909
		public static Gdk.Color LabelDisabledColor = new Gdk.Color(60, 59, 64);

		// Token: 0x0400038E RID: 910
		public static Gdk.Color RootBG = new Gdk.Color(68, 68, 68);

		// Token: 0x0400038F RID: 911
		public static Gdk.Color LeftDarkBG = new Gdk.Color(50, 50, 50);

		// Token: 0x04000390 RID: 912
		public static Gdk.Color SampleNormalBG = new Gdk.Color(50, 50, 50);

		// Token: 0x04000391 RID: 913
		public static Gdk.Color SampleHoverBG = new Gdk.Color(100, 100, 100);

		// Token: 0x04000392 RID: 914
		public static Gdk.Color SeperatorColor = new Gdk.Color(83, 83, 83);

		// Token: 0x04000393 RID: 915
		public static Gdk.Color ToolTipBG = new Gdk.Color(86, 86, 91);

		// Token: 0x04000394 RID: 916
		public static string ToolTipFamily = "冬青黑体简体中文 W6";

		// Token: 0x04000395 RID: 917
		public static double LinkSize = 16.0 * PangoScaleFactor;

		// Token: 0x04000396 RID: 918
		public static double MainTitleSize = 28.0 * PangoScaleFactor;

		// Token: 0x04000397 RID: 919
		public static double LeftTitleSize = 20.0 * PangoScaleFactor;

		// Token: 0x04000398 RID: 920
		public static double RightTitleSize = 28.0 * PangoScaleFactor;

		// Token: 0x04000399 RID: 921
		public static double SampleDesSize = 12.0 * PangoScaleFactor;

		// Token: 0x0400039A RID: 922
		public static double SampleTitleSize = 14.0 * PangoScaleFactor;

		// Token: 0x0400039B RID: 923
		public static double LoginLinkSize = 14.0 * PangoScaleFactor;

		// Token: 0x0400039C RID: 924
		public static double TooltipDesSize = 12.0 * PangoScaleFactor;
	}
}
