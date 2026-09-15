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
	[Extension(typeof(BaseAssetModel))]
	[AssetOrder(4)]
	internal class WebSimulatorWinAssetModel : WinExeAssetModel
	{
		public override int Order
		{
			get
			{
				return 4;
			}
		}

		public WebSimulatorWinAssetModel()
		{
			base.DownloadSucceed += this.DownloadSucceedHandler;
		}

		public WebSimulatorWinAssetModel(Plugin model) : base(model)
		{
			base.DownloadSucceed += this.DownloadSucceedHandler;
		}

		public override bool CanHandle(Plugin pluginModel)
		{
			return Platform.IsWindows && pluginModel.PluginName.Equals("Cocos Simulator for Web");
		}

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

		private void DownloadSucceedHandler(object sender, DownloadSucceedEventArgs e)
		{
			this.Install(false);
		}
	}
}
