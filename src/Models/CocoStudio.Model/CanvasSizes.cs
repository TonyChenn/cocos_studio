using System;

namespace CocoStudio.Model
{
	public static class CanvasSizes
	{
		public static string FormatString(this SizeF size)
		{
			return string.Format("{0} * {1}", size.Width, size.Height);
		}

		public static SizeF ConvertToSize(this string sizeStr)
		{
			double num = Convert.ToDouble(sizeStr.Substring(0, sizeStr.IndexOf("*") - 1));
			double num2 = Convert.ToDouble(sizeStr.Substring(sizeStr.IndexOf("*") + 1));
			return new SizeF((float)((int)num), (float)((int)num2));
		}
	}
}
