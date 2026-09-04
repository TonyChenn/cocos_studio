using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using CocoStudio.Core;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200001C RID: 28
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(6)]
	public class WinExeAssetModel : BaseAssetModel
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00006493 File Offset: 0x00004693
		public override int Order
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00006498 File Offset: 0x00004698
		public override bool HasInstalled
		{
			get
			{
				return RegistryServices.IsKeysContainsDisplayName(base.AssetInfo.UninstallName);
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000064B7 File Offset: 0x000046B7
		public WinExeAssetModel()
		{
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000064BF File Offset: 0x000046BF
		public WinExeAssetModel(Plugin model) : base(model)
		{
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000064C8 File Offset: 0x000046C8
		public override bool CanHandle(Plugin pluginModel)
		{
			return Platform.IsWindows && pluginModel.OpenType == OperationType.exe.ToString();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000064EC File Offset: 0x000046EC
		public override bool InitRunMode()
		{
			if (!base.InitRunMode())
			{
				if (base.AssetInfo.IsInstalled)
				{
					base.RunMode = RunModeEnum.Open;
				}
				else
				{
					base.RunMode = RunModeEnum.Install;
				}
			}
			return true;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006514 File Offset: 0x00004714
		public override bool InitUninstallMode()
		{
			if (base.AssetInfo.IsInstalled)
			{
				base.UninstallMode = UninstallModeEnum.Uninstall;
			}
			else
			{
				base.UninstallMode = UninstallModeEnum.Delete;
			}
			return true;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006534 File Offset: 0x00004734
		public override bool ExistsToFull()
		{
			bool result = true;
			string displayVersion = RegistryServices.GetDisplayVersion(base.AssetInfo.UninstallName);
			if (string.IsNullOrEmpty(displayVersion) || !string.Equals(displayVersion, base.AssetInfo.PluginVersion))
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006574 File Offset: 0x00004774
		protected override IProgressMonitor OnInstall()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			Process process = Process.Start(new ProcessStartInfo(base.AssetInfo.PluginPath)
			{
				WorkingDirectory = Path.GetDirectoryName(base.AssetInfo.PluginPath)
			});
			process.WaitForExit();
			if (!this.HasInstalled)
			{
				@default.ReportError(null, null);
			}
			return @default;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000065D4 File Offset: 0x000047D4
		protected override IProgressMonitor OnUninstall()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (base.AssetInfo.IsUninstall)
			{
				string uninstallString = RegistryServices.GetUninstallString(base.AssetInfo.UninstallName);
				Process process = new Process();
				if (File.Exists(uninstallString))
				{
					process.StartInfo.FileName = uninstallString;
				}
				else
				{
					process.StartInfo.FileName = "msiexec.exe";
					process.StartInfo.Arguments = "/x{" + uninstallString + "}";
				}
				process.Start();
				process.WaitForExit();
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + process.Id);
				ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
				foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					int processId = Convert.ToInt32(managementObject["ProcessID"]);
					Process.GetProcessById(processId).WaitForExit();
				}
				if (this.HasInstalled)
				{
					@default.ReportError(null, null);
				}
			}
			else
			{
				@default.ReportError("此插件不支持卸载: " + base.AssetInfo.UninstallName, null);
			}
			return @default;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006714 File Offset: 0x00004914
		protected override IProgressMonitor OnOpen()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (base.AssetInfo.IsUninstall)
			{
				string text = string.Empty;
				string uninstallString = RegistryServices.GetUninstallString(base.AssetInfo.UninstallName);
				if (File.Exists(uninstallString))
				{
					text = Path.Combine(Path.GetDirectoryName(uninstallString), base.AssetInfo.ProcedureExeName);
				}
				else
				{
					string installLocation = RegistryServices.GetInstallLocation(base.AssetInfo.UninstallName);
					if (Directory.Exists(installLocation))
					{
						text = Path.Combine(installLocation, base.AssetInfo.ProcedureExeName);
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					@default.ReportError(null, null);
					return @default;
				}
				if (string.IsNullOrEmpty(base.AssetInfo.ProcedureExeName) || Directory.Exists(text))
				{
					Process.Start("Explorer", "/select," + text);
					return @default;
				}
				if (!File.Exists(text))
				{
					LogConfig.Logger.Error("打开插件失败，启动程序和卸载程序没有在同一个目录:  " + base.AssetInfo.PluginFullName);
					return @default;
				}
				Process.Start(new ProcessStartInfo(text)
				{
					WorkingDirectory = Path.GetDirectoryName(text)
				});
			}
			else
			{
				Process.Start("Explorer", "/select," + base.AssetInfo.PluginPath);
			}
			return @default;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000684C File Offset: 0x00004A4C
		public override bool UninstallPrompt()
		{
			return base.DeletePrompt(LanguageInfo.Launcher_ConfirmUninstall, base.AssetInfo.PluginName);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000686C File Offset: 0x00004A6C
		private string GetPluginProgramPath()
		{
			return string.Empty;
		}
	}
}
