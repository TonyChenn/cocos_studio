using System;
using System.IO;
using Cocos.Launcher.Control;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using GLib;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000018 RID: 24
	public class AssetModel
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00005088 File Offset: 0x00003288
		public virtual int Order
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00005094 File Offset: 0x00003294
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x0000508B File Offset: 0x0000328B
		public bool CanInstallSilent
		{
			get
			{
				return this.canInstallSilent;
			}
			private set
			{
				this.canInstallSilent = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000509C File Offset: 0x0000329C
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000050A4 File Offset: 0x000032A4
		public Plugin AssetInfo { get; private set; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000AB RID: 171 RVA: 0x000050B0 File Offset: 0x000032B0
		// (remove) Token: 0x060000AC RID: 172 RVA: 0x000050E8 File Offset: 0x000032E8
		public event EventHandler<PluginDownloadEventArgs> DownloadSelf;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000AD RID: 173 RVA: 0x00005120 File Offset: 0x00003320
		// (remove) Token: 0x060000AE RID: 174 RVA: 0x00005158 File Offset: 0x00003358
		public event EventHandler<DownloadSucceedEventArgs> DownloadSucceed;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000AF RID: 175 RVA: 0x00005190 File Offset: 0x00003390
		// (remove) Token: 0x060000B0 RID: 176 RVA: 0x000051C8 File Offset: 0x000033C8
		public event EventHandler<EventArgs> DeleteSelf;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000B1 RID: 177 RVA: 0x00005200 File Offset: 0x00003400
		// (remove) Token: 0x060000B2 RID: 178 RVA: 0x00005238 File Offset: 0x00003438
		public event EventHandler<RefreshRunModeEventArgs> RefreshRunModeEvent;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000B3 RID: 179 RVA: 0x00005270 File Offset: 0x00003470
		// (remove) Token: 0x060000B4 RID: 180 RVA: 0x000052A8 File Offset: 0x000034A8
		public event EventHandler<RefreshUninstallModeEventArgs> RefreshUninstallModeEvent;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000B5 RID: 181 RVA: 0x000052E0 File Offset: 0x000034E0
		// (remove) Token: 0x060000B6 RID: 182 RVA: 0x00005318 File Offset: 0x00003518
		public event EventHandler<ProgressChangedEventArgs> DownLoadProgressChanged;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000534D File Offset: 0x0000354D
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00005355 File Offset: 0x00003555
		public RunModeEnum RunMode
		{
			get
			{
				return this.runMode;
			}
			protected set
			{
				this.runMode = value;
				this.UpdateRunMode(this.runMode);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000536A File Offset: 0x0000356A
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00005372 File Offset: 0x00003572
		public UninstallModeEnum UninstallMode
		{
			get
			{
				return this.uninstallMode;
			}
			protected set
			{
				this.uninstallMode = value;
				this.UpdateUninstallMode(this.uninstallMode);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005387 File Offset: 0x00003587
		public AssetModel()
		{
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000538F File Offset: 0x0000358F
		public AssetModel(Plugin model)
		{
			this.AssetInfo = model;
			this.InitRunMode();
			this.InitUninstallMode();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000053AC File Offset: 0x000035AC
		private void InitDownload()
		{
			if (this.download == null)
			{
				this.InitAssetInfo();
				this.download = new HttpDownload(this.AssetInfo.PluginUrl, this.AssetInfo.PluginPath);
				this.download.DownloadFinished += this.DownloadFinishedEventHandler;
				this.download.ProgressChanged += this.DownloadProgressChangedHandler;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005418 File Offset: 0x00003618
		private void InitAssetInfo()
		{
			if (string.IsNullOrEmpty(this.AssetInfo.PluginPath))
			{
				this.AssetInfo.PluginPath = (this.AssetInfo.UnZipPath = this.GetPathByName(this.AssetInfo.PluginUrl));
			}
			if (string.IsNullOrEmpty(this.AssetInfo.ImagePath))
			{
				this.AssetInfo.ImagePath = this.GetImagePath(this.AssetInfo.ImageUrl);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005490 File Offset: 0x00003690
		private string GetImagePath(string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return string.Empty;
			}
			string[] array = imageUrl.Split(new char[]
			{
				'/'
			});
			return Path.Combine(ConstantConfig.Paths.AssetStoreImagePath, array[array.Length - 1]);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000054D8 File Offset: 0x000036D8
		private string GetPathByName(string downloadUrl)
		{
			if (string.IsNullOrEmpty(downloadUrl))
			{
				return string.Empty;
			}
			string[] array = downloadUrl.Split(new char[]
			{
				'/'
			});
			string path = Path.Combine(Option.UserConfig.CocosStorePath, array[array.Length - 1]);
			string extension = Path.GetExtension(path);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			return this.GetPath(fileNameWithoutExtension, extension);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000553C File Offset: 0x0000373C
		private string GetPath(string name, string suffix)
		{
			string text = string.Empty;
			int num = 0;
			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(suffix) || num < 0)
			{
				return text;
			}
			text = Path.Combine(Option.UserConfig.CocosStorePath, name + suffix);
			while (File.Exists(text))
			{
				text = Path.Combine(Option.UserConfig.CocosStorePath, name + string.Format("({0})", ++num) + suffix);
			}
			return text;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000055B4 File Offset: 0x000037B4
		public virtual bool InitRunMode()
		{
			if (!File.Exists(this.AssetInfo.PluginPath))
			{
				this.RunMode = RunModeEnum.Gain;
				return true;
			}
			return false;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000055D2 File Offset: 0x000037D2
		public virtual bool InitUninstallMode()
		{
			this.UninstallMode = UninstallModeEnum.Delete;
			return true;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000055DC File Offset: 0x000037DC
		public virtual bool CanHandle(Plugin pluginModel)
		{
			return false;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000055E0 File Offset: 0x000037E0
		public void StartDownload()
		{
			try
			{
				this.InitDownload();
				this.download.StartDownloadAsync();
				this.AssetInfo.IsLoading = true;
				this.UpdateProgress(this.AssetInfo.PluginFraction, this.AssetInfo.PluginSize, 0f, -1L);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("继续下载错误: 存储路径:{0}，下载路径:{1}", this.AssetInfo.PluginPath, this.AssetInfo.PluginUrl), exception);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005670 File Offset: 0x00003870
		public void StopDownload()
		{
			if (!this.AssetInfo.IsLoading)
			{
				return;
			}
			if (this.download != null)
			{
				this.download.StopDownload();
			}
			this.AssetInfo.IsLoading = false;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000569F File Offset: 0x0000389F
		public void DeleteItem()
		{
			this.DeleteFile(this.AssetInfo.PluginPath);
			if (this.DeleteSelf != null)
			{
				this.DeleteSelf(this, null);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000056D4 File Offset: 0x000038D4
		public void SetServiceInfo(ServicePluginInfo info)
		{
			if (this.AssetInfo.PluginVersion != info.Version)
			{
				this.AssetInfo.VersionFromService = info.Version;
				this.AssetInfo.DownloadUrlFromService = info.DownloadUrl;
				Timeout.Add(0U, delegate
				{
					this.RunMode = RunModeEnum.Update;
					return false;
				});
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005738 File Offset: 0x00003938
		public bool DeleteFile(string path)
		{
			bool result;
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				result = true;
			}
			catch (Exception arg)
			{
				LogConfig.Output.Error("文件删除失败" + arg);
				result = false;
			}
			return result;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005784 File Offset: 0x00003984
		protected virtual void UpdateRunMode(RunModeEnum runMode)
		{
			if (this.RefreshRunModeEvent != null)
			{
				this.RefreshRunModeEvent(this, new RefreshRunModeEventArgs(runMode));
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000057A0 File Offset: 0x000039A0
		protected virtual void UpdateUninstallMode(UninstallModeEnum uninstallMode)
		{
			if (this.RefreshUninstallModeEvent != null)
			{
				this.RefreshUninstallModeEvent(this, new RefreshUninstallModeEventArgs(uninstallMode));
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000057BC File Offset: 0x000039BC
		private void DownloadFinishedEventHandler(object sender, DownloadFinishedEventArgs e)
		{
			if (e.IsSuccessed)
			{
				this.download.DownloadFinished -= this.DownloadFinishedEventHandler;
				this.download.ProgressChanged -= this.DownloadProgressChangedHandler;
				this.download = null;
				this.AssetInfo.IsLoading = false;
				this.AssetInfo.PluginFraction = 100f;
				this.InitRunMode();
				this.InitUninstallMode();
				if (this.DownloadSucceed != null)
				{
					this.DownloadSucceed(this, new DownloadSucceedEventArgs(this.AssetInfo.PluginPath, this.AssetInfo.PluginUrl));
					return;
				}
			}
			else
			{
				if (!e.IsSuccessed && !string.IsNullOrEmpty(e.Error))
				{
					LogConfig.Output.Error("插件下载失败：" + e.Error);
					Services.OutputService.Info(string.Format(LanguageInfo.Launcher_DownloadFailed, this.AssetInfo.PluginName));
					this.UpdateProgress(this.AssetInfo.PluginFraction, this.AssetInfo.PluginSize, -1f, -1L);
					return;
				}
				this.UpdateProgress(this.AssetInfo.PluginFraction, this.AssetInfo.PluginSize, -1f, -1L);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000058FB File Offset: 0x00003AFB
		private void DownloadProgressChangedHandler(object sender, ProgressChangedEventArgs e)
		{
			this.UpdateProgress(e.Fraction, e.FileSize, e.DownloadSpeed, e.RemainTime);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000591B File Offset: 0x00003B1B
		private void UpdateProgress(float fraction, float totalSize, float speed = 0f, long remainTime = -1L)
		{
			if (this.DownLoadProgressChanged != null)
			{
				this.DownLoadProgressChanged(this, new ProgressChangedEventArgs(fraction, totalSize, speed, remainTime));
			}
			this.AssetInfo.PluginFraction = fraction;
			this.AssetInfo.PluginSize = totalSize;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005953 File Offset: 0x00003B53
		public void SendDownloadSelf(string oldUrl)
		{
			if (this.DownloadSelf != null)
			{
				this.DownloadSelf(this, new PluginDownloadEventArgs(this.AssetInfo, oldUrl));
			}
			PageManager.Instance.SetPluginNumber();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000597F File Offset: 0x00003B7F
		public virtual void Gain()
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005981 File Offset: 0x00003B81
		public virtual void Update(Plugin newInfo = null)
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005983 File Offset: 0x00003B83
		public virtual void Open()
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005985 File Offset: 0x00003B85
		public virtual bool CanInstall()
		{
			return true;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005988 File Offset: 0x00003B88
		public virtual void Install(bool slient = false)
		{
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000598A File Offset: 0x00003B8A
		public virtual void Uninstall()
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000598C File Offset: 0x00003B8C
		public virtual void Delete()
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005990 File Offset: 0x00003B90
		public virtual object Clone(Plugin info)
		{
			return Activator.CreateInstance(base.GetType(), new object[]
			{
				info
			}) as AssetModel;
		}

		// Token: 0x04000059 RID: 89
		public bool isOpenling;

		// Token: 0x0400005A RID: 90
		private bool canInstallSilent;

		// Token: 0x0400005B RID: 91
		protected HttpDownload download;

		// Token: 0x04000062 RID: 98
		private RunModeEnum runMode;

		// Token: 0x04000063 RID: 99
		private UninstallModeEnum uninstallMode;
	}
}
