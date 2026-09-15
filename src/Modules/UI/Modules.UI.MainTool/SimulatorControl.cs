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
	public class SimulatorControl : IPlayControl
	{
		public static IPlayControl Instance { get; private set; } = new SimulatorControl();

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

		public bool CanStop
		{
			get
			{
				return this.currentProcess != null;
			}
		}

		public event EventHandler<StateChangedEventArgs> StateChanged;

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

		public void Stop()
		{
			this.ReleaseCurrentProcess();
			this.RaiseStateChangedEvent(false);
		}

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

		private void ErrorDataReceivedHandler(object sender, DataReceivedEventArgs e)
		{
			LogConfig.OutputWithoutTip.Error(e.Data);
		}

		private const string defaultPathWin = "cocos-simulator-bin\\win32\\Simulator.exe";

		private const string defaultPathMac = "cocos-simulator-bin/mac/Simulator.app/Contents/MacOS/Simulator";

		private string simulatorPath;

		private System.Diagnostics.Process currentProcess = null;
	}
}
