using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.MutualEditor;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.AutoUpdate
{
	public class UpdateWindow : Gtk.Window
	{
		public UpdateWindow() : base(Gtk.WindowType.Toplevel)
		{
			throw new Exception();
		}

		public UpdateWindow(DownloadMonitor downMonitor, string downloadLink = "") : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			base.Modal = true;
			this.SetToDialogStyle(null, true, true, true);
			this.monitor = downMonitor;
			if (this.monitor == null)
			{
				throw new Exception("Download monitor cannot be null");
			}
			this.InitEvent();
			this.InitWidget();
			this.InitText();
			this.InitStatus(downloadLink);
		}

		private void InitEvent()
		{
			base.DeleteEvent += this.WindowsFileProcess_DeleteEvent;
			this.buttonInstall.Clicked += this.OnButtonYesClicked;
		}

		private void InitWidget()
		{
			this.label_currentVersion = new Label(string.Format(LanguageInfo.AutoUpdate_CurrentVersion, "2.3.3.0"));
			this.label_currentVersion.SetFontSize(13.0);
			this.label_currentVersion.Xalign = 0f;
			this.label_failed = new Label(LanguageInfo.AutoUpdate_Failed);
			this.label_failed.ModifyFg(StateType.Normal, new Color(byte.MaxValue, 0, 0));
			this.label_failed.SetFontSize(13.0);
			this.label_failed.Xalign = 0f;
			this.progressBar = new ProgressBar();
			this.labelLink_download = new LabelLinkButton(LanguageInfo.AutoUpdate_VersionTooLow);
			this.labelLink_download.Label.Xalign = 0f;
			this.eventbox_darkBorder.ModifyBg(StateType.Normal, new Color(50, 50, 54));
			this.label_title.SetFontSize(14.0);
			this.textView_des.SetFontSize(14.0);
			this.textView_des.WrapMode = WrapMode.WordChar;
			this.buttonInstall.Name = "MainButton";
		}

		private void InitText()
		{
			base.Title = LanguageInfo.AutoUpdate_Title;
			this.buttonInstall.Label = LanguageInfo.AutoUpdate_Install;
			if (Platform.IsMac)
			{
				MacServerInfo macServerInfo = this.monitor.ServerInfo as MacServerInfo;
				if (this.monitor.NeedUpdateStudio)
				{
					this.label_title.Text = string.Format(LanguageInfo.AutoUpdate_NewVerFound, macServerInfo.DisplayVersion);
				}
				else
				{
					this.label_title.Text = string.Format(LanguageInfo.AutoUpdate_NewRuntime, macServerInfo.RuntimeName, macServerInfo.RuntimeVersion);
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (this.monitor.NeedUpdateStudio)
				{
					stringBuilder.AppendLine(macServerInfo.Desc);
					stringBuilder.AppendLine("\r\n");
				}
				if (this.monitor.NeedUpdateRuntime)
				{
					stringBuilder.AppendLine(macServerInfo.RuntimeDesc);
				}
				this.textView_des.Buffer.Text = stringBuilder.ToString();
				return;
			}
			WinServerInfo winServerInfo = this.monitor.ServerInfo as WinServerInfo;
			this.label_title.Text = string.Format(LanguageInfo.AutoUpdate_NewVerFound, winServerInfo.DisplayVersion);
			this.textView_des.Buffer.Text = winServerInfo.Desc;
		}

		private void InitStatus(string downloadLink)
		{
			if (!string.IsNullOrEmpty(downloadLink))
			{
				this.labelLink_download.URL = downloadLink;
				this.SwitchStatus(UpdateWindowStatus.LowVersion);
				return;
			}
			if (this.monitor.IsDownloading)
			{
				this.SwitchStatus(UpdateWindowStatus.Downloading);
				return;
			}
			if (this.monitor.IsSuccessed)
			{
				this.SwitchStatus(UpdateWindowStatus.Finished);
				return;
			}
			this.SwitchStatus(UpdateWindowStatus.Init);
		}

		private void SwitchStatus(UpdateWindowStatus status)
		{
			this.currentStatus = status;
			switch (status)
			{
			case UpdateWindowStatus.Init:
				this.ChangeBottomLeftWidget(this.label_currentVersion);
				this.buttonInstall.Label = LanguageInfo.AutoUpdate_Download;
				this.buttonInstall.Sensitive = true;
				return;
			case UpdateWindowStatus.Downloading:
				this.ChangeBottomLeftWidget(this.progressBar);
				this.buttonInstall.Label = LanguageInfo.AutoUpdate_Install;
				this.buttonInstall.Sensitive = false;
				GLib.Timeout.Add(50U, new TimeoutHandler(this.RefreshUI));
				return;
			case UpdateWindowStatus.Finished:
				this.progressBar.Fraction = 1.0;
				this.ChangeBottomLeftWidget(this.label_currentVersion);
				this.buttonInstall.Label = LanguageInfo.AutoUpdate_Install;
				this.buttonInstall.Sensitive = true;
				return;
			case UpdateWindowStatus.Failed:
				this.progressBar.Fraction = 0.0;
				this.ChangeBottomLeftWidget(this.label_failed);
				this.buttonInstall.Label = LanguageInfo.AutoUpdate_Download;
				this.buttonInstall.Sensitive = true;
				return;
			case UpdateWindowStatus.LowVersion:
				this.ChangeBottomLeftWidget(this.labelLink_download);
				this.buttonInstall.Label = LanguageInfo.AutoUpdate_Download;
				this.buttonInstall.Sensitive = false;
				return;
			default:
				return;
			}
		}

		private bool RefreshUI()
		{
			if (this.hasDisposed)
			{
				return false;
			}
			if (!this.monitor.HasStarted)
			{
				return true;
			}
			if (this.monitor.IsDownloading)
			{
				if (this.monitor.CurrentProgress == 0f)
				{
					this.timeoutCount++;
				}
				if (this.timeoutCount >= 200)
				{
					this.monitor.Finish(false, "donwload timeout");
					this.timeoutCount = 0;
					this.SwitchStatus(UpdateWindowStatus.Failed);
				}
				this.progressBar.Fraction = (double)this.monitor.CurrentProgress;
				return true;
			}
			if (this.monitor.IsSuccessed)
			{
				this.SwitchStatus(UpdateWindowStatus.Finished);
			}
			else
			{
				this.SwitchStatus(UpdateWindowStatus.Failed);
			}
			return false;
		}

		private void Install()
		{
			this.isInstallClose = true;
			System.Diagnostics.Process process = this.PreInstall();
			if (process == null)
			{
				LogConfig.Output.Error(LanguageInfo.Output_FailedToInstallUpdate);
				this.Close();
				return;
			}
			if (DetectHelper.CheckIsCocosRunning())
			{
				MessageBox.Show(LanguageInfo.MessageBox268_PleaseCloseCosos, MessageBoxImage.Other, null, null);
			}
			this.Close();
			if (Services.MainWindow.Quit())
			{
				try
				{
					process.Start();
					MutualCore.Instance.Dispose();
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("启动自动更新进程时出错", exception);
				}
			}
		}

		private System.Diagnostics.Process PreInstall()
		{
			string text;
			string text2;
			if (Platform.IsWindows)
			{
				if (!this.PreInstallWindows(out text, out text2))
				{
					return null;
				}
			}
			else if (!this.PreInstallMac(out text, out text2))
			{
				return null;
			}
			if (!string.IsNullOrEmpty(text))
			{
				Console.WriteLine("更新程序：" + text);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				Console.WriteLine("执行参数：" + text2);
			}
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.UseShellExecute = false;
			processStartInfo.FileName = text;
			processStartInfo.Arguments = text2;
			return new System.Diagnostics.Process
			{
				StartInfo = processStartInfo
			};
		}

		private bool PreInstallWindows(out string fileName, out string args)
		{
			string path = "Cocos.Update.exe";
			string sourceFileName = System.IO.Path.Combine(Option.AssemblyDir, path);
			string text = System.IO.Path.Combine(PathHelper.AutoUpdateTempPath, path);
			try
			{
				File.Copy(sourceFileName, text, true);
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error(LanguageInfo.Output_FailedToCopyToTemp, exception);
				string empty;
				args = (empty = string.Empty);
				fileName = empty;
				return false;
			}
			if (File.Exists(text))
			{
				string str;
				if (this.monitor.NeedUpdateRuntime)
				{
					str = this.monitor.StudioInstallFilePath;
				}
				else
				{
					str = this.monitor.WinSmallPackageFilePath;
				}
				fileName = text;
				args = "\"" + str + "\" \"#SKIP#\"";
				return true;
			}
			LogConfig.Output.Error(string.Format(LanguageInfo.Output_CantFindUpdateExe, text));
			string empty2;
			args = (empty2 = string.Empty);
			fileName = empty2;
			return false;
		}

		private bool PreInstallMac(out string fileName, out string args)
		{
			string text;
			if (this.monitor.NeedUpdateStudio)
			{
				text = this.monitor.StudioInstallFilePath;
			}
			else
			{
				text = "#SKIP#";
			}
			string text2;
			if (this.monitor.NeedUpdateRuntime)
			{
				text2 = this.monitor.MacRuntimeInstallFilePath;
			}
			else
			{
				text2 = "#SKIP#";
			}
			args = string.Concat(new string[]
			{
				"'",
				text,
				"' '",
				text2,
				"'"
			});
			fileName = System.IO.Path.Combine(Option.AssemblyDir, "Cocos.Update");
			return true;
		}

		private void RestartDownload()
		{
			string output;
			ServerUpdateInfo serverUpdateInfo = DetectHelper.GetServerUpdateInfo(out output);
			if (serverUpdateInfo == null)
			{
				GLib.Timeout.Add(0U, delegate
				{
					MessageBox.Show(output, MessageBoxImage.Info, this, null);
					return false;
				});
				return;
			}
			this.monitor.Reset(serverUpdateInfo);
			DownloadProcesser downloadProcesser = new DownloadProcesser();
			downloadProcesser.DownloadAsync(this.monitor);
			this.SwitchStatus(UpdateWindowStatus.Downloading);
		}

		private void Close()
		{
			if (this.isInstallClose)
			{
				DetectHelper.LocalConfig.IsSkipToday = false;
				DetectHelper.LocalConfig.IsNeverRemind = false;
				DetectHelper.LocalConfig.SaveToFile();
			}
			else
			{
				RemindDialog remindDialog = new RemindDialog(this);
				int num = remindDialog.Run();
				remindDialog.Destroy();
				if (num != -5)
				{
					return;
				}
				DetectHelper.LocalConfig.AppVersion = this.monitor.ServerInfo.AppVersion;
				DetectHelper.LocalConfig.RuntimeVersion = this.monitor.ServerInfo.RuntimeVersion;
				switch (remindDialog.RemindType)
				{
				case EnumRemindType.Always:
					DetectHelper.LocalConfig.IsSkipToday = false;
					DetectHelper.LocalConfig.IsNeverRemind = false;
					break;
				case EnumRemindType.Later:
					DetectHelper.LocalConfig.IsSkipToday = true;
					DetectHelper.LocalConfig.IsNeverRemind = false;
					break;
				case EnumRemindType.Skip:
					DetectHelper.LocalConfig.IsSkipToday = false;
					DetectHelper.LocalConfig.IsNeverRemind = true;
					break;
				}
				DetectHelper.LocalConfig.SaveToFile();
			}
			this.hasDisposed = true;
			UpdateManager.UpdateWindow = null;
			this.monitor = null;
			this.Destroy();
		}

		private void ChangeBottomLeftWidget(Widget newWidget)
		{
			if (this.alignment_bottomLeft.Child == newWidget)
			{
				return;
			}
			if (this.alignment_bottomLeft.Child != null)
			{
				this.alignment_bottomLeft.Remove(this.alignment_bottomLeft.Child);
			}
			this.alignment_bottomLeft.Add(newWidget);
			newWidget.Show();
		}

		private void OnButtonYesClicked(object sender, EventArgs e)
		{
			switch (this.currentStatus)
			{
			case UpdateWindowStatus.Init:
				if (this.monitor.IsSuccessed)
				{
					this.SwitchStatus(UpdateWindowStatus.Finished);
					this.Install();
					return;
				}
				if (this.monitor.HasStarted && !this.monitor.IsDownloading)
				{
					this.RestartDownload();
					return;
				}
				this.SwitchStatus(UpdateWindowStatus.Downloading);
				return;
			case UpdateWindowStatus.Downloading:
				break;
			case UpdateWindowStatus.Finished:
				this.Install();
				break;
			case UpdateWindowStatus.Failed:
				this.RestartDownload();
				return;
			default:
				return;
			}
		}

		private void WindowsFileProcess_DeleteEvent(object o, DeleteEventArgs args)
		{
			this.Close();
			args.RetVal = true;
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 480;
			base.HeightRequest = 560;
			base.Name = "Modules.Communal.AutoUpdate.UpdateWindow";
			base.Title = Catalog.GetString("UpdateWindow");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Resizable = false;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 4;
			this.vbox_main.BorderWidth = 15U;
			this.alignment_title = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_title.Name = "alignment_title";
			this.alignment_title.TopPadding = 5U;
			this.alignment_title.BottomPadding = 4U;
			this.label_title = new Label();
			this.label_title.Name = "label_title";
			this.label_title.Xalign = 0f;
			this.label_title.LabelProp = Catalog.GetString("Cocos Stuio 已推出新版本：X.X.X");
			this.alignment_title.Add(this.label_title);
			this.vbox_main.Add(this.alignment_title);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.alignment_title];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.eventbox_darkBorder = new EventBox();
			this.eventbox_darkBorder.Name = "eventbox_darkBorder";
			this.alignment_scrolledWindows = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_scrolledWindows.Name = "alignment_scrolledWindows";
			this.alignment_scrolledWindows.LeftPadding = 3U;
			this.alignment_scrolledWindows.TopPadding = 2U;
			this.alignment_scrolledWindows.RightPadding = 1U;
			this.alignment_scrolledWindows.BottomPadding = 2U;
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.textView_des = new TextView();
			this.textView_des.CanFocus = true;
			this.textView_des.Name = "textView_des";
			this.textView_des.Editable = false;
			this.GtkScrolledWindow.Add(this.textView_des);
			this.alignment_scrolledWindows.Add(this.GtkScrolledWindow);
			this.eventbox_darkBorder.Add(this.alignment_scrolledWindows);
			this.vbox_main.Add(this.eventbox_darkBorder);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_main[this.eventbox_darkBorder];
			boxChild2.Position = 1;
			this.alignment_bottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_bottom.Name = "alignment_bottom";
			this.alignment_bottom.TopPadding = 10U;
			this.hbox_bottom = new HBox();
			this.hbox_bottom.Name = "hbox_bottom";
			this.hbox_bottom.Spacing = 8;
			this.vbox_bottomLeft = new VBox();
			this.vbox_bottomLeft.Name = "vbox_bottomLeft";
			this.alignment_progressTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_progressTop.Name = "alignment_progressTop";
			this.vbox_bottomLeft.Add(this.alignment_progressTop);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_bottomLeft[this.alignment_progressTop];
			boxChild3.Position = 0;
			this.alignment_bottomLeft = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_bottomLeft.Name = "alignment_bottomLeft";
			this.vbox_bottomLeft.Add(this.alignment_bottomLeft);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_bottomLeft[this.alignment_bottomLeft];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			this.alignment_progressBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_progressBottom.Name = "alignment_progressBottom";
			this.vbox_bottomLeft.Add(this.alignment_progressBottom);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_bottomLeft[this.alignment_progressBottom];
			boxChild5.Position = 2;
			this.hbox_bottom.Add(this.vbox_bottomLeft);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_bottom[this.vbox_bottomLeft];
			boxChild6.Position = 0;
			this.alignment_btnInstall = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_btnInstall.Name = "alignment_btnInstall";
			this.alignment_btnInstall.LeftPadding = 10U;
			this.buttonInstall = new Button();
			this.buttonInstall.WidthRequest = 90;
			this.buttonInstall.HeightRequest = 26;
			this.buttonInstall.CanFocus = true;
			this.buttonInstall.Name = "buttonInstall";
			this.buttonInstall.UseUnderline = true;
			this.buttonInstall.Label = Catalog.GetString("立即更新");
			this.alignment_btnInstall.Add(this.buttonInstall);
			this.hbox_bottom.Add(this.alignment_btnInstall);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_bottom[this.alignment_btnInstall];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.alignment_bottom.Add(this.hbox_bottom);
			this.vbox_main.Add(this.alignment_bottom);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.alignment_bottom];
			boxChild8.Position = 2;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 480;
			base.DefaultHeight = 560;
			base.Hide();
		}

		private const string skipMarp = "#SKIP#";

		private const int maxTimeoutCount = 200;

		private bool hasDisposed;

		private bool isInstallClose;

		private DownloadMonitor monitor;

		private int timeoutCount;

		private UpdateWindowStatus currentStatus;

		private ProgressBar progressBar;

		private Label label_currentVersion;

		private Label label_failed;

		private LabelLinkButton labelLink_download;

		private VBox vbox_main;

		private Alignment alignment_title;

		private Label label_title;

		private EventBox eventbox_darkBorder;

		private Alignment alignment_scrolledWindows;

		private ScrolledWindow GtkScrolledWindow;

		private TextView textView_des;

		private Alignment alignment_bottom;

		private HBox hbox_bottom;

		private VBox vbox_bottomLeft;

		private Alignment alignment_progressTop;

		private Alignment alignment_bottomLeft;

		private Alignment alignment_progressBottom;

		private Alignment alignment_btnInstall;

		private Button buttonInstall;
	}
}
