using System;
using System.Text.RegularExpressions;

namespace CocoStudio.ControlLib.Model
{
	public class ProjectViewModel
	{
		public static bool IsMatchToName(string name)
		{
			return !Regex.IsMatch(name, "[\\*\\\\/:?<>|\"]") && !string.IsNullOrEmpty(name);
		}

		public static bool IsMatchToLogin(string name)
		{
			string pattern = "[\\u4e00-\\u9fa5]";
			return !Regex.IsMatch(name, pattern) && !string.IsNullOrEmpty(name);
		}
	}
}
