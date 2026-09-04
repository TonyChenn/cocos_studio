using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Projects;
using CocoStudio.Projects.Visiter;
using GLib;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Output;
using Xwt.GtkBackend;

namespace Modules.UI.MainTool
{
	// Token: 0x02000011 RID: 17
	public class SimulatorControl : IPlayControl
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003A54 File Offset: 0x00001C54
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003A6A File Offset: 0x00001C6A
		public static IPlayControl Instance { get; private set; } = new SimulatorControl();

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003A80 File Offset: 0x00001C80
		public bool CanPlay
		{
			get
			{
				CocosItem currentSelectedProject = Services.ProjectOperations.CurrentSelectedProject;
				bool result;
				if (currentSelectedProject == null)
				{
					result = false;
				}
				else
				{
					CocosFile cocosFile = currentSelectedProject.CocosFile;
					if (cocosFile == null)
					{
						result = false;
					}
					else
					{
						string type = cocosFile.Type;
						result = (!type.Equals(NodeType.Layer.ToString()) && !type.Equals(NodeType.Plist.ToString()));
					}
				}
				return result;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003B04 File Offset: 0x00001D04
		public bool CanStop
		{
			get
			{
				return this.currentProcess != null;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600005F RID: 95 RVA: 0x00003B2C File Offset: 0x00001D2C
		// (remove) Token: 0x06000060 RID: 96 RVA: 0x00003B68 File Offset: 0x00001D68
		public event EventHandler<StateChangedEventArgs> StateChanged;

		// Token: 0x06000061 RID: 97 RVA: 0x00003BA4 File Offset: 0x00001DA4
		private SimulatorControl()
		{
			if (Platform.IsMac)
			{
				this.simulatorPath = Path.Combine(Option.CocosInstallDir, "cocos-simulator-bin/mac/Simulator.app/Contents/MacOS/Simulator");
			}
			else
			{
				this.simulatorPath = Path.Combine(Option.CocosInstallDir, "cocos-simulator-bin\\win32\\Simulator.exe");
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003C00 File Offset: 0x00001E00
		public bool Play()
		{
			Services.Workbench.ActiveDocument.IsDirty = true;
			Services.Workbench.ActiveDocument.Save();
			this.ReleaseCurrentProcess();
			try
			{
				this.currentProcess = this.CreateSimulatorProcess(Cocos2dxReaderType.cocos2dx_2XCSD);
				this.currentProcess.Start();
				this.currentProcess.BeginOutputReadLine();
				this.currentProcess.BeginErrorReadLine();
				this.RaiseStateChangedEvent(true);
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error(LanguageInfo.StartLauchFail, exception);
				this.ReleaseCurrentProcess();
				return false;
			}
			return true;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003CA8 File Offset: 0x00001EA8
		public void Stop()
		{
			this.ReleaseCurrentProcess();
			this.RaiseStateChangedEvent(false);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003CBC File Offset: 0x00001EBC
		private void ReleaseCurrentProcess()
		{
			if (this.currentProcess != null)
			{
				try
				{
					if (!this.currentProcess.HasExited)
					{
						this.currentProcess.Kill();
					}
					this.currentProcess.Exited -= this.SimulatorExitedHandler;
					this.currentProcess.OutputDataReceived -= this.OutputDataReceivedHandler;
					this.currentProcess.ErrorDataReceived -= this.ErrorDataReceivedHandler;
					this.currentProcess.Dispose();
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("停止进程失败", exception);
				}
				finally
				{
					this.currentProcess = null;
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003D94 File Offset: 0x00001F94
		private void RaiseStateChangedEvent(bool isPlay)
		{
			Timeout.Add(500U, delegate
			{
				if (this.StateChanged != null)
				{
					string state = "Stop";
					if (isPlay)
					{
						state = "Play";
					}
					StateChangedEventArgs e = new StateChangedEventArgs(state);
					this.StateChanged(this, e);
				}
				return false;
			});
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003DD0 File Offset: 0x00001FD0
		private System.Diagnostics.Process CreateSimulatorProcess(Cocos2dxReaderType type)
		{
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.EnableRaisingEvents = true;
			process.Exited += this.SimulatorExitedHandler;
			process.OutputDataReceived += this.OutputDataReceivedHandler;
			process.ErrorDataReceived += this.ErrorDataReceivedHandler;
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.UseShellExecute = false;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.RedirectStandardOutput = true;
			if (type == Cocos2dxReaderType.cocos2dx_2XCSD)
			{
				processStartInfo.Arguments = this.GetStartParam();
				processStartInfo.FileName = this.simulatorPath;
			}
			process.StartInfo = processStartInfo;
			return process;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003E78 File Offset: 0x00002078
		private string GetStartParam()
		{
			string result;
			if (Services.ProjectOperations.CurrentSelectedProject == null)
			{
				result = string.Empty;
			}
			else
			{
				string path = Services.ProjectOperations.CurrentSelectedSolution.BaseDirectory;
				string arg = Path.Combine(path, "cocosstudio");
				int num = 480;
				int num2 = 320;
				SizeF sceneSize = Services.ProjectOperations.CurrentSelectedSolution.GetSceneSize();
				if (sceneSize != null)
				{
					num = (int)sceneSize.Width;
					num2 = (int)sceneSize.Height;
				}
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				Services.MainWindow.GdkWindow.GetPosition(out num5, out num6);
				Services.MainWindow.GdkWindow.GetSize(out num3, out num4);
				string arg2 = "disable";
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("-entry \"{0}\"", Services.ProjectOperations.CurrentSelectedProject.FullPath));
				stringBuilder.Append(string.Format(" -workdir \"{0}\"", arg));
				stringBuilder.Append(string.Format(" -resolution {0}x{1}", num, num2));
				stringBuilder.Append(string.Format(" -position {0},{1}", num5 + num3 / 2 - num / 2, num6 + num4 / 2 - num2 / 2));
				stringBuilder.Append(string.Format(" -search-path \"{0}\"", Option.EditorDefaultResourcePath));
				stringBuilder.Append(string.Format(" -scale {0}", (float)Option.UserConfig.SimulatorInitScale / 100f));
				stringBuilder.Append(string.Format(" -console {0}", arg2));
				string text = stringBuilder.ToString();
				result = text;
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004030 File Offset: 0x00002230
		private void SimulatorExitedHandler(object sender, EventArgs e)
		{
			System.Diagnostics.Process process = sender as System.Diagnostics.Process;
			if (process.ExitCode != 0)
			{
				Services.Workbench.Pads.OutputPad.BringToFront();
			}
			this.ReleaseCurrentProcess();
			this.RaiseStateChangedEvent(false);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004078 File Offset: 0x00002278
		private void OutputDataReceivedHandler(object sender, DataReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				if (e.Data.Contains("iShow!"))
				{
					Services.GetService<IOutputPad>().ScrollToEnd();
				}
				else
				{
					LogConfig.OutputWithoutTip.Info(e.Data, false);
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000040D2 File Offset: 0x000022D2
		private void ErrorDataReceivedHandler(object sender, DataReceivedEventArgs e)
		{
			LogConfig.OutputWithoutTip.Error(e.Data);
		}

		// Token: 0x04000028 RID: 40
		private const string defaultPathWin = "cocos-simulator-bin\\win32\\Simulator.exe";

		// Token: 0x04000029 RID: 41
		private const string defaultPathMac = "cocos-simulator-bin/mac/Simulator.app/Contents/MacOS/Simulator";

		// Token: 0x0400002A RID: 42
		private string simulatorPath;

		// Token: 0x0400002B RID: 43
		private System.Diagnostics.Process currentProcess = null;
	}
}
