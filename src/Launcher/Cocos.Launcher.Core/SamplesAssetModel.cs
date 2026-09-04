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
	// Token: 0x0200001E RID: 30
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(2)]
	internal class SamplesAssetModel : BaseAssetModel
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00006AED File Offset: 0x00004CED
		public override int Order
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006AF0 File Offset: 0x00004CF0
		public SamplesAssetModel()
		{
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006AF8 File Offset: 0x00004CF8
		public SamplesAssetModel(Plugin model) : base(model)
		{
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006B01 File Offset: 0x00004D01
		public override bool CanHandle(Plugin info)
		{
			return info.OpenType == OperationType.cocos.ToString();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006B1E File Offset: 0x00004D1E
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

		// Token: 0x0600011D RID: 285 RVA: 0x00006B48 File Offset: 0x00004D48
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

		// Token: 0x0600011E RID: 286 RVA: 0x00006BC4 File Offset: 0x00004DC4
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

		// Token: 0x0600011F RID: 287 RVA: 0x00006C60 File Offset: 0x00004E60
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

		// Token: 0x06000120 RID: 288 RVA: 0x00006C8C File Offset: 0x00004E8C
		private string GetModelToUnZipDir()
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(base.AssetInfo.PluginFullName);
			return Path.Combine(ConstantConfig.Paths.DownloadDemoPath, fileNameWithoutExtension);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006CBC File Offset: 0x00004EBC
		protected override void OnDelete()
		{
			if (Directory.Exists(base.AssetInfo.UnZipPath))
			{
				Directory.Delete(base.AssetInfo.UnZipPath, true);
			}
		}
	}
}
