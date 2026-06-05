using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CocoStudio.Basic
{
	// Token: 0x0200000A RID: 10
	public static class StringExtend
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002634 File Offset: 0x00000834
		public static string ReplaceLast(this string str, string oldStr, string newStr, StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase)
		{
			newStr = (newStr ?? string.Empty);
			if (!string.IsNullOrWhiteSpace(str))
			{
				int num = str.LastIndexOf(oldStr, comparisonType);
				if (num != -1)
				{
					return new string(str.Take(num).ToArray<char>()) + newStr;
				}
			}
			return str;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002690 File Offset: 0x00000890
		public static bool IsAnyEqual(this string str, params string[] parmas)
		{
			return str.IsAnyEqual(StringComparison.InvariantCultureIgnoreCase, parmas);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000026E0 File Offset: 0x000008E0
		public static bool IsAnyEqual(this string str, StringComparison comparison, params string[] parmas)
		{
			bool result;
			if (str != null)
			{
				result = (parmas.FirstOrDefault((string item) => item != null && item.Equals(str, comparison)) != null);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000273C File Offset: 0x0000093C
		public static bool CheckForFileName(this string fileName)
		{
			string pattern = "^[a-zA-Z]:(((\\\\(?! )[^/:*?<>\\\"|\\\\]+)+\\\\?)|(\\\\)?)\\s*$";
			Regex regex = new Regex(pattern);
			return regex.IsMatch("C:\\" + fileName);
		}
	}
}
