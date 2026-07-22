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
	// Token: 0x0200000E RID: 14
	public class InstallerServices
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000059 RID: 89 RVA: 0x00003024 File Offset: 0x00001224
		// (remove) Token: 0x0600005A RID: 90 RVA: 0x0000305C File Offset: 0x0000125C
		public event EventHandler<EventArgs> InstallFinished;

		// Token: 0x0600005B RID: 91 RVA: 0x00003091 File Offset: 0x00001291
		internal InstallerServices()
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003190 File Offset: 0x00001390
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

		// Token: 0x0600005D RID: 93 RVA: 0x00003214 File Offset: 0x00001414
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

		// Token: 0x0600005E RID: 94 RVA: 0x00003258 File Offset: 0x00001458
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

		// Token: 0x0400001F RID: 31
		public const string conifgFileName = "UnmergedConfig.xml";

		// Token: 0x04000020 RID: 32
		public const string frameworkNodeName = "CocosFramework";

		// Token: 0x04000021 RID: 33
		public const string sdkNodeName = "SDK";

		// Token: 0x04000022 RID: 34
		public const string ndkNodeName = "NDK";

		// Token: 0x04000023 RID: 35
		public const string jdkNodeName = "JDK";

		// Token: 0x04000024 RID: 36
		public const string installDirNodeName = "InstallDir";
	}
}
