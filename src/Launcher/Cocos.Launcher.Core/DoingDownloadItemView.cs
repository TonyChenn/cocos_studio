using System;
using System.ComponentModel;
using System.IO;
using Cocos.Launcher.Control;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Pango;
using Stetic;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class DoingDownloadItemView : Bin
	{
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.HeightRequest = 98;
			base.Name = "Cocos.Launcher.Core.DoingDownloadItemView";
			this.eventbox_bg = new EventBox();
			this.eventbox_bg.Name = "eventbox_bg";
			this.hbox = new HBox();
			this.hbox.Name = "hbox";
			this.hbox.Spacing = 10;
			this.alignment_image = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_image.Name = "alignment_image";
			this.image = new Gtk.Image();
			this.image.WidthRequest = 98;
			this.image.HeightRequest = 98;
			this.image.Name = "image";
			this.alignment_image.Add(this.image);
			this.hbox.Add(this.alignment_image);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox[this.alignment_image];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment_loading = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_loading.Name = "alignment_loading";
			this.vbox1 = new VBox();
			this.vbox1.Name = "vbox1";
			this.label_title = new Label();
			this.label_title.Name = "label_title";
			this.label_title.Xalign = 0f;
			this.vbox1.Add(this.label_title);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox1[this.label_title];
			boxChild2.Position = 0;
			this.alignment6 = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment6.Name = "alignment6";
			this.alignment6.TopPadding = 5U;
			this.alignment6.BottomPadding = 5U;
			this.progressbar3 = new ProgressBar();
			this.progressbar3.WidthRequest = 630;
			this.progressbar3.HeightRequest = 8;
			this.progressbar3.Name = "progressbar3";
			this.alignment6.Add(this.progressbar3);
			this.vbox1.Add(this.alignment6);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox1[this.alignment6];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.hbox2 = new HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			this.label_size = new Label();
			this.label_size.WidthRequest = 150;
			this.label_size.Name = "label_size";
			this.label_size.Xalign = 0f;
			this.label_size.LabelProp = Catalog.GetString("0MB");
			this.hbox2.Add(this.label_size);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox2[this.label_size];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.label_speed = new Label();
			this.label_speed.Name = "label_speed";
			this.label_speed.Xalign = 0f;
			this.label_speed.LabelProp = Catalog.GetString("0kb/s");
			this.hbox2.Add(this.label_speed);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox2[this.label_speed];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.label_time = new Label();
			this.label_time.Name = "label_time";
			this.label_time.LabelProp = Catalog.GetString("00:00:00");
			this.hbox2.Add(this.label_time);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox2[this.label_time];
			boxChild6.PackType = PackType.End;
			boxChild6.Position = 2;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.vbox1.Add(this.hbox2);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox1[this.hbox2];
			boxChild7.Position = 2;
			this.alignment_loading.Add(this.vbox1);
			this.hbox.Add(this.alignment_loading);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox[this.alignment_loading];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.alignment_button = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_button.Name = "alignment_button";
			this.alignment_button.LeftPadding = 5U;
			this.hbox1 = new HBox();
			this.hbox1.WidthRequest = 60;
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 10;
			this.alignment_button.Add(this.hbox1);
			this.hbox.Add(this.alignment_button);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox[this.alignment_button];
			boxChild9.Position = 2;
			this.eventbox_bg.Add(this.hbox);
			base.Add(this.eventbox_bg);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		public DoingDownloadItemView(AssetModel model)
		{
			this.Build();
			this.m_Model = model;
			this.InitWidget();
			this.InitEvent();
			this.InitView();
			base.ShowAll();
		}

		private void InitWidget()
		{
			try
			{
				if (File.Exists(this.m_Model.AssetInfo.ImagePath))
				{
					Pixbuf pixbuf = PixbufHelper.Load(this.m_Model.AssetInfo.ImagePath);
					this.image.Pixbuf = pixbuf.ScaleSimple(98, 98, InterpType.Bilinear);
				}
				else
				{
					this.image.Pixbuf = ImageIcon.GetPixbuf("Cocos.Launcher.Resource.LauncherResource.plugindefault.png");
					this.DownloadPic();
				}
			}
			catch (Exception)
			{
				this.image.Pixbuf = ImageIcon.GetPixbuf("Cocos.Launcher.Resource.LauncherResource.plugindefault.png");
				this.DownloadPic();
			}
			this.label_title.ModifyFg(StateType.Normal, ConstantConfig.Colors.ContentLabelColor1);
			this.label_title.SetFontSize(14.0);
			this.label_title.LabelProp = this.m_Model.AssetInfo.PluginName;
			this.label_title.WidthRequest = 300;
			this.label_title.Ellipsize = EllipsizeMode.End;
			this.label_size.ModifyFg(StateType.Normal, ConstantConfig.Colors.NewsInfoColor);
			this.label_size.SetFontSize(12.0);
			this.label_size.Text = "0MB/0MB";
			this.label_speed.ModifyFg(StateType.Normal, ConstantConfig.Colors.NewsInfoColor);
			this.label_speed.SetFontSize(12.0);
			this.label_speed.Text = "";
			this.label_time.ModifyFg(StateType.Normal, ConstantConfig.Colors.NewsInfoColor);
			this.label_time.SetFontSize(12.0);
			this.label_time.Text = "";
			this.alignment_button.TopPadding = 22U;
			this.alignment_button.BottomPadding = 22U;
			this.startCheckbox = new CheckboxView("Cocos.Launcher.Resource.LauncherResource.pause.png", "Cocos.Launcher.Resource.LauncherResource.start.png");
			this.startCheckbox.SetToolTips(LanguageInfo.UIAnimation_ToolTip_Pause, LanguageInfo.Launcher_Start);
			this.startCheckbox.Active = this.m_Model.AssetInfo.IsLoading;
			ImageButtonView imageButtonView = new ImageButtonView();
			imageButtonView.IsCursor = true;
			imageButtonView.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.delete.png");
			imageButtonView.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.delete_move.png");
			this.hbox1.PackStart(this.startCheckbox, false, false, 0U);
			this.hbox1.PackStart(imageButtonView, false, false, 0U);
			imageButtonView.ButtonReleaseEvent += this.deleteButton_ButtonReleaseEvent;
			this.progressbar3.Name = "ProgressbarBg_Plugin";
		}

		private void InitEvent()
		{
			this.startCheckbox.Clicked += this.startCheckbox_Clicked;
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
			this.m_Model.DownLoadProgressChanged += this.LoadingProgressChanged;
			this.m_Model.DeleteSelf += this.DeleteView;
			this.m_Model.DownloadSucceed += this.DownloadOverEvent;
			base.Destroyed += this.DoingDownloadView_Destroyed;
		}

		private void LoadingProgressChanged(object sender, Cocos.Launcher.Library.ProgressChangedEventArgs e)
		{
			this.UpdateProgress(e.Fraction, e.FileSize, e.DownloadSpeed, e.RemainTime);
		}

		private void DownloadOverEvent(object sender, DownloadSucceedEventArgs e)
		{
			this.Destroy();
		}

		private void DeleteView(object sender, EventArgs e)
		{
			this.Destroy();
		}

		private void InitView()
		{
			if (this.m_Model.AssetInfo.IsLoading)
			{
				this.UpdateProgress(this.m_Model.AssetInfo.PluginFraction, this.m_Model.AssetInfo.PluginSize, 0f, -1L);
				return;
			}
			this.UpdateProgress(this.m_Model.AssetInfo.PluginFraction, this.m_Model.AssetInfo.PluginSize, -1f, -1L);
		}

		private void DoingDownloadView_Destroyed(object sender, EventArgs e)
		{
			base.Destroyed -= this.DoingDownloadView_Destroyed;
			this.m_Model.DeleteSelf -= this.DeleteView;
			this.m_Model.DownloadSucceed -= this.DownloadOverEvent;
			this.m_Model.DownLoadProgressChanged -= this.LoadingProgressChanged;
			CocoStudio.Core.Services.NetworkService.NetworkChanged -= this.NetworkService_NetworkChanged;
		}

		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			if (!e.IsNetworkingSuccessed)
			{
				this.startCheckbox.Active = false;
			}
		}

		private void DownloadPic()
		{
			if (!string.IsNullOrEmpty(this.m_Model.AssetInfo.ImageUrl) && !string.IsNullOrEmpty(this.m_Model.AssetInfo.ImagePath))
			{
				if (!this.DeleteFile(this.m_Model.AssetInfo.ImagePath))
				{
					return;
				}
				HttpDownload httpDownload = new HttpDownload(this.m_Model.AssetInfo.ImageUrl, this.m_Model.AssetInfo.ImagePath);
				httpDownload.DownloadFinished += this.downloadpic_HttpDownLoadEndInfoEvent;
				httpDownload.StartDownloadAsync();
			}
		}

		private bool DeleteFile(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception arg)
			{
				LogConfig.Output.Error("文件删除失败" + arg);
				return false;
			}
			return true;
		}

		public void StartDownload()
		{
			this.startCheckbox.Active = true;
		}

		public void UpdateProgress(float fraction, float totalSize, float speed = 0f, long remainTime = -1L)
		{
			if (speed < 0f)
			{
				this.startCheckbox.Active = false;
			}
			float fraction2 = fraction / 100f;
			string loadSpeedToString = this.GetLoadSpeedToString(speed);
			string sizeStr = this.GetSizeStr(fraction, totalSize);
			string remainTimeStr = this.GetRemainTimeStr(remainTime);
			this.UpdateProgress(fraction2, sizeStr, loadSpeedToString, remainTimeStr);
		}

		private void UpdateProgress(float fraction, string downloadSize, string speed, string remainTime)
		{
			this.progressbar3.Fraction = (double)fraction;
			this.label_size.Text = downloadSize;
			this.label_speed.Text = speed;
			this.label_time.Text = remainTime;
		}

		private string GetLoadSpeedToString(float speed)
		{
			string result = string.Empty;
			if (speed < 0f)
			{
				result = LanguageInfo.Launcher_Paused;
			}
			else if (speed < 1024f)
			{
				result = string.Format("{0:0.00} KB/s", speed);
			}
			else
			{
				speed /= 1024f;
				result = string.Format("{0:0.00} MB/s", speed);
			}
			return result;
		}

		private string GetSizeStr(float Fraction, float size)
		{
			float num = Fraction * size / 100f;
			return string.Format("{0:0.00} MB/", num) + Math.Round((double)size, 2).ToString() + "MB";
		}

		private string GetRemainTimeStr(long remainTime)
		{
			string result = string.Empty;
			if (remainTime > -1L)
			{
				result = string.Format("{0:00}:{1:00}:{2:00}", remainTime / 3600L, remainTime % 3600L / 60L, remainTime % 60L);
			}
			return result;
		}

		private string GetShowString(string oldstr, int lenght = 10)
		{
			if (string.IsNullOrEmpty(oldstr))
			{
				return string.Empty;
			}
			if (oldstr.Length <= lenght)
			{
				return oldstr;
			}
			string str = oldstr.Substring(0, lenght);
			return str + "...";
		}

		private void downloadpic_HttpDownLoadEndInfoEvent(object sender, DownloadFinishedEventArgs e)
		{
			HttpDownload httpDownload = sender as HttpDownload;
			httpDownload.DownloadFinished -= this.downloadpic_HttpDownLoadEndInfoEvent;
			if (e.IsSuccessed && File.Exists(this.m_Model.AssetInfo.ImagePath))
			{
				try
				{
					Pixbuf pixbuf = PixbufHelper.Load(this.m_Model.AssetInfo.ImagePath);
					this.image.Pixbuf = pixbuf.ScaleSimple(98, 98, InterpType.Bilinear);
				}
				catch (Exception arg)
				{
					LogConfig.Output.Error("图片加载失败" + arg);
				}
				base.ShowAll();
				return;
			}
			if (!e.IsSuccessed)
			{
				this.DeleteFile(this.m_Model.AssetInfo.ImagePath);
			}
		}

		private void startCheckbox_Clicked(object sender, EventArgs e)
		{
			if (this.startCheckbox.Active)
			{
				this.m_Model.StartDownload();
				return;
			}
			this.m_Model.StopDownload();
		}

		private void deleteButton_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			bool active = this.startCheckbox.Active;
			this.startCheckbox.Active = false;
			MessageBoxResult messageBoxResult = MessageBox.Show(LanguageInfo.Launcher_ConfirmDelete, MessageBoxButton.YesNo, MessageBoxImage.Question, Services.MainWindow, EnumMainButton.Yes, null);
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				this.m_Model.DeleteItem();
				return;
			}
			this.startCheckbox.Active = active;
		}

		private EventBox eventbox_bg;

		private HBox hbox;

		private Gtk.Alignment alignment_image;

		private Gtk.Image image;

		private Gtk.Alignment alignment_loading;

		private VBox vbox1;

		private Label label_title;

		private Gtk.Alignment alignment6;

		private ProgressBar progressbar3;

		private HBox hbox2;

		private Label label_size;

		private Label label_speed;

		private Label label_time;

		private Gtk.Alignment alignment_button;

		private HBox hbox1;

		private CheckboxView startCheckbox;

		private AssetModel m_Model;
	}
}
