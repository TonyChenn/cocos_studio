using System;
using System.IO;
using Microsoft.Win32;

namespace Cocos.Update
{
	internal class DebugHelper
	{
		public static void WriteLogInfo(string logInfo)
		{
		}

		public static string GetUserCustomerConfigFloder()
		{
			return Path.Combine(DebugHelper.GetConfigFolderLocation(), "Cocos", "CocosStudio2");
		}

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

		private static string logFilePath = Path.Combine(DebugHelper.GetUserCustomerConfigFloder(), "AutoUpdateLogInfo.txt");
	}
}
