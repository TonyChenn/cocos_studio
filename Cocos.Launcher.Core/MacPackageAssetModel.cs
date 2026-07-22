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
	// Token: 0x0200001A RID: 26
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(7)]
	public class MacPackageAssetModel : BaseAssetModel
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00006091 File Offset: 0x00004291
		public override int Order
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00006094 File Offset: 0x00004294
		public override bool HasInstalled
		{
			get
			{
				return Directory.Exists(base.AssetInfo.MacPath) || File.Exists(base.AssetInfo.MacPath);
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000060C8 File Offset: 0x000042C8
		public MacPackageAssetModel()
		{
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000060D0 File Offset: 0x000042D0
		public MacPackageAssetModel(Plugin model) : base(model)
		{
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000060D9 File Offset: 0x000042D9
		public override bool CanHandle(Plugin pluginModel)
		{
			return Platform.IsMac && pluginModel.OpenType == OperationType.exe.ToString();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000060FD File Offset: 0x000042FD
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

		// Token: 0x060000F7 RID: 247 RVA: 0x00006125 File Offset: 0x00004325
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

		// Token: 0x060000F8 RID: 248 RVA: 0x00006145 File Offset: 0x00004345
		public override bool ExistsToFull()
		{
			return this.HasInstalled;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006150 File Offset: 0x00004350
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

		// Token: 0x060000FA RID: 250 RVA: 0x00006208 File Offset: 0x00004408
		protected override IProgressMonitor OnUninstall()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			@default.ReportError(null, null);
			return @default;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000622C File Offset: 0x0000442C
		public override bool UninstallPrompt()
		{
			string fileName = Path.GetFileName(base.AssetInfo.MacPath);
			MessageBox.Show(string.Format(LanguageInfo.Launcher_MacUninstall, fileName), MessageBoxImage.Other, null, null);
			return false;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006260 File Offset: 0x00004460
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
