using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CocoStudio.Basic;
using GLib;

namespace Cocos.Launcher.Library
{
	public class HttpDownload
	{
		private bool IsCanceled
		{
			get
			{
				return this.isAsync && this.cancelToken != null && this.cancelToken.Token.IsCancellationRequested;
			}
		}

		public event EventHandler<ProgressChangedEventArgs> ProgressChanged = delegate(object param0, ProgressChangedEventArgs param1)
		{
		};

		public event EventHandler<DownloadFinishedEventArgs> DownloadFinished;

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

		public void StopDownload()
		{
			if (!this.isAsync || this.cancelToken == null)
			{
				return;
			}
			this.cancelToken.Cancel();
		}

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

		private long GetRemainTime(float speed)
		{
			float num = (float)(this.sumLength - this.startPos);
			float num2 = num / 1024f / speed;
			return (long)num2;
		}

		private long startPos;

		private long sumLength;

		private string networkUrl;

		private string localPath;

		private bool isSuccessed;

		private bool isDownloading;

		private int m_time;

		private int timeOut = 10000;

		private int m_tryagain = 10;

		private int currentTimeOutNumber;

		private Exception error;

		private CancellationTokenSource cancelToken;

		private bool isAsync;
	}
}
