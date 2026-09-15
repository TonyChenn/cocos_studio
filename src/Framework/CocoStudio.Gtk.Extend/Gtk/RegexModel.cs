using System;
using System.Text.RegularExpressions;

namespace Gtk
{
	public class RegexModel
	{
		public static bool HasChinese(string text)
		{
			return Regex.IsMatch(text, "[\\u4e00-\\u9fa5]");
		}

		public static bool IsSystemReserveName(string fileName)
		{
			bool result;
			if (string.IsNullOrWhiteSpace(fileName))
			{
				result = false;
			}
			else
			{
				Regex regex = new Regex("^(com\\d|aux|prn|con|nul|lpt\\d){1}($|\\.$|\\..)", RegexOptions.IgnoreCase);
				result = regex.IsMatch(fileName);
			}
			return result;
		}

		public static bool HasSpecialCharacter(string text)
		{
			return !Regex.IsMatch(text, "^[a-zA-Z0-9_\\u4e00-\\u9fa5]+$");
		}

		public static bool IsValidObjectName(string name)
		{
			return Regex.IsMatch(name, "^[A-Za-z]$|^[A-Za-z]+[A-Za-z0-9_]*[A-Za-z0-9_]$");
		}

		public static bool IsNumber(string text)
		{
			return Regex.IsMatch(text, "^[0-9]*$");
		}

		public const string ResourcePathRegex = "^[A-Za-z0-9, ._@-]+$";

		public const string EmailRegex = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
	}
}
