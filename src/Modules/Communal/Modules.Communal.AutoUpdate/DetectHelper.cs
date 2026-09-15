using System;
using System.Diagnostics;
using System.IO;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.AutoUpdate
{
	internal class DetectHelper
	{
		public static LocalUpdateConfig LocalConfig
		{
			get
			{
				if (DetectHelper.localconfig == null)
				{
					DetectHelper.localconfig = new LocalUpdateConfig();
				}
				return DetectHelper.localconfig;
			}
		}

		public static ServerUpdateInfo GetServerUpdateInfo(out string output)
		{
			bool flag = false;
			ServerUpdateInfo serverUpdateInfo = null;
			if (Platform.IsMac)
			{
				foreach (string path in PathHelper.MacXmlUrls)
				{
					serverUpdateInfo = new MacServerInfo(path);
					if (serverUpdateInfo.LinkSuccess)
					{
						flag = true;
					}
					if (serverUpdateInfo.LoadSuccess)
					{
						output = "";
						return serverUpdateInfo;
					}
				}
			}
			else
			{
				foreach (string path2 in PathHelper.WindowsXmlUrls)
				{
					serverUpdateInfo = new WinServerInfo(path2);
					if (serverUpdateInfo.LinkSuccess)
					{
						flag = true;
					}
					if (serverUpdateInfo.LoadSuccess)
					{
						output = "";
						return serverUpdateInfo;
					}
				}
			}
			if (!flag)
			{
				output = LanguageInfo.AutoUpdate_NoNetWork;
				return null;
			}
			output = LanguageInfo.AutoUpdate_FailedToGetInfo;
			return null;
		}

		public static bool CheckCanUpdate(ServerUpdateInfo serverInfo, out string output)
		{
			if (serverInfo.RequiredMinVersion != null && Option.EditorVersion < new Version(serverInfo.RequiredMinVersion))
			{
				output = LanguageInfo.AutoUpdate_CanNotUpdate;
				return false;
			}
			output = "";
			return true;
		}

		public static bool CheckNeedUpdateStudio(ServerUpdateInfo serverInfo, out string output)
		{
			if (Option.EditorVersion >= new Version(serverInfo.AppVersion))
			{
				output = LanguageInfo.MessageBox_Content106;
				return false;
			}
			output = "";
			return true;
		}

		public static bool CheckNeedUpdateRuntime(ServerUpdateInfo serverInfo, out string output)
		{
			if (RuntimeHelper.RuntimeVersion >= new Version(serverInfo.RuntimeVersion))
			{
				output = LanguageInfo.AutoUpdate_RuntimeUpToDate;
				return false;
			}
			output = "";
			return true;
		}

		public static bool CheckNeedShowDialog(bool downStudio, bool downRuntime, ServerUpdateInfo serverInfo = null)
		{
			if (serverInfo != null)
			{
				if (downStudio)
				{
					if (string.IsNullOrEmpty(DetectHelper.LocalConfig.AppVersion))
					{
						DetectHelper.ResetLocalConfig(serverInfo.AppVersion, serverInfo.RuntimeVersion);
						return true;
					}
					Version v = new Version(serverInfo.AppVersion);
					Version v2 = new Version(DetectHelper.LocalConfig.AppVersion);
					if (v > v2)
					{
						DetectHelper.ResetLocalConfig(serverInfo.AppVersion, serverInfo.RuntimeVersion);
						return true;
					}
				}
				if (downRuntime)
				{
					if (string.IsNullOrEmpty(DetectHelper.LocalConfig.RuntimeVersion))
					{
						DetectHelper.ResetLocalConfig(serverInfo.AppVersion, serverInfo.RuntimeVersion);
						return true;
					}
					Version v3 = new Version(serverInfo.RuntimeVersion);
					Version v4 = new Version(DetectHelper.LocalConfig.RuntimeVersion);
					if (v3 > v4)
					{
						DetectHelper.ResetLocalConfig(serverInfo.AppVersion, serverInfo.RuntimeVersion);
						return true;
					}
				}
			}
			return !DetectHelper.LocalConfig.IsNeverRemind && (!DetectHelper.LocalConfig.IsSkipToday || Math.Abs((DetectHelper.LocalConfig.InfoTime - DateTime.Now).Days) >= 1);
		}

		public static bool CheckIsCocosRunning()
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension("Cocos.exe");
			if (Process.GetProcessesByName(fileNameWithoutExtension).Length > 0)
			{
				return true;
			}
			string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension("CocosStudio.exe");
			return Process.GetProcessesByName(fileNameWithoutExtension2).Length > 1;
		}

		private static void ResetLocalConfig(string appVer, string runtimeVer)
		{
			DetectHelper.LocalConfig.IsNeverRemind = false;
			DetectHelper.LocalConfig.IsSkipToday = false;
			DetectHelper.LocalConfig.AppVersion = appVer;
			DetectHelper.localconfig.RuntimeVersion = runtimeVer;
			DetectHelper.LocalConfig.SaveToFile();
		}

		private static LocalUpdateConfig localconfig;
	}
}
