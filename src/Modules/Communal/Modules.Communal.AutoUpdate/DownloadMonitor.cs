using System;
using System.IO;
using System.Threading;
using MonoDevelop.Core;

namespace Modules.Communal.AutoUpdate
{
	public class DownloadMonitor
	{
		public ServerUpdateInfo ServerInfo { get; private set; }

		public bool NeedUpdateStudio { get; private set; }

		public bool NeedUpdateRuntime { get; private set; }

		public string StudioInstallFilePath
		{
			get
			{
				if (this.ServerInfo == null || string.IsNullOrEmpty(this.ServerInfo.MD5Value))
				{
					throw new Exception("Failed to get install file path");
				}
				string extension = Path.GetExtension(this.ServerInfo.DownloadUrl);
				return Path.Combine(PathHelper.AutoUpdateTempPath, this.ServerInfo.MD5Value + extension);
			}
		}

		public string WinSmallPackageFilePath
		{
			get
			{
				if (!Platform.IsWindows)
				{
					throw new Exception("Can only be used in Windows");
				}
				WinServerInfo winServerInfo = this.ServerInfo as WinServerInfo;
				if (winServerInfo == null || string.IsNullOrEmpty(winServerInfo.SmallPackageMD5))
				{
					throw new Exception("Failed to get small install file path");
				}
				string extension = Path.GetExtension(winServerInfo.SmallPackageLink);
				return Path.Combine(PathHelper.AutoUpdateTempPath, winServerInfo.SmallPackageMD5 + extension);
			}
		}

		public string MacRuntimeInstallFilePath
		{
			get
			{
				if (!Platform.IsMac)
				{
					throw new Exception("Can only be used in Mac");
				}
				MacServerInfo macServerInfo = this.ServerInfo as MacServerInfo;
				if (macServerInfo == null || string.IsNullOrEmpty(macServerInfo.RuntimeMD5))
				{
					throw new Exception("Failed to get runtime install file path");
				}
				string extension = Path.GetExtension(macServerInfo.RuntimeDownloadLink);
				return Path.Combine(PathHelper.AutoUpdateTempPath, macServerInfo.RuntimeMD5 + extension);
			}
		}

		public float CurrentProgress
		{
			get
			{
				return this.currentProgress;
			}
			private set
			{
				if (value < 0f)
				{
					this.currentProgress = 0f;
					return;
				}
				if (value <= 1f)
				{
					this.currentProgress = value;
					return;
				}
				this.currentProgress = 1f;
			}
		}

		public bool HasStarted { get; private set; }

		public bool IsDownloading { get; private set; }

		public bool IsSuccessed { get; private set; }

		public bool IsCancelled
		{
			get
			{
				return this.cancelToken.Token.IsCancellationRequested;
			}
		}

		public event EventHandler<ProgressChangedArgs> ProgressChanged;

		public event EventHandler<DownloadFinishedArgs> DownloadFinished;

		public DownloadMonitor(ServerUpdateInfo info, bool updateStudio, bool updateRuntime)
		{
			this.NeedUpdateStudio = updateStudio;
			this.NeedUpdateRuntime = updateRuntime;
			this.Reset(info);
		}

		public void Reset(ServerUpdateInfo info)
		{
			if (info == null || !info.LinkSuccess || !info.LoadSuccess)
			{
				throw new Exception("无效的服务器更新信息");
			}
			this.CurrentProgress = 0f;
			this.ServerInfo = info;
			this.HasStarted = false;
			this.IsDownloading = false;
			this.IsSuccessed = false;
			if (this.cancelToken != null)
			{
				this.cancelToken.Dispose();
			}
			this.cancelToken = new CancellationTokenSource();
		}

		public void Start()
		{
			this.HasStarted = true;
			this.IsDownloading = true;
			this.IsSuccessed = false;
			if (this.cancelToken != null)
			{
				this.cancelToken.Dispose();
			}
			this.cancelToken = new CancellationTokenSource();
		}

		public void UpdateProgress(float progress)
		{
			this.CurrentProgress = progress;
			if (this.ProgressChanged != null)
			{
				ProgressChangedArgs e = new ProgressChangedArgs(this.CurrentProgress);
				this.ProgressChanged(this, e);
			}
		}

		public void Cancel()
		{
			if (this.IsDownloading)
			{
				this.cancelToken.Cancel();
				this.Finish(false, "Download canceled");
			}
		}

		public void Finish(bool isSuccessed, string output = "")
		{
			this.IsSuccessed = isSuccessed;
			this.IsDownloading = false;
			if (isSuccessed)
			{
				this.UpdateProgress(1f);
			}
			else
			{
				this.UpdateProgress(0f);
			}
			if (this.DownloadFinished != null)
			{
				DownloadFinishedArgs e = new DownloadFinishedArgs(isSuccessed, output);
				this.DownloadFinished(this, e);
			}
		}

		private float currentProgress;

		private CancellationTokenSource cancelToken;
	}
}
