using System;
using System.Diagnostics;
using System.IO;
using CocoStudio.Core;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(5)]
	internal class ResourceAssetModel : BaseAssetModel
	{
		public override int Order
		{
			get
			{
				return 5;
			}
		}

		public ResourceAssetModel()
		{
		}

		public ResourceAssetModel(Plugin model) : base(model)
		{
		}

		public override bool CanHandle(Plugin info)
		{
			return info.OpenType == OperationType.source.ToString() || info.OpenType == OperationType.other.ToString();
		}

		public override bool InitRunMode()
		{
			if (!base.InitRunMode())
			{
				base.RunMode = RunModeEnum.Open;
			}
			return true;
		}

		protected override IProgressMonitor OnOpen()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (!File.Exists(base.AssetInfo.PluginPath))
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

		protected override IProgressMonitor OnInstall()
		{
			return this.OnOpen();
		}
	}
}
