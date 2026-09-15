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
	internal class CocosPythonTool
	{
		public CocosPythonTool(CocosMonitor cocosMointor)
		{
			this.monitor = cocosMointor;
		}

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

		private string GetWorkingDirectory()
		{
			return Path.GetDirectoryName(Path.GetDirectoryName(Option.AssemblyDir));
		}

		private bool StartRunUsingConsole(string pythonPath, string arguments)
		{
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				return this.RunUsingNativeConsole(pythonPath, arguments);
			}
			return this.RunUsingGtkConsole(pythonPath, arguments);
		}

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

		private void OnOutputReceived(object sender, DataReceivedEventArgs e)
		{
			this.SendOutputInfo(e.Data);
		}

		private void OnErrorReceived(object sender, DataReceivedEventArgs e)
		{
			this.SendOutputInfo(e.Data);
		}

		private void SendOutputInfo(string info)
		{
			if (this.monitor != null && !string.IsNullOrEmpty(info))
			{
				this.monitor.SendInfo(info);
			}
		}

		private const string str_Third_Party = "Third_Party";

		private const string str_tools = "tools";

		private const string str_python = "python";

		private const string str_pythonexe = "python.exe";

		private const string str_cocos = "cocos";

		private const string str_cocospy = "cocos.py";

		private const string str_cocos2d_console = "cocos2d-console";

		private const string str_bin = "bin";

		private CocosMonitor monitor;
	}
}
