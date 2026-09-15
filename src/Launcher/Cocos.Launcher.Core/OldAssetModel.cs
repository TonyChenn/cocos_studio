using System;
using System.Diagnostics;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(0)]
	internal class OldAssetModel : BaseAssetModel
	{
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		public OldAssetModel()
		{
		}

		public OldAssetModel(Plugin model) : base(model)
		{
		}

		public override bool CanHandle(Plugin pluginModel)
		{
			return string.IsNullOrEmpty(pluginModel.OpenType) || pluginModel.OpenType == OperationType.none.ToString();
		}

		protected override IProgressMonitor OnOpen()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (Platform.IsWindows)
			{
				if (string.Equals(Path.GetExtension(base.AssetInfo.PluginPath), ".exe"))
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo(base.AssetInfo.PluginPath);
					processStartInfo.WorkingDirectory = Path.GetDirectoryName(base.AssetInfo.PluginPath);
					try
					{
						Process.Start(processStartInfo);
						return @default;
					}
					catch (Exception arg)
					{
						LogConfig.Output.Error("程序打开失败：" + arg);
						return @default;
					}
				}
				Process.Start("Explorer", "/select," + base.AssetInfo.PluginPath);
			}
			else if (string.Equals(Path.GetExtension(base.AssetInfo.PluginPath), ".dmg") || string.Equals(Path.GetExtension(base.AssetInfo.PluginPath), ".pkg"))
			{
				Process.Start("open", base.AssetInfo.PluginPath);
			}
			else
			{
				Process.Start("open", "-R " + string.Format("\"{0}\"", base.AssetInfo.PluginPath));
			}
			return @default;
		}
	}
}
