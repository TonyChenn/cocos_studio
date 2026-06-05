using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x02000007 RID: 7
	internal class DownloadProcesser
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002838 File Offset: 0x00000A38
		public void DownloadAsync(DownloadMonitor monitor)
		{
			if (monitor == null)
			{
				throw new Exception("The monitor is null");
			}
			Task task;
			if (Platform.IsMac)
			{
				task = new Task(delegate()
				{
					this.StartMacDownload(monitor);
				});
			}
			else
			{
				task = new Task(delegate()
				{
					this.StartWinDownload(monitor);
				});
			}
			task.Start();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000028A0 File Offset: 0x00000AA0
		private void StartWinDownload(DownloadMonitor monitor)
		{
			monitor.Start();
			try
			{
				string text;
				string text2;
				string serverUrl;
				if (monitor.NeedUpdateRuntime)
				{
					text = monitor.StudioInstallFilePath;
					text2 = monitor.ServerInfo.PackageSize;
					serverUrl = monitor.ServerInfo.DownloadUrl;
				}
				else
				{
					text = monitor.WinSmallPackageFilePath;
					text2 = ((WinServerInfo)monitor.ServerInfo).SmallPackageSize;
					serverUrl = ((WinServerInfo)monitor.ServerInfo).SmallPackageLink;
				}
				if (this.CheckFileIsDownloaded(text, text2))
				{
					monitor.Finish(true, "The install package is already downloaded");
				}
				else
				{
					List<string> retainFiles = new List<string>
					{
						text
					};
					this.InitDirectory(PathHelper.AutoUpdateTempPath, retainFiles);
					if (this.DownloadFile(monitor, text, serverUrl, 0L, long.Parse(text2)))
					{
						monitor.Finish(true, "");
					}
					else
					{
						monitor.Finish(false, "download cancelled");
					}
				}
			}
			catch (Exception ex)
			{
				monitor.Finish(false, ex.Message);
				LogConfig.Logger.Error(LanguageInfo.Output_FailedToDownload, ex);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000029A0 File Offset: 0x00000BA0
		private void StartMacDownload(DownloadMonitor monitor)
		{
			monitor.Start();
			bool flag = monitor.NeedUpdateStudio;
			bool flag2 = monitor.NeedUpdateRuntime;
			if (monitor.NeedUpdateStudio)
			{
				string studioInstallFilePath = monitor.StudioInstallFilePath;
				string packageSize = monitor.ServerInfo.PackageSize;
				flag = !this.CheckFileIsDownloaded(studioInstallFilePath, packageSize);
			}
			if (monitor.NeedUpdateRuntime)
			{
				string macRuntimeInstallFilePath = monitor.MacRuntimeInstallFilePath;
				string runtimeSize = ((MacServerInfo)monitor.ServerInfo).RuntimeSize;
				flag2 = !this.CheckFileIsDownloaded(macRuntimeInstallFilePath, runtimeSize);
			}
			if (!flag && !flag2)
			{
				monitor.Finish(true, "The install package is already downloaded");
				return;
			}
			List<string> retainFiles = new List<string>
			{
				monitor.StudioInstallFilePath,
				monitor.MacRuntimeInstallFilePath
			};
			this.InitDirectory(PathHelper.AutoUpdateTempPath, retainFiles);
			try
			{
				if (this.DownloadMacPackages(monitor, flag, flag2))
				{
					monitor.Finish(true, "");
				}
				else
				{
					monitor.Finish(false, "download cancelled");
				}
			}
			catch (Exception ex)
			{
				monitor.Finish(false, ex.Message);
				LogConfig.Logger.Error(ex.ToString());
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002AB8 File Offset: 0x00000CB8
		private bool DownloadMacPackages(DownloadMonitor monitor, bool downloadStudio, bool downloadRuntime)
		{
			long num = 0L;
			long num2 = 0L;
			string localPath = null;
			string serverUrl = null;
			string localPath2 = null;
			string serverUrl2 = null;
			if (downloadStudio)
			{
				localPath = monitor.StudioInstallFilePath;
				serverUrl = monitor.ServerInfo.DownloadUrl;
				num = long.Parse(monitor.ServerInfo.PackageSize);
			}
			if (downloadRuntime)
			{
				localPath2 = monitor.MacRuntimeInstallFilePath;
				serverUrl2 = ((MacServerInfo)monitor.ServerInfo).RuntimeDownloadLink;
				num2 = long.Parse(((MacServerInfo)monitor.ServerInfo).RuntimeSize);
			}
			long sumLength = num + num2;
			bool flag = false;
			if (downloadStudio)
			{
				flag = this.DownloadFile(monitor, localPath, serverUrl, 0L, sumLength);
				if (!flag)
				{
					return false;
				}
			}
			if (downloadRuntime)
			{
				flag = this.DownloadFile(monitor, localPath2, serverUrl2, num, sumLength);
				if (!flag)
				{
					return false;
				}
			}
			return flag;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002B6C File Offset: 0x00000D6C
		private bool CheckFileIsDownloaded(string filePath, string fileSize)
		{
			if (File.Exists(filePath))
			{
				FileInfo fileInfo = new FileInfo(filePath);
				string text = fileInfo.Length.ToString();
				return text.Equals(fileSize);
			}
			return false;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002BA8 File Offset: 0x00000DA8
		private bool DownloadFile(DownloadMonitor monitor, string localPath, string serverUrl, long startLength, long sumLength)
		{
			long num = 0L;
			FileStream fileStream = null;
			bool result;
			try
			{
				if (File.Exists(localPath))
				{
					fileStream = File.OpenWrite(localPath);
					num = fileStream.Length;
					fileStream.Seek(num, SeekOrigin.Current);
				}
				else
				{
					fileStream = new FileStream(localPath, FileMode.Create);
				}
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(serverUrl);
				httpWebRequest.Timeout = 2000;
				if (num > 0L)
				{
					httpWebRequest.AddRange(num);
				}
				Stream responseStream;
				try
				{
					responseStream = httpWebRequest.GetResponse().GetResponseStream();
				}
				catch (InvalidOperationException exception)
				{
					LogConfig.Logger.Error("尝试断点续传时出错，重新开始下载", exception);
					num = 0L;
					fileStream.Seek(0L, SeekOrigin.Begin);
					httpWebRequest = (HttpWebRequest)WebRequest.Create(serverUrl);
					httpWebRequest.Timeout = 2000;
					responseStream = httpWebRequest.GetResponse().GetResponseStream();
				}
				using (responseStream)
				{
					responseStream.ReadTimeout = 2000;
					byte[] buffer = new byte[512];
					int i = responseStream.Read(buffer, 0, 512);
					num += (long)i;
					while (i > 0)
					{
						fileStream.Write(buffer, 0, i);
						i = responseStream.Read(buffer, 0, 512);
						float num2;
						if (sumLength <= 0L)
						{
							num2 = 0f;
						}
						else
						{
							num2 = (float)(num + startLength) / (float)sumLength;
						}
						if (num2 < 0f)
						{
							num2 = 0f;
						}
						if (num2 > 1f)
						{
							num2 = 0.99f;
						}
						monitor.UpdateProgress(num2);
						num += (long)i;
						if (monitor.IsCancelled)
						{
							fileStream.Close();
							return false;
						}
					}
					fileStream.Close();
					FileInfo fileInfo = new FileInfo(localPath);
					string text = fileInfo.Name.Replace(fileInfo.Extension, "").ToUpper();
					fileStream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
					HashAlgorithm hashAlgorithm = MD5.Create();
					byte[] value = hashAlgorithm.ComputeHash(fileStream);
					string value2 = BitConverter.ToString(value).Replace("-", "").ToUpper();
					if (text.Equals(value2))
					{
						result = true;
					}
					else
					{
						LogConfig.Logger.Error("下载的文件与其MD5码不对应");
						File.Delete(localPath);
						result = false;
					}
				}
			}
			catch (Exception exception2)
			{
				LogConfig.Logger.Error("自动更新下载文件时出错", exception2);
				if (fileStream != null)
				{
					fileStream.Dispose();
				}
				fileStream = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002E24 File Offset: 0x00001024
		private void InitDirectory(string dir, List<string> retainFiles)
		{
			if (string.IsNullOrEmpty(dir))
			{
				return;
			}
			try
			{
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
				else
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(dir);
					foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
					{
						directoryInfo2.Delete(true);
					}
					foreach (FileInfo fileInfo in directoryInfo.GetFiles())
					{
						bool flag = false;
						foreach (string value in retainFiles)
						{
							if (fileInfo.FullName.Equals(value))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							fileInfo.Delete();
						}
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("重置目录{0}时出错", dir), exception);
			}
		}
	}
}
