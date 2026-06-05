using System;
using System.Text.RegularExpressions;

namespace Gtk
{
	// Token: 0x02000076 RID: 118
	public class RegexModel
	{
		// Token: 0x060002B0 RID: 688 RVA: 0x0000AA38 File Offset: 0x00008C38
		public static bool HasChinese(string text)
		{
			return Regex.IsMatch(text, "[\\u4e00-\\u9fa5]");
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000AA58 File Offset: 0x00008C58
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

		// Token: 0x060002B2 RID: 690 RVA: 0x0000AA9C File Offset: 0x00008C9C
		public static bool HasSpecialCharacter(string text)
		{
			return !Regex.IsMatch(text, "^[a-zA-Z0-9_\\u4e00-\\u9fa5]+$");
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000AABC File Offset: 0x00008CBC
		public static bool IsValidObjectName(string name)
		{
			return Regex.IsMatch(name, "^[A-Za-z]$|^[A-Za-z]+[A-Za-z0-9_]*[A-Za-z0-9_]$");
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000AADC File Offset: 0x00008CDC
		public static bool IsNumber(string text)
		{
			return Regex.IsMatch(text, "^[0-9]*$");
		}

		// Token: 0x0400033D RID: 829
		public const string ResourcePathRegex = "^[A-Za-z0-9, ._@-]+$";

		// Token: 0x0400033E RID: 830
		public const string EmailRegex = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
	}
}
