using System;
using Gdk;
using Pango;

namespace Gtk
{
	public class LabelStyleSetting
	{
		private const double PangoScaleFactor = 1024.0;

		public static Gdk.Color LabelHoverColor = new Gdk.Color(53, 165, byte.MaxValue);

		public static Gdk.Color LabelNormalColor = new Gdk.Color(20, 148, byte.MaxValue);

		public static Gdk.Color LabelPressedColor = new Gdk.Color(14, 131, 230);

		public static Gdk.Color LabelDisabledColor = new Gdk.Color(60, 59, 64);

		public static Gdk.Color RootBG = new Gdk.Color(68, 68, 68);

		public static Gdk.Color LeftDarkBG = new Gdk.Color(50, 50, 50);

		public static Gdk.Color SampleNormalBG = new Gdk.Color(50, 50, 50);

		public static Gdk.Color SampleHoverBG = new Gdk.Color(100, 100, 100);

		public static Gdk.Color SeperatorColor = new Gdk.Color(83, 83, 83);

		public static Gdk.Color ToolTipBG = new Gdk.Color(86, 86, 91);

		public static string ToolTipFamily = "冬青黑体简体中文 W6";

		public static double LinkSize = 16.0 * PangoScaleFactor;

		public static double MainTitleSize = 28.0 * PangoScaleFactor;

		public static double LeftTitleSize = 20.0 * PangoScaleFactor;

		public static double RightTitleSize = 28.0 * PangoScaleFactor;

		public static double SampleDesSize = 12.0 * PangoScaleFactor;

		public static double SampleTitleSize = 14.0 * PangoScaleFactor;

		public static double LoginLinkSize = 14.0 * PangoScaleFactor;

		public static double TooltipDesSize = 12.0 * PangoScaleFactor;
	}
}
