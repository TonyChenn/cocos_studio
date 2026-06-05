using System;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using GLib;
using Gtk;
using Mono.Addins;
using MonoDevelop.Ide;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x0200000C RID: 12
	[Extension(Type = typeof(ICommandHandle))]
	public class UpdateManager : ICommandHandle
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003602 File Offset: 0x00001802
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00003609 File Offset: 0x00001809
		public static UpdateWindow UpdateWindow { get; set; }

		// Token: 0x0600006F RID: 111 RVA: 0x00003614 File Offset: 0x00001814
		void ICommandHandle.Initialize()
		{
			GlobalCommand.CheckUpdateCmd.Execute += this.CheckUpdateCmd_Execute;
			GlobalCommand.CheckUpdateCmd.Update += this.CheckUpdateCmd_CanExecute;
			if (Services.MainWindow != null)
			{
				Services.MainWindow.InitializeCompleted += this.OnMainWindowInitialized;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000367D File Offset: 0x0000187D
		private void OnMainWindowInitialized(object sender, EventArgs e)
		{
			Services.MainWindow.InitializeCompleted -= this.OnMainWindowInitialized;
			GLib.Timeout.Add(3000U, delegate
			{
				GlobalCommand.CheckUpdateCmd.RaiseExecute(true);
				return false;
			});
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000036BD File Offset: 0x000018BD
		private void CheckUpdateCmd_CanExecute(object sender, CommandUpdateArgs args)
		{
			args.Info.Enabled = !this.isChecking;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000036F0 File Offset: 0x000018F0
		private void CheckUpdateCmd_Execute(object sender, CommandRunArgs args)
		{
			if (UpdateManager.UpdateWindow != null)
			{
				UpdateManager.UpdateWindow.Present();
				return;
			}
			bool isAutoRun = false;
			if (args.DataItem != null && args.DataItem.Equals(true))
			{
				isAutoRun = true;
			}
			Task task = new Task(delegate()
			{
				this.CheckUpdateAsync(isAutoRun);
			});
			task.Start();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003760 File Offset: 0x00001960
		private void CheckUpdateAsync(bool isAutoRun)
		{
			this.isChecking = true;
			try
			{
				string text;
				ServerUpdateInfo serverUpdateInfo = DetectHelper.GetServerUpdateInfo(out text);
				if (serverUpdateInfo == null)
				{
					if (isAutoRun)
					{
						LogConfig.Logger.Error(text);
					}
					else
					{
						this.ShowMessage(text);
					}
					this.isChecking = false;
					return;
				}
				string text2;
				bool flag = DetectHelper.CheckNeedUpdateStudio(serverUpdateInfo, out text2);
				string text3;
				bool flag2 = DetectHelper.CheckNeedUpdateRuntime(serverUpdateInfo, out text3);
				if (!flag2 && !flag)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text = text2;
					}
					else
					{
						text = text3;
					}
					if (!isAutoRun)
					{
						this.ShowMessage(text);
					}
					this.isChecking = false;
					return;
				}
				if (!DetectHelper.CheckCanUpdate(serverUpdateInfo, out text))
				{
					if (this.monitor == null)
					{
						this.monitor = new DownloadMonitor(serverUpdateInfo, false, false);
					}
					this.monitor.Cancel();
					this.monitor.Reset(serverUpdateInfo);
					string downloadLink;
					if (string.IsNullOrEmpty(serverUpdateInfo.FullPackageUrl))
					{
						downloadLink = "http://www.cocos2d-x.org/download";
					}
					else
					{
						downloadLink = serverUpdateInfo.FullPackageUrl;
					}
					this.ShowUpdateInfoWindow(downloadLink);
					this.isChecking = false;
					return;
				}
				bool flag3 = true;
				if (this.monitor != null && this.monitor.IsDownloading && this.monitor.CurrentProgress > 0f)
				{
					flag3 = false;
				}
				if (flag3)
				{
					this.monitor = new DownloadMonitor(serverUpdateInfo, flag, flag2);
					if (isAutoRun && DetectHelper.CheckNeedShowDialog(flag, flag2, serverUpdateInfo))
					{
						this.monitor.DownloadFinished += this.HandleDownloadFinished;
					}
					DownloadProcesser downloadProcesser = new DownloadProcesser();
					downloadProcesser.DownloadAsync(this.monitor);
				}
				if (!isAutoRun)
				{
					this.isChecking = false;
					this.ShowUpdateInfoWindow("");
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("检查自动更新失败", exception);
			}
			this.isChecking = false;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003910 File Offset: 0x00001B10
		private void HandleDownloadFinished(object sender, DownloadFinishedArgs e)
		{
			DownloadMonitor downloadMonitor = sender as DownloadMonitor;
			downloadMonitor.DownloadFinished -= this.HandleDownloadFinished;
			if (downloadMonitor.IsSuccessed && DetectHelper.CheckNeedShowDialog(downloadMonitor.NeedUpdateStudio, downloadMonitor.NeedUpdateRuntime, null))
			{
				this.ShowUpdateInfoWindow("");
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000039CC File Offset: 0x00001BCC
		private void ShowUpdateInfoWindow(string downloadLink = "")
		{
			GLib.Timeout.Add(0U, delegate
			{
				Window defaultModalParent = MessageService.GetDefaultModalParent();
				if (defaultModalParent != ApplicationCurrent.MainWindow)
				{
					return false;
				}
				if (!ApplicationCurrent.MainWindow.IsActive)
				{
					return false;
				}
				if (UpdateManager.UpdateWindow != null)
				{
					UpdateManager.UpdateWindow.Present();
				}
				else
				{
					UpdateManager.UpdateWindow = new UpdateWindow(this.monitor, downloadLink);
					UpdateManager.UpdateWindow.Show();
				}
				return false;
			});
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003A1C File Offset: 0x00001C1C
		private void ShowMessage(string info)
		{
			GLib.Timeout.Add(0U, delegate
			{
				MessageBox.Show(info, MessageBoxImage.Other, null, null);
				return false;
			});
		}

		// Token: 0x04000026 RID: 38
		private DownloadMonitor monitor;

		// Token: 0x04000027 RID: 39
		private bool isChecking;
	}
}
