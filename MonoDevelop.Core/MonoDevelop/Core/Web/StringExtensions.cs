using System;

namespace MonoDevelop.Core.Web
{
	// Token: 0x02000263 RID: 611
	internal static class StringExtensions
	{
		// Token: 0x06001635 RID: 5685 RVA: 0x00059D5E File Offset: 0x00057F5E
		public static string SafeTrim(this string value)
		{
			if (value != null)
			{
				return value.Trim();
			}
			return null;
		}
	}
}
