using System;
using System.Diagnostics;
using System.IO;
using Cocos.Launcher.Library;
using CocoStudio.Core;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000025 RID: 37
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(3)]
	public class SkeletalAssetModel : WinExeAssetModel
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00007C74 File Offset: 0x00005E74
		public override int Order
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007C77 File Offset: 0x00005E77
		public SkeletalAssetModel()
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007C8A File Offset: 0x00005E8A
		public SkeletalAssetModel(Plugin model) : base(model)
		{
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007C9E File Offset: 0x00005E9E
		public override bool CanHandle(Plugin pluginModel)
		{
			return pluginModel.UninstallName == this.cocosAnimationEditorDisName;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007CB8 File Offset: 0x00005EB8
		protected override IProgressMonitor OnUninstall()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (base.AssetInfo.IsUninstall)
			{
				if (!RegistryServices.IsKeysContainsDisplayName(base.AssetInfo.UninstallName))
				{
					return @default;
				}
				string setupFactoryRegistryKeyCode = RegistryServices.GetSetupFactoryRegistryKeyCode(base.AssetInfo.UninstallName);
				if (!string.IsNullOrEmpty(setupFactoryRegistryKeyCode))
				{
					Process process = new Process();
					process.StartInfo.FileName = setupFactoryRegistryKeyCode;
					process.Start();
					process.WaitForExit();
				}
				if (RegistryServices.IsKeysContainsDisplayName(base.AssetInfo.UninstallName))
				{
					@default.ReportError(null, null);
				}
			}
			else
			{
				@default.ReportError("此插件不支持卸载: " + base.AssetInfo.UninstallName, null);
			}
			return @default;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007D64 File Offset: 0x00005F64
		protected override IProgressMonitor OnOpen()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			string text = Path.Combine(RegistryServices.GetSetupFactoryInstallLocation(base.AssetInfo.UninstallName), base.AssetInfo.ProcedureExeName);
			if (!File.Exists(text))
			{
				@default.ReportError(null, null);
				return @default;
			}
			Process.Start(new ProcessStartInfo(text)
			{
				WorkingDirectory = Path.GetDirectoryName(text)
			});
			return @default;
		}

		// Token: 0x0400006D RID: 109
		private string cocosAnimationEditorDisName = "Cocos Skeletal Animation Editor";
	}
}
