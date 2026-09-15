using System;
using System.IO;
using Cocos.Launcher.Control;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using GLib;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	public class AssetModel
	{
		public virtual int Order
		{
			get
			{
				return 0;
			}
		}

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

		public Plugin AssetInfo { get; private set; }

		public event EventHandler<PluginDownloadEventArgs> DownloadSelf;

		public event EventHandler<DownloadSucceedEventArgs> DownloadSucceed;

		public event EventHandler<EventArgs> DeleteSelf;

		public event EventHandler<RefreshRunModeEventArgs> RefreshRunModeEvent;

		public event EventHandler<RefreshUninstallModeEventArgs> RefreshUninstallModeEvent;

		public event EventHandler<ProgressChangedEventArgs> DownLoadProgressChanged;

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

		public AssetModel()
		{
		}

		public AssetModel(Plugin model)
		{
			this.AssetInfo = model;
			this.InitRunMode();
			this.InitUninstallMode();
		}

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

		public virtual bool InitRunMode()
		{
			if (!File.Exists(this.AssetInfo.PluginPath))
			{
				this.RunMode = RunModeEnum.Gain;
				return true;
			}
			return false;
		}

		public virtual bool InitUninstallMode()
		{
			this.UninstallMode = UninstallModeEnum.Delete;
			return true;
		}

		public virtual bool CanHandle(Plugin pluginModel)
		{
			return false;
		}

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

		public void DeleteItem()
		{
			this.DeleteFile(this.AssetInfo.PluginPath);
			if (this.DeleteSelf != null)
			{
				this.DeleteSelf(this, null);
			}
		}

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

		protected virtual void UpdateRunMode(RunModeEnum runMode)
		{
			if (this.RefreshRunModeEvent != null)
			{
				this.RefreshRunModeEvent(this, new RefreshRunModeEventArgs(runMode));
			}
		}

		protected virtual void UpdateUninstallMode(UninstallModeEnum uninstallMode)
		{
			if (this.RefreshUninstallModeEvent != null)
			{
				this.RefreshUninstallModeEvent(this, new RefreshUninstallModeEventArgs(uninstallMode));
			}
		}

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

		private void DownloadProgressChangedHandler(object sender, ProgressChangedEventArgs e)
		{
			this.UpdateProgress(e.Fraction, e.FileSize, e.DownloadSpeed, e.RemainTime);
		}

		private void UpdateProgress(float fraction, float totalSize, float speed = 0f, long remainTime = -1L)
		{
			if (this.DownLoadProgressChanged != null)
			{
				this.DownLoadProgressChanged(this, new ProgressChangedEventArgs(fraction, totalSize, speed, remainTime));
			}
			this.AssetInfo.PluginFraction = fraction;
			this.AssetInfo.PluginSize = totalSize;
		}

		public void SendDownloadSelf(string oldUrl)
		{
			if (this.DownloadSelf != null)
			{
				this.DownloadSelf(this, new PluginDownloadEventArgs(this.AssetInfo, oldUrl));
			}
			PageManager.Instance.SetPluginNumber();
		}

		public virtual void Gain()
		{
		}

		public virtual void Update(Plugin newInfo = null)
		{
		}

		public virtual void Open()
		{
		}

		public virtual bool CanInstall()
		{
			return true;
		}

		public virtual void Install(bool slient = false)
		{
		}

		public virtual void Uninstall()
		{
		}

		public virtual void Delete()
		{
		}

		public virtual object Clone(Plugin info)
		{
			return Activator.CreateInstance(base.GetType(), new object[]
			{
				info
			}) as AssetModel;
		}

		public bool isOpenling;

		private bool canInstallSilent;

		protected HttpDownload download;

		private RunModeEnum runMode;

		private UninstallModeEnum uninstallMode;
	}
}
