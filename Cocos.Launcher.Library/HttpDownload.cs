using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CocoStudio.Basic;
using GLib;

namespace Cocos.Launcher.Library
{
	// Token: 0x02000002 RID: 2
	public class HttpDownload
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private bool IsCanceled
		{
			get
			{
				return this.isAsync && this.cancelToken != null && this.cancelToken.Token.IsCancellationRequested;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000002 RID: 2 RVA: 0x00002084 File Offset: 0x00000284
		// (remove) Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		public event EventHandler<ProgressChangedEventArgs> ProgressChanged = delegate(object param0, ProgressChangedEventArgs param1)
		{
		};

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000004 RID: 4 RVA: 0x000020F4 File Offset: 0x000002F4
		// (remove) Token: 0x06000005 RID: 5 RVA: 0x0000212C File Offset: 0x0000032C
		public event EventHandler<DownloadFinishedEventArgs> DownloadFinished;

		// Token: 0x06000006 RID: 6 RVA: 0x00002164 File Offset: 0x00000364
		public HttpDownload(string networkUrl, string localPath)
		{
			this.networkUrl = networkUrl;
			this.localPath = localPath;
			string directoryName = Path.GetDirectoryName(localPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021DC File Offset: 0x000003DC
		public void StartDownloadAsync()
		{
			if (this.isDownloading)
			{
				return;
			}
			this.isAsync = true;
			if (this.cancelToken != null)
			{
				this.cancelToken.Dispose();
			}
			this.cancelToken = new CancellationTokenSource();
			Task task = new Task(delegate()
			{
				this.Download();
			});
			task.Start();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000222F File Offset: 0x0000042F
		public void StartDownload()
		{
			if (this.isDownloading)
			{
				return;
			}
			this.isAsync = false;
			if (this.cancelToken != null)
			{
				this.cancelToken.Dispose();
			}
			this.cancelToken = null;
			this.Download();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002264 File Offset: 0x00000464
		private void Download()
		{
			if (this.isDownloading)
			{
				return;
			}
			this.isSuccessed = false;
			this.isDownloading = true;
			this.error = null;
			this.m_time = Environment.TickCount;
			FileStream fileStream = null;
			if (string.IsNullOrEmpty(this.localPath))
			{
				this.error = new Exception("The file path is empty.");
				return;
			}
			if (File.Exists(this.localPath))
			{
				try
				{
					fileStream = File.OpenWrite(this.localPath);
					fileStream.Seek(fileStream.Length, SeekOrigin.Current);
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error(string.Format("Failed to read \"{0}\"", this.localPath), exception);
					fileStream = null;
				}
			}
			if (fileStream == null)
			{
				try
				{
					fileStream = new FileStream(this.localPath, FileMode.Create, FileAccess.ReadWrite);
				}
				catch (Exception exception2)
				{
					LogConfig.Logger.Error(string.Format("Failed to create FileStream using \"{0}\"", this.localPath), exception2);
					this.error = exception2;
					return;
				}
			}
			this.startPos = fileStream.Length;
			this.DownloadFile(fileStream);
			this.FinishingDownload();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000237C File Offset: 0x0000057C
		private void DownloadFile(FileStream fs)
		{
			if (fs == null)
			{
				return;
			}
			try
			{
				GC.Collect();
				HttpWebRequest httpWebRequest = this.networkUrl.CreatRequest("get");
				if (this.startPos > 0L)
				{
					httpWebRequest.AddRange((int)this.startPos);
				}
				using (Stream responseStream = httpWebRequest.GetResponse().GetResponseStream())
				{
					responseStream.ReadTimeout = this.timeOut;
					long contentLength = httpWebRequest.GetResponse().ContentLength;
					this.sumLength = this.startPos + contentLength;
					byte[] buffer = new byte[512];
					int num = responseStream.Read(buffer, 0, 512);
					this.startPos += (long)num;
					int num2 = 0;
					while (num > 0 && !this.IsCanceled)
					{
						fs.Write(buffer, 0, num);
						num = responseStream.Read(buffer, 0, 512);
						if (num2 < 400)
						{
							num2++;
						}
						else
						{
							GLib.Timeout.Add(0U, delegate
							{
								this.UpdateProgress();
								return false;
							});
							num2 = 0;
						}
						this.startPos += (long)num;
					}
				}
				if (!this.IsCanceled)
				{
					if (this.sumLength != this.startPos)
					{
						this.error = new InvalidOperationException("The length of the file is mismatched.");
					}
					else
					{
						this.isSuccessed = true;
					}
				}
			}
			catch (Exception ex)
			{
				this.error = ex;
			}
			finally
			{
				if (fs != null)
				{
					fs.Dispose();
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002590 File Offset: 0x00000790
		private void FinishingDownload()
		{
			this.isDownloading = false;
			string errorMessage = string.Empty;
			if (!this.IsCanceled && this.error != null)
			{
				if (this.error is WebException && (this.error as WebException).Status == WebExceptionStatus.Timeout && this.currentTimeOutNumber < this.m_tryagain)
				{
					this.currentTimeOutNumber++;
					this.StartDownloadAsync();
					return;
				}
				errorMessage = "Failed to download: " + this.error.Message;
			}
			this.currentTimeOutNumber = 0;
			GLib.Timeout.Add(0U, delegate
			{
				this.UpdateProgress();
				if (this.DownloadFinished != null)
				{
					this.DownloadFinished(this, new DownloadFinishedEventArgs(this.isSuccessed, errorMessage, this.localPath));
				}
				return false;
			});
			if (this.cancelToken != null)
			{
				this.cancelToken.Dispose();
			}
			this.cancelToken = null;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000265F File Offset: 0x0000085F
		public void StopDownload()
		{
			if (!this.isAsync || this.cancelToken == null)
			{
				return;
			}
			this.cancelToken.Cancel();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002680 File Offset: 0x00000880
		private void UpdateProgress()
		{
			if (this.sumLength >= this.startPos && this.sumLength != 0L)
			{
				float fraction = (float)this.startPos * 100f / (float)this.sumLength;
				float filesize = (float)this.sumLength / 1024f / 1024f;
				float loadSpeedToFloat = this.GetLoadSpeedToFloat();
				long remainTime = this.GetRemainTime(loadSpeedToFloat);
				this.m_time = Environment.TickCount;
				if (this.ProgressChanged != null)
				{
					this.ProgressChanged(this, new ProgressChangedEventArgs(fraction, filesize, loadSpeedToFloat, remainTime));
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002708 File Offset: 0x00000908
		private float GetLoadSpeedToFloat()
		{
			float num = (float)(Environment.TickCount - this.m_time);
			float num2 = num / 1000f;
			if (num2 == 0f)
			{
				num2 = 0.001f;
			}
			return 200f / num2;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002740 File Offset: 0x00000940
		private long GetRemainTime(float speed)
		{
			float num = (float)(this.sumLength - this.startPos);
			float num2 = num / 1024f / speed;
			return (long)num2;
		}

		// Token: 0x04000001 RID: 1
		private long startPos;

		// Token: 0x04000002 RID: 2
		private long sumLength;

		// Token: 0x04000003 RID: 3
		private string networkUrl;

		// Token: 0x04000004 RID: 4
		private string localPath;

		// Token: 0x04000005 RID: 5
		private bool isSuccessed;

		// Token: 0x04000006 RID: 6
		private bool isDownloading;

		// Token: 0x04000007 RID: 7
		private int m_time;

		// Token: 0x04000008 RID: 8
		private int timeOut = 10000;

		// Token: 0x04000009 RID: 9
		private int m_tryagain = 10;

		// Token: 0x0400000A RID: 10
		private int currentTimeOutNumber;

		// Token: 0x0400000B RID: 11
		private Exception error;

		// Token: 0x0400000C RID: 12
		private CancellationTokenSource cancelToken;

		// Token: 0x0400000D RID: 13
		private bool isAsync;
	}
}
