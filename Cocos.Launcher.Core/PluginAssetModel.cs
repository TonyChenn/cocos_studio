using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000021 RID: 33
	[AssetOrder(1)]
	[Extension(typeof(BaseAssetModel))]
	internal class PluginAssetModel : BaseAssetModel
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000787C File Offset: 0x00005A7C
		public override int Order
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000787F File Offset: 0x00005A7F
		public PluginAssetModel()
		{
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00007887 File Offset: 0x00005A87
		public PluginAssetModel(Plugin model) : base(model)
		{
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007890 File Offset: 0x00005A90
		public override bool CanHandle(Plugin info)
		{
			return info.OpenType == OperationType.plugin.ToString();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000078AD File Offset: 0x00005AAD
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

		// Token: 0x06000146 RID: 326 RVA: 0x000078D8 File Offset: 0x00005AD8
		protected override IProgressMonitor OnInstall()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			ZipModel zipModel = new ZipModel();
			zipModel.UnZip(base.AssetInfo.PluginPath, Option.AddinLocationFolder);
			if (!zipModel.IsSucceeded)
			{
				@default.ReportError(zipModel.Error, null);
			}
			string modelToUnZipDir = this.GetModelToUnZipDir();
			if (zipModel.IsSucceeded)
			{
				base.AssetInfo.UnZipPath = modelToUnZipDir;
			}
			else if (Directory.Exists(modelToUnZipDir))
			{
				Directory.Delete(modelToUnZipDir, true);
			}
			return @default;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007950 File Offset: 0x00005B50
		protected override IProgressMonitor OnOpen()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (!File.Exists(base.AssetInfo.UnZipPath) && !Directory.Exists(base.AssetInfo.UnZipPath))
			{
				@default.ReportError(null, null);
				return @default;
			}
			if (Platform.IsWindows)
			{
				Process.Start("Explorer", "/select," + base.AssetInfo.UnZipPath);
			}
			else
			{
				Process.Start("open", "-R " + string.Format("\"{0}\"", base.AssetInfo.UnZipPath));
			}
			return @default;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000079EA File Offset: 0x00005BEA
		public override bool ExistsToFull()
		{
			return false;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000079F0 File Offset: 0x00005BF0
		public override bool CanInstall()
		{
			bool result = true;
			try
			{
				Process[] processesByName = Process.GetProcessesByName("CocosStudio");
				if (processesByName.Count<Process>() > 0)
				{
					result = false;
					MessageBox.Show(LanguageInfo.Launcher_PluginInstallPrompt, MessageBoxImage.Other, null, null);
				}
				else
				{
					string modelToUnZipDir = this.GetModelToUnZipDir();
					if (Directory.Exists(modelToUnZipDir))
					{
						Directory.Delete(modelToUnZipDir, true);
					}
				}
			}
			catch (Exception arg)
			{
				result = false;
				LogConfig.Logger.Error("检测插件安装失败" + arg);
			}
			return result;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007A68 File Offset: 0x00005C68
		private string GetModelToUnZipDir()
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(base.AssetInfo.PluginFullName);
			return Path.Combine(Option.AddinLocationFolder, fileNameWithoutExtension);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007A93 File Offset: 0x00005C93
		protected override void OnDelete()
		{
			if (File.Exists(base.AssetInfo.PluginPath))
			{
				File.Delete(base.AssetInfo.PluginPath);
			}
		}
	}
}
