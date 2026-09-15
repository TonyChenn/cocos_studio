using System;
using System.Diagnostics;
using System.IO;
using Cocos.Launcher.Control;
using Cocos.Launcher.Library;
using CocoStudio.Core;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(2)]
	internal class SamplesAssetModel : BaseAssetModel
	{
		public override int Order
		{
			get
			{
				return 2;
			}
		}

		public SamplesAssetModel()
		{
		}

		public SamplesAssetModel(Plugin model) : base(model)
		{
		}

		public override bool CanHandle(Plugin info)
		{
			return info.OpenType == OperationType.cocos.ToString();
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
			zipModel.UnZip(base.AssetInfo.PluginPath, ConstantConfig.Paths.DownloadDemoPath);
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
			string modelToUnZipDir = this.GetModelToUnZipDir();
			bool flag = Directory.Exists(modelToUnZipDir);
			if (flag)
			{
				base.AssetInfo.UnZipPath = modelToUnZipDir;
			}
			return flag;
		}

		private string GetModelToUnZipDir()
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(base.AssetInfo.PluginFullName);
			return Path.Combine(ConstantConfig.Paths.DownloadDemoPath, fileNameWithoutExtension);
		}

		protected override void OnDelete()
		{
			if (Directory.Exists(base.AssetInfo.UnZipPath))
			{
				Directory.Delete(base.AssetInfo.UnZipPath, true);
			}
		}
	}
}
