using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CocoStudio.Basic
{
	public static class StringExtend
	{
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

		public static bool IsAnyEqual(this string str, params string[] parmas)
		{
			return str.IsAnyEqual(StringComparison.InvariantCultureIgnoreCase, parmas);
		}

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

		public static bool CheckForFileName(this string fileName)
		{
			string pattern = "^[a-zA-Z]:(((\\\\(?! )[^/:*?<>\\\"|\\\\]+)+\\\\?)|(\\\\)?)\\s*$";
			Regex regex = new Regex(pattern);
			return regex.IsMatch("C:\\" + fileName);
		}
	}
}
