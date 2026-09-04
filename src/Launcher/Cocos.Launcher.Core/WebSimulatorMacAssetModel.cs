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
	// Token: 0x0200001B RID: 27
	[AssetOrder(4)]
	[Extension(typeof(BaseAssetModel))]
	internal class WebSimulatorMacAssetModel : MacPackageAssetModel
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000062E5 File Offset: 0x000044E5
		public override int Order
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000062E8 File Offset: 0x000044E8
		public WebSimulatorMacAssetModel()
		{
			base.DownloadSucceed += this.DownloadSucceedHandler;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006302 File Offset: 0x00004502
		public WebSimulatorMacAssetModel(Plugin model) : base(model)
		{
			base.DownloadSucceed += this.DownloadSucceedHandler;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000631D File Offset: 0x0000451D
		public override bool CanHandle(Plugin pluginModel)
		{
			return Platform.IsMac && pluginModel.PluginName.Equals("Cocos Simulator for Web");
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000633C File Offset: 0x0000453C
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

		// Token: 0x06000102 RID: 258 RVA: 0x0000646C File Offset: 0x0000466C
		private string GetSudoToolPath()
		{
			return Path.Combine(Option.AssemblyDir, "CocosStudioUpdate");
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000648A File Offset: 0x0000468A
		private void DownloadSucceedHandler(object sender, DownloadSucceedEventArgs e)
		{
			this.Install(false);
		}
	}
}
