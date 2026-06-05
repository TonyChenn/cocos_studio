using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x02000006 RID: 6
	internal static class PathHelper
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002774 File Offset: 0x00000974
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000027A4 File Offset: 0x000009A4
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

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000027D1 File Offset: 0x000009D1
		public static string UpdateLocalConfigPath
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, PathHelper.AutoUpdateRootPath, "AutoUpdateLocalConfig.xml");
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000027E7 File Offset: 0x000009E7
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

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000027F8 File Offset: 0x000009F8
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

		// Token: 0x0400000F RID: 15
		private const string autoUpdateLocalConfigName = "AutoUpdateLocalConfig.xml";

		// Token: 0x04000010 RID: 16
		private const string windowsVerInfoName = "WindowsVersionInfo.xml";

		// Token: 0x04000011 RID: 17
		private const string macVerInfoName = "MacVersionInfo.xml";
	}
}
