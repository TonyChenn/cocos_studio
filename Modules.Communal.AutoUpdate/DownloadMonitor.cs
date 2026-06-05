using System;
using System.IO;
using System.Threading;
using MonoDevelop.Core;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x02000002 RID: 2
	public class DownloadMonitor
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public ServerUpdateInfo ServerInfo { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		public bool NeedUpdateStudio { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002072 File Offset: 0x00000272
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000207A File Offset: 0x0000027A
		public bool NeedUpdateRuntime { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002084 File Offset: 0x00000284
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020E4 File Offset: 0x000002E4
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000214C File Offset: 0x0000034C
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

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000021B4 File Offset: 0x000003B4
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000021BC File Offset: 0x000003BC
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000021ED File Offset: 0x000003ED
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000021F5 File Offset: 0x000003F5
		public bool HasStarted { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021FE File Offset: 0x000003FE
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002206 File Offset: 0x00000406
		public bool IsDownloading { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000220F File Offset: 0x0000040F
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002217 File Offset: 0x00000417
		public bool IsSuccessed { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002220 File Offset: 0x00000420
		public bool IsCancelled
		{
			get
			{
				return this.cancelToken.Token.IsCancellationRequested;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000013 RID: 19 RVA: 0x00002240 File Offset: 0x00000440
		// (remove) Token: 0x06000014 RID: 20 RVA: 0x00002278 File Offset: 0x00000478
		public event EventHandler<ProgressChangedArgs> ProgressChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000015 RID: 21 RVA: 0x000022B0 File Offset: 0x000004B0
		// (remove) Token: 0x06000016 RID: 22 RVA: 0x000022E8 File Offset: 0x000004E8
		public event EventHandler<DownloadFinishedArgs> DownloadFinished;

		// Token: 0x06000017 RID: 23 RVA: 0x0000231D File Offset: 0x0000051D
		public DownloadMonitor(ServerUpdateInfo info, bool updateStudio, bool updateRuntime)
		{
			this.NeedUpdateStudio = updateStudio;
			this.NeedUpdateRuntime = updateRuntime;
			this.Reset(info);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000233C File Offset: 0x0000053C
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

		// Token: 0x06000019 RID: 25 RVA: 0x000023AC File Offset: 0x000005AC
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

		// Token: 0x0600001A RID: 26 RVA: 0x000023E4 File Offset: 0x000005E4
		public void UpdateProgress(float progress)
		{
			this.CurrentProgress = progress;
			if (this.ProgressChanged != null)
			{
				ProgressChangedArgs e = new ProgressChangedArgs(this.CurrentProgress);
				this.ProgressChanged(this, e);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002419 File Offset: 0x00000619
		public void Cancel()
		{
			if (this.IsDownloading)
			{
				this.cancelToken.Cancel();
				this.Finish(false, "Download canceled");
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000243C File Offset: 0x0000063C
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

		// Token: 0x04000001 RID: 1
		private float currentProgress;

		// Token: 0x04000002 RID: 2
		private CancellationTokenSource cancelToken;
	}
}
