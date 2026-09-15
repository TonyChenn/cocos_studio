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
	[AssetOrder(1)]
	[Extension(typeof(BaseAssetModel))]
	internal class PluginAssetModel : BaseAssetModel
	{
		public override int Order
		{
			get
			{
				return 1;
			}
		}

		public PluginAssetModel()
		{
		}

		public PluginAssetModel(Plugin model) : base(model)
		{
		}

		public override bool CanHandle(Plugin info)
		{
			return info.OpenType == OperationType.plugin.ToString();
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

		public override bool ExistsToFull()
		{
			return false;
		}

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

		private string GetModelToUnZipDir()
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(base.AssetInfo.PluginFullName);
			return Path.Combine(Option.AddinLocationFolder, fileNameWithoutExtension);
		}

		protected override void OnDelete()
		{
			if (File.Exists(base.AssetInfo.PluginPath))
			{
				File.Delete(base.AssetInfo.PluginPath);
			}
		}
	}
}
