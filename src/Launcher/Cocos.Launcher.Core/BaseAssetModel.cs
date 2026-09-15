using System;
using System.IO;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Core;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;

namespace Cocos.Launcher.Core
{
	[TypeExtensionPoint]
	public class BaseAssetModel : AssetModel
	{
		public virtual bool HasInstalled
		{
			get
			{
				return false;
			}
		}

		public BaseAssetModel()
		{
		}

		public BaseAssetModel(Plugin model) : base(model)
		{
		}

		public override void Gain()
		{
			base.AssetInfo.PluginFraction = 0f;
			base.AssetInfo.IsInstalled = false;
			base.DeleteFile(base.AssetInfo.PluginPath);
			base.SendDownloadSelf(base.AssetInfo.PluginPath);
		}

		public override void Update(Plugin newInfo = null)
		{
			if (newInfo == null && string.IsNullOrEmpty(base.AssetInfo.DownloadUrlFromService))
			{
				return;
			}
			base.DeleteFile(base.AssetInfo.PluginPath);
			string pluginUrl = base.AssetInfo.PluginUrl;
			if (newInfo != null)
			{
				base.AssetInfo.PluginVersion = newInfo.PluginVersion;
				base.AssetInfo.PluginUrl = newInfo.PluginUrl;
			}
			else
			{
				base.AssetInfo.PluginVersion = base.AssetInfo.VersionFromService;
				base.AssetInfo.PluginUrl = base.AssetInfo.DownloadUrlFromService;
			}
			base.AssetInfo.PluginFraction = 0f;
			base.AssetInfo.PluginPath = string.Empty;
			base.AssetInfo.IsInstalled = false;
			base.SendDownloadSelf(pluginUrl);
		}

		public override bool CanInstall()
		{
			return base.CanInstall();
		}

		public override void Install(bool slient = false)
		{
			if (base.AssetInfo.IsInstalling)
			{
				return;
			}
			if (!File.Exists(base.AssetInfo.PluginPath))
			{
				this.SetUpdateModelToGain();
				return;
			}
			if (this.ExistsToFull())
			{
				MessageBox.Show(string.Format(LanguageInfo.Launcher_AlreadyInstalled, base.AssetInfo.PluginName), MessageBoxImage.Info, null, null);
				this.SetUpdateModelToOpen();
				return;
			}
			if (!this.CanInstall())
			{
				return;
			}
			Task.Run(delegate()
			{
				IProgressMonitor progressMonitor;
				try
				{
					this.AssetInfo.IsInstalling = true;
					if (slient)
					{
						progressMonitor = this.OnInstallSlient();
					}
					else
					{
						progressMonitor = this.OnInstall();
					}
				}
				catch (Exception ex)
				{
					progressMonitor = CocoStudio.Core.Services.ProgressMonitors.Default;
					progressMonitor.ReportError(this.AssetInfo.PluginFullName + "安装失败：" + ex, ex);
					LogConfig.Logger.Error(this.AssetInfo.PluginFullName + "安装失败", ex);
				}
				this.SendInstallEvent(progressMonitor);
			});
		}

		public override void Open()
		{
			if (this.isOpenling)
			{
				return;
			}
			this.isOpenling = true;
			GLib.Timeout.Add(1500U, delegate
			{
				this.isOpenling = false;
				return false;
			});
			IProgressMonitor progressMonitor;
			try
			{
				progressMonitor = this.OnOpen();
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error(base.AssetInfo.PluginFullName + "打开失败：" + ex);
				progressMonitor = CocoStudio.Core.Services.ProgressMonitors.Default;
				progressMonitor.ReportError(base.AssetInfo.PluginFullName + "打开失败：" + ex, ex);
			}
			this.SendOpenEvent(progressMonitor);
		}

		public override void Uninstall()
		{
			if (base.AssetInfo.IsUninstalling)
			{
				return;
			}
			if (base.AssetInfo.IsInstalled && !this.HasInstalled)
			{
				string dialog_ButtonOK = LanguageInfo.Dialog_ButtonOK;
				ButtonText btnText = new ButtonText(dialog_ButtonOK, false);
				MessageBoxResult messageBoxResult = MessageBox.Show(string.Format(LanguageInfo.Launcher_AlreadyUninstalled, base.AssetInfo.PluginName), btnText, MessageBoxImage.Info, null, EnumMainButton.Yes, null);
				if (messageBoxResult == MessageBoxResult.Yes)
				{
					base.DeleteItem();
				}
				return;
			}
			IProgressMonitor monitor = CocoStudio.Core.Services.ProgressMonitors.Default;
			if (!this.UninstallPrompt())
			{
				monitor.ReportError("", null);
				this.SendUninstallEvent(monitor);
				return;
			}
			Task.Run(delegate()
			{
				try
				{
					this.AssetInfo.IsUninstalling = true;
					monitor = this.OnUninstall();
				}
				catch (Exception ex)
				{
					monitor.ReportError(this.AssetInfo.PluginFullName + "卸载失败：" + ex, ex);
				}
				this.SendUninstallEvent(monitor);
			});
		}

		public override void Delete()
		{
			if (!this.DeletePrompt(LanguageInfo.Launcher_MsgConfirmDelete, base.AssetInfo.PluginName))
			{
				return;
			}
			IProgressMonitor @default = CocoStudio.Core.Services.ProgressMonitors.Default;
			try
			{
				this.OnDelete();
			}
			catch (Exception ex)
			{
				@default.ReportError(base.AssetInfo.PluginFullName + "删除失败：" + ex, ex);
			}
			this.SendUninstallEvent(@default);
		}

		protected virtual IProgressMonitor OnInstall()
		{
			return null;
		}

		protected virtual IProgressMonitor OnInstallSlient()
		{
			return null;
		}

		protected virtual IProgressMonitor OnOpen()
		{
			return null;
		}

		protected virtual IProgressMonitor OnUninstall()
		{
			return null;
		}

		protected virtual void OnDelete()
		{
			if (File.Exists(base.AssetInfo.PluginPath))
			{
				File.Delete(base.AssetInfo.PluginPath);
			}
		}

		public virtual bool ExistsToFull()
		{
			return false;
		}

		public bool DeletePrompt(string info, string name)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show(string.Format(info, name), MessageBoxButton.YesNo, MessageBoxImage.Question, null, EnumMainButton.Yes, null);
			return messageBoxResult == MessageBoxResult.Yes;
		}

		public virtual bool UninstallPrompt()
		{
			return false;
		}

		private void SendOpenEvent(IProgressMonitor monitor)
		{
			if (!monitor.AsyncOperation.Success)
			{
				this.SetUpdateModelToGain();
			}
		}

		private void SendInstallEvent(IProgressMonitor monitor)
		{
			GLib.Timeout.Add(0U, delegate
			{
				if (monitor.AsyncOperation.Success)
				{
					this.SetUpdateModelToOpen();
				}
				this.AssetInfo.IsInstalling = false;
				return false;
			});
		}

		private void SendUninstallEvent(IProgressMonitor monitor)
		{
			GLib.Timeout.Add(0U, delegate
			{
				if (monitor.AsyncOperation.Success)
				{
					this.DeleteItem();
				}
				else
				{
					NullProgressMonitor nullProgressMonitor = monitor as NullProgressMonitor;
					if (nullProgressMonitor != null)
					{
						LogConfig.Logger.Error(nullProgressMonitor.Messages);
					}
				}
				this.AssetInfo.IsUninstalling = false;
				return false;
			});
		}

		public void SetUpdateModelToOpen()
		{
			base.RunMode = RunModeEnum.Open;
			base.AssetInfo.IsInstalled = true;
			if (base.AssetInfo.OpenType == OperationType.exe.ToString())
			{
				base.UninstallMode = UninstallModeEnum.Uninstall;
			}
			DownloadService.Instance.AssetManager.SavePluginListInfo();
		}

		public void SetUpdateModelToGain()
		{
			MessageBox.Show(string.Format(LanguageInfo.Launcher_NotExist, base.AssetInfo.PluginName), MessageBoxImage.Info, null, null);
			base.RunMode = RunModeEnum.Gain;
		}
	}
}
