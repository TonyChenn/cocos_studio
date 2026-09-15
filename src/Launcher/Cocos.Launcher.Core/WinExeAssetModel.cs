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
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(6)]
	public class WinExeAssetModel : BaseAssetModel
	{
		public override int Order
		{
			get
			{
				return 6;
			}
		}

		public override bool HasInstalled
		{
			get
			{
				return RegistryServices.IsKeysContainsDisplayName(base.AssetInfo.UninstallName);
			}
		}

		public WinExeAssetModel()
		{
		}

		public WinExeAssetModel(Plugin model) : base(model)
		{
		}

		public override bool CanHandle(Plugin pluginModel)
		{
			return Platform.IsWindows && pluginModel.OpenType == OperationType.exe.ToString();
		}

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

		public override bool UninstallPrompt()
		{
			return base.DeletePrompt(LanguageInfo.Launcher_ConfirmUninstall, base.AssetInfo.PluginName);
		}

		private string GetPluginProgramPath()
		{
			return string.Empty;
		}
	}
}
