using System;
using System.Diagnostics;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000024 RID: 36
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(0)]
	internal class OldAssetModel : BaseAssetModel
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00007AF7 File Offset: 0x00005CF7
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007AFA File Offset: 0x00005CFA
		public OldAssetModel()
		{
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007B02 File Offset: 0x00005D02
		public OldAssetModel(Plugin model) : base(model)
		{
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007B0B File Offset: 0x00005D0B
		public override bool CanHandle(Plugin pluginModel)
		{
			return string.IsNullOrEmpty(pluginModel.OpenType) || pluginModel.OpenType == OperationType.none.ToString();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007B38 File Offset: 0x00005D38
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
