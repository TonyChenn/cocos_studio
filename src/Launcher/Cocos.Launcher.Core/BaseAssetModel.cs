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
	// Token: 0x02000019 RID: 25
	[TypeExtensionPoint]
	public class BaseAssetModel : AssetModel
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000059BB File Offset: 0x00003BBB
		public virtual bool HasInstalled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000059BE File Offset: 0x00003BBE
		public BaseAssetModel()
		{
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000059C6 File Offset: 0x00003BC6
		public BaseAssetModel(Plugin model) : base(model)
		{
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000059D0 File Offset: 0x00003BD0
		public override void Gain()
		{
			base.AssetInfo.PluginFraction = 0f;
			base.AssetInfo.IsInstalled = false;
			base.DeleteFile(base.AssetInfo.PluginPath);
			base.SendDownloadSelf(base.AssetInfo.PluginPath);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005A1C File Offset: 0x00003C1C
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

		// Token: 0x060000DE RID: 222 RVA: 0x00005AE3 File Offset: 0x00003CE3
		public override bool CanInstall()
		{
			return base.CanInstall();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005BA8 File Offset: 0x00003DA8
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

		// Token: 0x060000E0 RID: 224 RVA: 0x00005C44 File Offset: 0x00003E44
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

		// Token: 0x060000E1 RID: 225 RVA: 0x00005D68 File Offset: 0x00003F68
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

		// Token: 0x060000E2 RID: 226 RVA: 0x00005E28 File Offset: 0x00004028
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

		// Token: 0x060000E3 RID: 227 RVA: 0x00005E98 File Offset: 0x00004098
		protected virtual IProgressMonitor OnInstall()
		{
			return null;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005E9B File Offset: 0x0000409B
		protected virtual IProgressMonitor OnInstallSlient()
		{
			return null;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005E9E File Offset: 0x0000409E
		protected virtual IProgressMonitor OnOpen()
		{
			return null;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005EA1 File Offset: 0x000040A1
		protected virtual IProgressMonitor OnUninstall()
		{
			return null;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005EA4 File Offset: 0x000040A4
		protected virtual void OnDelete()
		{
			if (File.Exists(base.AssetInfo.PluginPath))
			{
				File.Delete(base.AssetInfo.PluginPath);
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005EC8 File Offset: 0x000040C8
		public virtual bool ExistsToFull()
		{
			return false;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005ECC File Offset: 0x000040CC
		public bool DeletePrompt(string info, string name)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show(string.Format(info, name), MessageBoxButton.YesNo, MessageBoxImage.Question, null, EnumMainButton.Yes, null);
			return messageBoxResult == MessageBoxResult.Yes;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005EF4 File Offset: 0x000040F4
		public virtual bool UninstallPrompt()
		{
			return false;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005EF7 File Offset: 0x000040F7
		private void SendOpenEvent(IProgressMonitor monitor)
		{
			if (!monitor.AsyncOperation.Success)
			{
				this.SetUpdateModelToGain();
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005F48 File Offset: 0x00004148
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

		// Token: 0x060000ED RID: 237 RVA: 0x00005FE4 File Offset: 0x000041E4
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

		// Token: 0x060000EE RID: 238 RVA: 0x00006018 File Offset: 0x00004218
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

		// Token: 0x060000EF RID: 239 RVA: 0x0000606B File Offset: 0x0000426B
		public void SetUpdateModelToGain()
		{
			MessageBox.Show(string.Format(LanguageInfo.Launcher_NotExist, base.AssetInfo.PluginName), MessageBoxImage.Info, null, null);
			base.RunMode = RunModeEnum.Gain;
		}
	}
}
