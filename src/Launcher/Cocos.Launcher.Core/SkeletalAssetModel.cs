using System;
using System.Diagnostics;
using System.IO;
using Cocos.Launcher.Library;
using CocoStudio.Core;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(3)]
	public class SkeletalAssetModel : WinExeAssetModel
	{
		public override int Order
		{
			get
			{
				return 3;
			}
		}

		public SkeletalAssetModel()
		{
		}

		public SkeletalAssetModel(Plugin model) : base(model)
		{
		}

		public override bool CanHandle(Plugin pluginModel)
		{
			return pluginModel.UninstallName == this.cocosAnimationEditorDisName;
		}

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

		private string cocosAnimationEditorDisName = "Cocos Skeletal Animation Editor";
	}
}
