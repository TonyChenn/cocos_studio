using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	[AssetOrder(4)]
	[Extension(typeof(BaseAssetModel))]
	internal class WebSimulatorMacAssetModel : MacPackageAssetModel
	{
		public override int Order
		{
			get
			{
				return 4;
			}
		}

		public WebSimulatorMacAssetModel()
		{
			base.DownloadSucceed += this.DownloadSucceedHandler;
		}

		public WebSimulatorMacAssetModel(Plugin model) : base(model)
		{
			base.DownloadSucceed += this.DownloadSucceedHandler;
		}

		public override bool CanHandle(Plugin pluginModel)
		{
			return Platform.IsMac && pluginModel.PluginName.Equals("Cocos Simulator for Web");
		}

		protected override IProgressMonitor OnInstall()
		{
			string pluginPath = base.AssetInfo.PluginPath;
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (!File.Exists(pluginPath))
			{
				@default.ReportError(string.Format("The file {0} is not exist.", pluginPath), null);
				return @default;
			}
			string arguments = string.Format("installer -pkg \"{0}\" -target /", pluginPath);
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo
			{
				FileName = this.GetSudoToolPath(),
				Arguments = arguments,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};
			process.Start();
			process.WaitForExit();
			if (process.ExitCode == 0)
			{
				@default.ReportSuccess("Install succeed.");
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("Failed to install Cocos Web Simulator. Exit Code: {0}", process.ExitCode));
				stringBuilder.Append(string.Format("Output: {0}", process.StandardOutput.ReadToEnd()));
				stringBuilder.Append(string.Format("Error: {0}", process.StandardError.ReadToEnd()));
				LogConfig.Logger.Error(stringBuilder.ToString());
				@default.ReportError(stringBuilder.ToString(), null);
			}
			return @default;
		}

		private string GetSudoToolPath()
		{
			return Path.Combine(Option.AssemblyDir, "CocosStudioUpdate");
		}

		private void DownloadSucceedHandler(object sender, DownloadSucceedEventArgs e)
		{
			this.Install(false);
		}
	}
}
