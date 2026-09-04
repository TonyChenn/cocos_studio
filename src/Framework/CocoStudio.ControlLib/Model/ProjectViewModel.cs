using System;
using System.Text.RegularExpressions;

namespace CocoStudio.ControlLib.Model
{
	// Token: 0x02000007 RID: 7
	public class ProjectViewModel
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000027D0 File Offset: 0x000009D0
		public static bool IsMatchToName(string name)
		{
			return !Regex.IsMatch(name, "[\\*\\\\/:?<>|\"]") && !string.IsNullOrEmpty(name);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002808 File Offset: 0x00000A08
		public static bool IsMatchToLogin(string name)
		{
			string pattern = "[\\u4e00-\\u9fa5]";
			return !Regex.IsMatch(name, pattern) && !string.IsNullOrEmpty(name);
		}
	}
}
