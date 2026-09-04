using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using GLib;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x0200001B RID: 27
	internal class CocosPythonTool
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x000051F3 File Offset: 0x000033F3
		public CocosPythonTool(CocosMonitor cocosMointor)
		{
			this.monitor = cocosMointor;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005204 File Offset: 0x00003404
		public bool RunPython(Cocos2dxInfo cocosInfo, string cmd, bool showConsole = false)
		{
			string pythonPath = this.GetPythonPath(cocosInfo.RootPath);
			if (string.IsNullOrEmpty(pythonPath))
			{
				return false;
			}
			string fullArguments = this.GetFullArguments(cocosInfo.RootPath, cmd);
			if (string.IsNullOrEmpty(fullArguments))
			{
				return false;
			}
			this.GetWorkingDirectory();
			bool result;
			if (showConsole)
			{
				result = this.StartRunUsingConsole(pythonPath, fullArguments);
			}
			else
			{
				result = this.StartRunUsingGtkWindow(pythonPath, fullArguments, cocosInfo.VersionText);
			}
			return result;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005268 File Offset: 0x00003468
		private string GetPythonPath(string cocosRootPath)
		{
			string text = string.Empty;
			if (MonoDevelop.Core.Platform.IsMac)
			{
				string path = Path.Combine("tools", "cocos2d-console", "bin", "cocos");
				text = Path.Combine(cocosRootPath, path);
			}
			else
			{
				string path2 = Path.Combine("Third_Party", "python", "python.exe");
				text = Path.Combine(Option.UserCustomerConfigFolder, path2);
			}
			if (!File.Exists(text))
			{
				this.SendOutputInfo("Python exec is not exist");
				return string.Empty;
			}
			return text;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000052E4 File Offset: 0x000034E4
		private string GetFullArguments(string cocosRootPath, string cmd)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				string text = Path.Combine(new string[]
				{
					cocosRootPath,
					"tools",
					"cocos2d-console",
					"bin",
					"cocos.py"
				});
				if (!File.Exists(text))
				{
					this.SendOutputInfo("cocos.py is not exist");
					return string.Empty;
				}
				stringBuilder.Append(string.Format("\"{0}\"", text));
			}
			stringBuilder.Append(cmd);
			return stringBuilder.ToString();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000536C File Offset: 0x0000356C
		private string GetWorkingDirectory()
		{
			return Path.GetDirectoryName(Path.GetDirectoryName(Option.AssemblyDir));
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000538A File Offset: 0x0000358A
		private bool StartRunUsingConsole(string pythonPath, string arguments)
		{
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				return this.RunUsingNativeConsole(pythonPath, arguments);
			}
			return this.RunUsingGtkConsole(pythonPath, arguments);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000053A4 File Offset: 0x000035A4
		private bool RunUsingNativeConsole(string pythonPath, string arguments)
		{
			if (!PlatformAdapter.PlatformService.CanOpenTerminal)
			{
				return false;
			}
			string workingDirectory = this.GetWorkingDirectory();
			IProcessAsyncOperation processAsyncOperation = PlatformAdapter.PlatformService.StartConsoleProcess(pythonPath, arguments, workingDirectory, null, "Cocos Console", true);
			processAsyncOperation.WaitForCompleted();
			return processAsyncOperation.ExitCode == 0;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000547C File Offset: 0x0000367C
		private bool RunUsingGtkConsole(string pythonPath, string arguments)
		{
			bool result;
			try
			{
				CocosMonitor monitor = new CocosMonitor(true);
				GLib.Timeout.Add(0U, delegate
				{
					System.Diagnostics.Process proc = this.CreateProcess(pythonPath, arguments);
					ConsoleOutputWindow consoleOutputWindow = new ConsoleOutputWindow();
					if (Services.ProjectsService.CurrentSolution == null)
					{
						consoleOutputWindow.Title = "Cocos Console";
					}
					else
					{
						consoleOutputWindow.Title = string.Format("Cocos Console - {0}", Services.ProjectsService.CurrentSolution.Name);
					}
					consoleOutputWindow.StartRunning(monitor, proc);
					return false;
				});
				while (!monitor.HasStarted || monitor.IsProcessing)
				{
					System.Threading.Thread.Sleep(100);
				}
				result = true;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("执行Python脚本时出错", exception);
				result = false;
			}
			return result;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005520 File Offset: 0x00003720
		private bool StartRunUsingGtkWindow(string pythonPath, string arguments, string frameworkVersion)
		{
			bool result = true;
			using (System.Diagnostics.Process process = this.CreateProcess(pythonPath, arguments))
			{
				try
				{
					process.OutputDataReceived += this.OnOutputReceived;
					process.ErrorDataReceived += this.OnErrorReceived;
					this.SendOutputInfo(string.Format("Based on: {0}", frameworkVersion));
					process.Start();
					process.BeginOutputReadLine();
					process.BeginErrorReadLine();
					while (!process.HasExited)
					{
						if (this.monitor.IsCancelled && process != null)
						{
							process.Kill();
							return false;
						}
						System.Threading.Thread.Sleep(100);
					}
					if (process.ExitCode != 0)
					{
						result = false;
					}
				}
				catch (Exception exception)
				{
					this.SendOutputInfo("Failed to run the python script");
					LogConfig.Output.Error("执行Python脚本时出错", exception);
					result = false;
				}
				finally
				{
					if (process != null)
					{
						process.OutputDataReceived -= this.OnOutputReceived;
						process.ErrorDataReceived -= this.OnErrorReceived;
					}
				}
			}
			return result;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005638 File Offset: 0x00003838
		private System.Diagnostics.Process CreateProcess(string pythonPath, string arguments)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.UseShellExecute = false;
			processStartInfo.Arguments = arguments;
			processStartInfo.FileName = pythonPath;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardInput = true;
			processStartInfo.WorkingDirectory = this.GetWorkingDirectory();
			return new System.Diagnostics.Process
			{
				EnableRaisingEvents = true,
				StartInfo = processStartInfo
			};
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000569D File Offset: 0x0000389D
		private void OnOutputReceived(object sender, DataReceivedEventArgs e)
		{
			this.SendOutputInfo(e.Data);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000056AB File Offset: 0x000038AB
		private void OnErrorReceived(object sender, DataReceivedEventArgs e)
		{
			this.SendOutputInfo(e.Data);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000056B9 File Offset: 0x000038B9
		private void SendOutputInfo(string info)
		{
			if (this.monitor != null && !string.IsNullOrEmpty(info))
			{
				this.monitor.SendInfo(info);
			}
		}

		// Token: 0x04000039 RID: 57
		private const string str_Third_Party = "Third_Party";

		// Token: 0x0400003A RID: 58
		private const string str_tools = "tools";

		// Token: 0x0400003B RID: 59
		private const string str_python = "python";

		// Token: 0x0400003C RID: 60
		private const string str_pythonexe = "python.exe";

		// Token: 0x0400003D RID: 61
		private const string str_cocos = "cocos";

		// Token: 0x0400003E RID: 62
		private const string str_cocospy = "cocos.py";

		// Token: 0x0400003F RID: 63
		private const string str_cocos2d_console = "cocos2d-console";

		// Token: 0x04000040 RID: 64
		private const string str_bin = "bin";

		// Token: 0x04000041 RID: 65
		private CocosMonitor monitor;
	}
}
