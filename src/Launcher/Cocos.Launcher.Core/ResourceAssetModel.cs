using System;
using System.Diagnostics;
using System.IO;
using CocoStudio.Core;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000026 RID: 38
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(5)]
	internal class ResourceAssetModel : BaseAssetModel
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00007DCA File Offset: 0x00005FCA
		public override int Order
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007DCD File Offset: 0x00005FCD
		public ResourceAssetModel()
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007DD5 File Offset: 0x00005FD5
		public ResourceAssetModel(Plugin model) : base(model)
		{
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007DDE File Offset: 0x00005FDE
		public override bool CanHandle(Plugin info)
		{
			return info.OpenType == OperationType.source.ToString() || info.OpenType == OperationType.other.ToString();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007E13 File Offset: 0x00006013
		public override bool InitRunMode()
		{
			if (!base.InitRunMode())
			{
				base.RunMode = RunModeEnum.Open;
			}
			return true;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007E28 File Offset: 0x00006028
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

		// Token: 0x06000163 RID: 355 RVA: 0x00007EB0 File Offset: 0x000060B0
		protected override IProgressMonitor OnInstall()
		{
			return this.OnOpen();
		}
	}
}
