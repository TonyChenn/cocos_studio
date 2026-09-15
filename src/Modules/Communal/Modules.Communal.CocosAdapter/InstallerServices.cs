using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter
{
	public class InstallerServices
	{
		public event EventHandler<EventArgs> InstallFinished;

		internal InstallerServices()
		{
		}

		public void StartInstaller(bool initSelectAndroid = true, bool initSelectFramework = true)
		{
			if (!Services.NetworkService.IsOK)
			{
				MessageBox.Show(LanguageInfo.Tooltip_NoNetwork, MessageBoxImage.Error, null, null);
				return;
			}
			string installerPath = Path.Combine(Option.AssemblyDir, "Cocos.Installer.exe");
			if (MonoDevelop.Core.Platform.IsMac)
			{
				installerPath = "/Applications/Cocos/Cocos Studio 2.app/Contents/MacOS/Cocos Installer.app/Contents/MacOS/Cocos.Installer";
			}
			Task task = new Task(delegate()
			{
				try
				{
					using (System.Diagnostics.Process process = new System.Diagnostics.Process())
					{
						string arguments = this.CreateStartArguments(initSelectAndroid, initSelectFramework);
						ProcessStartInfo processStartInfo = new ProcessStartInfo(installerPath, arguments);
						if (MonoDevelop.Core.Platform.IsWindows)
						{
							processStartInfo.Verb = "runas";
						}
						process.StartInfo = processStartInfo;
						process.Start();
						process.WaitForExit();
						this.RefreshAndroidConfig();
						GLib.Timeout.Add(0U, delegate
						{
							if (this.InstallFinished != null)
							{
								this.InstallFinished(this, new EventArgs());
							}
							return false;
						});
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("启动Installer时出错", exception);
				}
			});
			task.Start();
		}

		private string CreateStartArguments(bool selectAndroid, bool selectFramework)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Install");
			if (selectAndroid)
			{
				stringBuilder.Append(" -Android");
			}
			if (selectFramework)
			{
				stringBuilder.Append(" -Framework");
			}
			return stringBuilder.ToString();
		}

		public void RefreshAndroidConfig()
		{
			bool flag = false;
			string userConfigFileByName = Option.GetUserConfigFileByName("UnmergedConfig.xml");
			if (File.Exists(userConfigFileByName))
			{
				string text = string.Empty;
				string text2 = string.Empty;
				string text3 = string.Empty;
				string text4 = string.Empty;
				try
				{
					XElement xelement = XElement.Load(userConfigFileByName);
					IEnumerable<XElement> enumerable = xelement.Elements();
					foreach (XElement xelement2 in enumerable)
					{
						if (xelement2.Name.ToString().Equals("SDK"))
						{
							text = xelement2.Value;
						}
						else if (xelement2.Name.ToString().Equals("NDK"))
						{
							text2 = xelement2.Value;
						}
						else if (xelement2.Name.ToString().Equals("JDK"))
						{
							text3 = xelement2.Value;
						}
						else if (xelement2.Name.ToString().Equals("CocosFramework"))
						{
							text4 = PackageServices.GetANTPath();
						}
						else if (xelement2.Name.ToString().Equals("InstallDir"))
						{
							Services.RecentFileService.LastInstallDir = xelement2.Value;
						}
					}
					if (!string.IsNullOrEmpty(text) && !text.Equals(Option.UserConfig.SDKPath))
					{
						Option.UserConfig.SDKPath = text;
						flag = true;
					}
					if (!string.IsNullOrEmpty(text2) && !text2.Equals(Option.UserConfig.NDKPath))
					{
						Option.UserConfig.NDKPath = text2;
						flag = true;
					}
					if (!string.IsNullOrEmpty(text3) && !text3.Equals(Option.UserConfig.JDKPath))
					{
						Option.UserConfig.JDKPath = text3;
						flag = true;
					}
					if (!string.IsNullOrEmpty(text4) && !text4.Equals(Option.UserConfig.ANTPath))
					{
						Option.UserConfig.ANTPath = text4;
						flag = true;
					}
					File.Delete(userConfigFileByName);
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("合并Installer生成的配置信息时出错", exception);
				}
			}
			if (flag)
			{
				Option.UserConfig.Save();
			}
		}

		public const string conifgFileName = "UnmergedConfig.xml";

		public const string frameworkNodeName = "CocosFramework";

		public const string sdkNodeName = "SDK";

		public const string ndkNodeName = "NDK";

		public const string jdkNodeName = "JDK";

		public const string installDirNodeName = "InstallDir";
	}
}
