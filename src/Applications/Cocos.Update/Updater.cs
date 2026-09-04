using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Gtk;

namespace Cocos.Update
{
	// Token: 0x02000006 RID: 6
	public class Updater
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000022C8 File Offset: 0x000004C8
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

		// Token: 0x0600000F RID: 15 RVA: 0x0000231C File Offset: 0x0000051C
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

		// Token: 0x06000010 RID: 16 RVA: 0x00002398 File Offset: 0x00000598
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

		// Token: 0x06000011 RID: 17 RVA: 0x00002460 File Offset: 0x00000660
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

		// Token: 0x06000012 RID: 18 RVA: 0x000024A8 File Offset: 0x000006A8
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

		// Token: 0x06000013 RID: 19 RVA: 0x00002503 File Offset: 0x00000703
		private void HandleWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.QuitCurrentApplication();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000250C File Offset: 0x0000070C
		public void QuitCurrentApplication()
		{
			ProcHelper.KillProc("Editor.AutoUpdate");
			ProcHelper.KillProc("Editor.AutoUpdate.vshost");
			string name = typeof(Updater).Assembly.GetName().Name;
			ProcHelper.KillProc(name);
		}

		// Token: 0x04000006 RID: 6
		private const string ToolName = "CocosStudioUpdate";

		// Token: 0x04000007 RID: 7
		private const string macAppPath = "/Applications/cocos/Cocos.app/Contents/MacOS/Cocos";

		// Token: 0x04000008 RID: 8
		private const string skipTag = "#SKIP#";

		// Token: 0x04000009 RID: 9
		private string StudioInstallFilePath;

		// Token: 0x0400000A RID: 10
		private string RuntimeInstallFilePath;
	}
}
