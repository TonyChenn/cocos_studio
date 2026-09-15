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
	[Extension(Type = typeof(ICommandHandle))]
	public class UpdateManager : ICommandHandle
	{
		public static UpdateWindow UpdateWindow { get; set; }

		void ICommandHandle.Initialize()
		{
			GlobalCommand.CheckUpdateCmd.Execute += this.CheckUpdateCmd_Execute;
			GlobalCommand.CheckUpdateCmd.Update += this.CheckUpdateCmd_CanExecute;
			if (Services.MainWindow != null)
			{
				Services.MainWindow.InitializeCompleted += this.OnMainWindowInitialized;
			}
		}

		private void OnMainWindowInitialized(object sender, EventArgs e)
		{
			Services.MainWindow.InitializeCompleted -= this.OnMainWindowInitialized;
			GLib.Timeout.Add(3000U, delegate
			{
				GlobalCommand.CheckUpdateCmd.RaiseExecute(true);
				return false;
			});
		}

		private void CheckUpdateCmd_CanExecute(object sender, CommandUpdateArgs args)
		{
			args.Info.Enabled = !this.isChecking;
		}

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

		private void HandleDownloadFinished(object sender, DownloadFinishedArgs e)
		{
			DownloadMonitor downloadMonitor = sender as DownloadMonitor;
			downloadMonitor.DownloadFinished -= this.HandleDownloadFinished;
			if (downloadMonitor.IsSuccessed && DetectHelper.CheckNeedShowDialog(downloadMonitor.NeedUpdateStudio, downloadMonitor.NeedUpdateRuntime, null))
			{
				this.ShowUpdateInfoWindow("");
			}
		}

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

		private void ShowMessage(string info)
		{
			GLib.Timeout.Add(0U, delegate
			{
				MessageBox.Show(info, MessageBoxImage.Other, null, null);
				return false;
			});
		}

		private DownloadMonitor monitor;

		private bool isChecking;
	}
}
