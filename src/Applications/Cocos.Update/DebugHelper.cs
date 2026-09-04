using System;
using System.IO;
using Microsoft.Win32;

namespace Cocos.Update
{
	// Token: 0x02000002 RID: 2
	internal class DebugHelper
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002066 File Offset: 0x00000266
		public static void WriteLogInfo(string logInfo)
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002068 File Offset: 0x00000268
		public static string GetUserCustomerConfigFloder()
		{
			return Path.Combine(DebugHelper.GetConfigFolderLocation(), "Cocos", "CocosStudio2");
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002080 File Offset: 0x00000280
		private static string GetConfigFolderLocation()
		{
			if (PlatformHelper.IsMacPlatform)
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				return Path.Combine(folderPath, "Library", "Application Support");
			}
			try
			{
				object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ChuKong\\CocoStudio\\AppSourceKey", "AppSourceFolder", null);
				if (value != null)
				{
					return value.ToString();
				}
			}
			catch (Exception ex)
			{
				DebugHelper.WriteLogInfo("读取install.config失败：\r\n" + ex.ToString());
			}
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		// Token: 0x04000001 RID: 1
		private static string logFilePath = Path.Combine(DebugHelper.GetUserCustomerConfigFloder(), "AutoUpdateLogInfo.txt");
	}
}
