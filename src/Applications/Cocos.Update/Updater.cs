using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Gtk;

namespace Cocos.Update
{
	public class Updater
	{
		public Updater(string studioInstallArgs, string macRuntimeArgs)
		{
			if (studioInstallArgs.Equals("#SKIP#"))
			{
				this.StudioInstallFilePath = "";
			}
			else
			{
				this.StudioInstallFilePath = studioInstallArgs;
			}
			if (macRuntimeArgs.Equals("#SKIP#"))
			{
				this.RuntimeInstallFilePath = "";
				return;
			}
			this.RuntimeInstallFilePath = macRuntimeArgs;
		}

		public void StartUpdate()
		{
			try
			{
				if (PlatformHelper.IsMacPlatform)
				{
					this.RunUpdateOnMac();
					this.RestartEidtorOnMac();
				}
				else
				{
					DebugHelper.WriteLogInfo("开始Windows平台的自动更新");
					BackgroundWorker backgroundWorker = new BackgroundWorker();
					backgroundWorker.DoWork += this.RunUpdateOnWindows;
					backgroundWorker.RunWorkerCompleted += this.HandleWorkerCompleted;
					backgroundWorker.RunWorkerAsync();
				}
			}
			catch (Exception ex)
			{
				DebugHelper.WriteLogInfo(ex.ToString());
			}
		}

		private void RunUpdateOnMac()
		{
			if (!string.IsNullOrEmpty(this.RuntimeInstallFilePath) && File.Exists(this.RuntimeInstallFilePath))
			{
				DebugHelper.WriteLogInfo("开始Mac平台运行环境的更新");
				Process process = new Process();
				process.StartInfo = new ProcessStartInfo("open", "\"" + this.RuntimeInstallFilePath + "\" -W");
				process.Start();
				process.WaitForExit();
			}
			if (!string.IsNullOrEmpty(this.StudioInstallFilePath) && File.Exists(this.StudioInstallFilePath))
			{
				DebugHelper.WriteLogInfo("开始Mac平台编辑器的更新");
				Process process2 = new Process();
				process2.StartInfo = new ProcessStartInfo("open", "\"" + this.StudioInstallFilePath + "\" -W");
				process2.Start();
				process2.WaitForExit();
			}
		}

		private void RestartEidtorOnMac()
		{
			DebugHelper.WriteLogInfo("Mac下安装完毕，尝试重启Cocos Studio");
			if (File.Exists("/Applications/cocos/Cocos.app/Contents/MacOS/Cocos"))
			{
				new Process
				{
					StartInfo = new ProcessStartInfo("/Applications/cocos/Cocos.app/Contents/MacOS/Cocos")
				}.Start();
			}
			Application.Quit();
		}

		private void RunUpdateOnWindows(object sender, DoWorkEventArgs e)
		{
			if (!string.IsNullOrEmpty(this.StudioInstallFilePath))
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo(this.StudioInstallFilePath);
				processStartInfo.WorkingDirectory = Path.GetDirectoryName(this.StudioInstallFilePath);
				if (File.Exists(this.StudioInstallFilePath))
				{
					DebugHelper.WriteLogInfo("开始Windows平台上编辑器的安装");
					Process.Start(processStartInfo);
				}
			}
			this.QuitCurrentApplication();
		}

		private void HandleWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.QuitCurrentApplication();
		}

		public void QuitCurrentApplication()
		{
			ProcHelper.KillProc("Editor.AutoUpdate");
			ProcHelper.KillProc("Editor.AutoUpdate.vshost");
			string name = typeof(Updater).Assembly.GetName().Name;
			ProcHelper.KillProc(name);
		}

		private const string ToolName = "CocosStudioUpdate";

		private const string macAppPath = "/Applications/cocos/Cocos.app/Contents/MacOS/Cocos";

		private const string skipTag = "#SKIP#";

		private string StudioInstallFilePath;

		private string RuntimeInstallFilePath;
	}
}
