using System;
using System.Diagnostics;
using System.IO;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(7)]
	public class MacPackageAssetModel : BaseAssetModel
	{
		public override int Order
		{
			get
			{
				return 7;
			}
		}

		public override bool HasInstalled
		{
			get
			{
				return Directory.Exists(base.AssetInfo.MacPath) || File.Exists(base.AssetInfo.MacPath);
			}
		}

		public MacPackageAssetModel()
		{
		}

		public MacPackageAssetModel(Plugin model) : base(model)
		{
		}

		public override bool CanHandle(Plugin pluginModel)
		{
			return Platform.IsMac && pluginModel.OpenType == OperationType.exe.ToString();
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
			return this.HasInstalled;
		}

		protected override IProgressMonitor OnInstall()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (string.Equals(Path.GetExtension(base.AssetInfo.PluginPath), ".dmg") || string.Equals(Path.GetExtension(base.AssetInfo.PluginPath), ".pkg"))
			{
				Process process = new Process();
				process.StartInfo.FileName = "open";
				process.StartInfo.Arguments = "\"" + base.AssetInfo.PluginPath + "\" -W";
				process.Start();
				process.WaitForExit();
			}
			if (!Directory.Exists(base.AssetInfo.MacPath))
			{
				@default.ReportError("", null);
			}
			return @default;
		}

		protected override IProgressMonitor OnUninstall()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			@default.ReportError(null, null);
			return @default;
		}

		public override bool UninstallPrompt()
		{
			string fileName = Path.GetFileName(base.AssetInfo.MacPath);
			MessageBox.Show(string.Format(LanguageInfo.Launcher_MacUninstall, fileName), MessageBoxImage.Other, null, null);
			return false;
		}

		protected override IProgressMonitor OnOpen()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (!this.HasInstalled)
			{
				@default.ReportError(null, null);
				return @default;
			}
			string macPath = base.AssetInfo.MacPath;
			if (Path.GetFileName(macPath).Contains(".app"))
			{
				Process.Start(new ProcessStartInfo(macPath)
				{
					WorkingDirectory = Path.GetDirectoryName(macPath)
				});
			}
			else
			{
				Process.Start("open", "-R " + string.Format("\"{0}\"", macPath));
			}
			return @default;
		}
	}
}
