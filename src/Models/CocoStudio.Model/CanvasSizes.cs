using System;

namespace CocoStudio.Model
{
	// Token: 0x020000C0 RID: 192
	public static class CanvasSizes
	{
		// Token: 0x06000610 RID: 1552 RVA: 0x0001958C File Offset: 0x0001778C
		public static string FormatString(this SizeF size)
		{
			return string.Format("{0} * {1}", size.Width, size.Height);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000195C0 File Offset: 0x000177C0
		public static SizeF ConvertToSize(this string sizeStr)
		{
			double num = Convert.ToDouble(sizeStr.Substring(0, sizeStr.IndexOf("*") - 1));
			double num2 = Convert.ToDouble(sizeStr.Substring(sizeStr.IndexOf("*") + 1));
			return new SizeF((float)((int)num), (float)((int)num2));
		}
	}
}
