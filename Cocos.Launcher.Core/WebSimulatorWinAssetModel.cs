using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using Microsoft.Win32;
using Mono.Addins;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200001D RID: 29
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(4)]
	internal class WebSimulatorWinAssetModel : WinExeAssetModel
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00006880 File Offset: 0x00004A80
		public override int Order
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006883 File Offset: 0x00004A83
		public WebSimulatorWinAssetModel()
		{
			base.DownloadSucceed += this.DownloadSucceedHandler;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000689D File Offset: 0x00004A9D
		public WebSimulatorWinAssetModel(Plugin model) : base(model)
		{
			base.DownloadSucceed += this.DownloadSucceedHandler;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000068B8 File Offset: 0x00004AB8
		public override bool CanHandle(Plugin pluginModel)
		{
			return Platform.IsWindows && pluginModel.PluginName.Equals("Cocos Simulator for Web");
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000068D8 File Offset: 0x00004AD8
		protected override IProgressMonitor OnInstall()
		{
			string pluginPath = base.AssetInfo.PluginPath;
			string arg = Path.Combine(Option.CocosInstallDir, "CocosWebSimulator");
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (!File.Exists(pluginPath))
			{
				@default.ReportError(string.Format("The file {0} is not exist.", pluginPath), null);
				return @default;
			}
			string arguments = string.Format("/C start \"installer\" /wait \"{0}\" /NCRC /S /D={1}", pluginPath, arg);
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo("cmd.exe", arguments)
			{
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardError = true,
				RedirectStandardOutput = true
			};
			process.Start();
			process.WaitForExit();
			if (process.ExitCode == 0)
			{
				@default.ReportSuccess("Install CocosWebSimulator succeed.");
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

		// Token: 0x06000116 RID: 278 RVA: 0x00006A1C File Offset: 0x00004C1C
		protected override IProgressMonitor OnOpen()
		{
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			string text = string.Empty;
			try
			{
				object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\CocosSimulator.exe", null, null);
				if (value != null)
				{
					text = value.ToString();
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("获取模拟器安装目录时出错", exception);
				@default.ReportError("Failed to get installed path.", exception);
				text = string.Empty;
			}
			if (string.IsNullOrEmpty(text))
			{
				@default.ReportError("Can't find the exe file.", null);
				return @default;
			}
			try
			{
				Process.Start(text);
				@default.ReportSuccess("Open simulator succeed");
			}
			catch (Exception exception2)
			{
				LogConfig.Logger.Error("打开模拟器工具时出错", exception2);
				@default.ReportError("Failed to open simulator", exception2);
			}
			return @default;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006AE4 File Offset: 0x00004CE4
		private void DownloadSucceedHandler(object sender, DownloadSucceedEventArgs e)
		{
			this.Install(false);
		}
	}
}
