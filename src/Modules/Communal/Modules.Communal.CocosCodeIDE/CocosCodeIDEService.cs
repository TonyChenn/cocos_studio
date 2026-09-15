using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using Newtonsoft.Json;

namespace Modules.Communal.CocosCodeIDE
{
	public abstract class CocosCodeIDEService
	{
		public static CocosCodeIDEService Instance
		{
			get
			{
				if (CocosCodeIDEService._IDEService == null)
				{
					if (Platform.IsMac)
					{
						CocosCodeIDEService._IDEService = new CocosCodeIDEOnMac();
					}
					else if (Platform.IsWindows)
					{
						CocosCodeIDEService._IDEService = new CocosCodeIDEOnWindows();
					}
				}
				return CocosCodeIDEService._IDEService;
			}
		}

		public bool IsCocosCodeIDEInstalled()
		{
			return !string.IsNullOrEmpty(this.GetCocosCodeIDEExePath());
		}

		public void DownloadCocosCodeIDE()
		{
			try
			{
				using (Process.Start("http://www.cocos2d-x.org/products/codeide"))
				{
				}
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error(LanguageInfo.MessageBox182_FailedToOpenWeb, exception);
			}
		}

		public string OpenCocosItemWithCocosCodeIDE(string projectDir, bool showFailedDialog = false)
		{
			string result = string.Empty;
			string cocosCodeIDEDirectoryPath = this.GetCocosCodeIDEDirectoryPath();
			if (cocosCodeIDEDirectoryPath != null)
			{
				try
				{
					ConfigJson configFile = this.GetConfigFile(cocosCodeIDEDirectoryPath);
					if (configFile == null)
					{
						this.OpenIDEWithCmd(projectDir);
						return result;
					}
					int codeIDEPort = this.GetCodeIDEPort(projectDir, configFile);
					if (codeIDEPort == -1)
					{
						this.OpenIDEWithCmd(projectDir);
					}
					else
					{
						this.OpenIDEWithSocket(projectDir, configFile.host, codeIDEPort);
					}
				}
				catch (Exception exception)
				{
					result = LanguageInfo.MessageBox180_FailedToOpenCodeIDE;
					LogConfig.Output.Error(LanguageInfo.MessageBox180_FailedToOpenCodeIDE, exception);
				}
				return result;
			}
			if (showFailedDialog)
			{
				CocosCodeIDEDialog cocosCodeIDEDialog = new CocosCodeIDEDialog();
				cocosCodeIDEDialog.Run();
				cocosCodeIDEDialog.Destroy();
			}
			result = LanguageInfo.MessageBox179_CantFindCodeIDE;
			return result;
		}

		public string GetExePath()
		{
			return this.GetCocosCodeIDEExePath();
		}

		private int GetCodeIDEPort(string projectDir, ConfigJson config)
		{
			int result = -1;
			foreach (ProcessInfo processInfo in config.processes)
			{
				if (processInfo.projectDirs.Contains(projectDir))
				{
					try
					{
						Process processById = Process.GetProcessById(processInfo.processID);
						if (processById != null)
						{
							result = processInfo.port;
							break;
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return result;
		}

		private ConfigJson GetConfigFile(string cocosCodeIDEDirectory)
		{
			string path = Path.Combine(cocosCodeIDEDirectory, "state.ini");
			ConfigJson result = null;
			try
			{
				string value = File.ReadAllText(path);
				result = JsonConvert.DeserializeObject<ConfigJson>(value);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Read Code Ide config json failed.", exception);
			}
			return result;
		}

		private void OpenIDEWithSocket(FilePath projectDir, string host, int port)
		{
			Socket socket = null;
			try
			{
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.Connect(host, port);
			}
			catch (Exception)
			{
				this.OpenIDEWithCmd(projectDir);
				socket = null;
			}
			if (socket == null)
			{
				return;
			}
			try
			{
				SendMessageInfo value = new SendMessageInfo("openProject", new string[]
				{
					projectDir
				});
				string str = JsonConvert.SerializeObject(value);
				byte[] bytes = Encoding.UTF8.GetBytes(str + "\r");
				socket.Send(bytes);
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error(LanguageInfo.MessageBox180_FailedToOpenCodeIDE, exception);
			}
		}

		protected abstract void OpenIDEWithCmd(FilePath projectDir);

		protected abstract string GetCocosCodeIDEDirectoryPath();

		protected abstract string GetCocosCodeIDEExePath();

		private const string Uri_CocosCodeIDE = "http://www.cocos2d-x.org/products/codeide";

		private static CocosCodeIDEService _IDEService;
	}
}
