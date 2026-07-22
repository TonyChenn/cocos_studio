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
	// Token: 0x02000003 RID: 3
	public abstract class CocosCodeIDEService
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002811 File Offset: 0x00000A11
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

		// Token: 0x06000009 RID: 9 RVA: 0x00002843 File Offset: 0x00000A43
		public bool IsCocosCodeIDEInstalled()
		{
			return !string.IsNullOrEmpty(this.GetCocosCodeIDEExePath());
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002858 File Offset: 0x00000A58
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

		// Token: 0x0600000B RID: 11 RVA: 0x000028B0 File Offset: 0x00000AB0
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

		// Token: 0x0600000C RID: 12 RVA: 0x0000296C File Offset: 0x00000B6C
		public string GetExePath()
		{
			return this.GetCocosCodeIDEExePath();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002974 File Offset: 0x00000B74
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

		// Token: 0x0600000E RID: 14 RVA: 0x000029FC File Offset: 0x00000BFC
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

		// Token: 0x0600000F RID: 15 RVA: 0x00002A4C File Offset: 0x00000C4C
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

		// Token: 0x06000010 RID: 16
		protected abstract void OpenIDEWithCmd(FilePath projectDir);

		// Token: 0x06000011 RID: 17
		protected abstract string GetCocosCodeIDEDirectoryPath();

		// Token: 0x06000012 RID: 18
		protected abstract string GetCocosCodeIDEExePath();

		// Token: 0x0400000D RID: 13
		private const string Uri_CocosCodeIDE = "http://www.cocos2d-x.org/products/codeide";

		// Token: 0x0400000E RID: 14
		private static CocosCodeIDEService _IDEService;
	}
}
