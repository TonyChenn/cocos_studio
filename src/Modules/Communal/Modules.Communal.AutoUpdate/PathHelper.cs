using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;

namespace Modules.Communal.AutoUpdate
{
	internal static class PathHelper
	{
		public static string AutoUpdateRootPath
		{
			get
			{
				string text = Path.Combine(Option.UserCustomerConfigFolder, "AutoUpdate");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string AutoUpdateTempPath
		{
			get
			{
				string text = Path.Combine(PathHelper.AutoUpdateRootPath, "Temp");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string UpdateLocalConfigPath
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, PathHelper.AutoUpdateRootPath, "AutoUpdateLocalConfig.xml");
			}
		}

		public static string WindowsXmlUrl
		{
			get
			{
				return Option.UpdateServerURL + "WindowsVersionInfo.xml";
			}
		}

		public static IEnumerable<string> WindowsXmlUrls
		{
			get
			{
				foreach (string text in Option.UpdateServerURLs)
				{
					yield return text + "WindowsVersionInfo.xml";
				}
			}
		}

		public static string MacXmlUrl
		{
			get
			{
				return Option.UpdateServerURL + "MacVersionInfo.xml";
			}
		}

		public static IEnumerable<string> MacXmlUrls
		{
			get
			{
				foreach (string text in Option.UpdateServerURLs)
				{
					yield return text + "MacVersionInfo.xml";
				}
			}
		}

		private const string autoUpdateLocalConfigName = "AutoUpdateLocalConfig.xml";

		private const string windowsVerInfoName = "WindowsVersionInfo.xml";

		private const string macVerInfoName = "MacVersionInfo.xml";
	}
}
