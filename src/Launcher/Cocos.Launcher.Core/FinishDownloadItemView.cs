using System;
using System.ComponentModel;
using System.IO;
using Cocos.Launcher.Control;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Pango;
using Stetic;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class FinishDownloadItemView : Bin
	{
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.WidthRequest = 245;
			base.HeightRequest = 98;
			base.Name = "Cocos.Launcher.Core.FinishDownloadItemView";
			this.eventbox1 = new EventBox();
			this.eventbox1.Name = "eventbox1";
			this.hbox_all = new HBox();
			this.hbox_all.Name = "hbox_all";
			this.hbox_all.Spacing = 8;
			this.alignment_image = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_image.Name = "alignment_image";
			this.image2 = new Gtk.Image();
			this.image2.WidthRequest = 98;
			this.image2.HeightRequest = 98;
			this.image2.Name = "image2";
			this.alignment_image.Add(this.image2);
			this.hbox_all.Add(this.alignment_image);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_all[this.alignment_image];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment_info = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_info.Name = "alignment_info";
			this.vbox_info = new VBox();
			this.vbox_info.Name = "vbox_info";
			this.vbox_info.Spacing = 6;
			this.alignment_name = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_name.Name = "alignment_name";
			this.alignment_name.TopPadding = 3U;
			this.label_name = new Label();
			this.label_name.WidthRequest = 130;
			this.label_name.Name = "label_name";
			this.label_name.Xalign = 0f;
			this.label_name.LabelProp = Catalog.GetString("label5");
			this.alignment_name.Add(this.label_name);
			this.vbox_info.Add(this.alignment_name);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_info[this.alignment_name];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.alignment_star = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_star.HeightRequest = 14;
			this.alignment_star.Name = "alignment_star";
			this.hbox_star = new HBox();
			this.hbox_star.Name = "hbox_star";
			this.alignment_star.Add(this.hbox_star);
			this.vbox_info.Add(this.alignment_star);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_info[this.alignment_star];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			this.label_time = new Label();
			this.label_time.Name = "label_time";
			this.label_time.Xalign = 0f;
			this.label_time.LabelProp = Catalog.GetString("label7");
			this.vbox_info.Add(this.label_time);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_info[this.label_time];
			boxChild4.Position = 2;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.alignment_button = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_button.HeightRequest = 24;
			this.alignment_button.Name = "alignment_button";
			this.hbox_button = new HBox();
			this.hbox_button.Name = "hbox_button";
			this.hbox_button.Spacing = 14;
			this.alignment_button.Add(this.hbox_button);
			this.vbox_info.Add(this.alignment_button);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_info[this.alignment_button];
			boxChild5.Position = 3;
			this.alignment_info.Add(this.vbox_info);
			this.hbox_all.Add(this.alignment_info);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_all[this.alignment_info];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.eventbox1.Add(this.hbox_all);
			base.Add(this.eventbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		public RunModeEnum RunMode
		{
			get
			{
				return this.runMode;
			}
			set
			{
				this.runMode = value;
				string text = LanguageInfo.Launcher_Open;
				switch (value)
				{
				case RunModeEnum.Install:
					text = LanguageInfo.AutoUpdate_Install;
					break;
				case RunModeEnum.Open:
					text = LanguageInfo.Launcher_Open;
					break;
				case RunModeEnum.Gain:
					text = LanguageInfo.Launcher_Get;
					break;
				case RunModeEnum.Update:
					text = LanguageInfo.Launcher_Update;
					break;
				}
				this.UpdateInstallButtonText(text);
			}
		}

		public UninstallModeEnum UninstallMode
		{
			get
			{
				return this.uninstallMode;
			}
			set
			{
				this.uninstallMode = value;
				string text = LanguageInfo.Command_Delete;
				switch (value)
				{
				case UninstallModeEnum.Uninstall:
					text = LanguageInfo.Launcher_Uninstall;
					if (!this.assetModel.AssetInfo.IsUninstall)
					{
						this.hbox_button.Remove(this.uninstallButton);
					}
					break;
				case UninstallModeEnum.Delete:
					text = LanguageInfo.Command_Delete;
					break;
				}
				this.UpdateUninstallButtonText(text);
			}
		}

		public FinishDownloadItemView(AssetModel model)
		{
			this.Build();
			this.assetModel = model;
			this.InitWidget();
			this.InitEvent();
			this.UpdateButtonShow();
			base.ShowAll();
		}

		private void InitWidget()
		{
			if (!string.IsNullOrEmpty(this.assetModel.AssetInfo.ImagePath) && File.Exists(this.assetModel.AssetInfo.ImagePath))
			{
				try
				{
					Pixbuf pixbuf = PixbufHelper.Load(this.assetModel.AssetInfo.ImagePath);
					this.image2.Pixbuf = pixbuf.ScaleSimple(98, 98, InterpType.Bilinear);
					goto IL_89;
				}
				catch (Exception)
				{
					this.image2.Pixbuf = ImageIcon.GetPixbuf("Cocos.Launcher.Resource.LauncherResource.plugindefault.png");
					goto IL_89;
				}
			}
			this.image2.Pixbuf = ImageIcon.GetPixbuf("Cocos.Launcher.Resource.LauncherResource.plugindefault.png");
			IL_89:
			if (Platform.IsMac)
			{
				this.vbox_info.Spacing = 8;
			}
			string text = this.assetModel.AssetInfo.PluginName;
			if (this.assetModel.AssetInfo.PluginName.Length > 40)
			{
				text = text.Substring(0, 40);
			}
			this.label_name.TooltipText = text;
			this.image2.TooltipText = text;
			this.InitButton();
			this.label_name.ModifyFg(StateType.Normal, ConstantConfig.Colors.ContentLabelColor1);
			this.label_name.SetFontSize(14.0);
			this.label_name.Ellipsize = EllipsizeMode.End;
			this.label_time.ModifyFg(StateType.Normal, ConstantConfig.Colors.NewsInfoColor);
			this.label_time.SetFontSize(12.0);
			this.assetModel.AssetInfo.PluginFraction = 100f;
			this.label_name.LabelProp = this.assetModel.AssetInfo.PluginName;
			if (File.Exists(this.assetModel.AssetInfo.PluginPath))
			{
				FileInfo fileInfo = new FileInfo(this.assetModel.AssetInfo.PluginPath);
				if (this.assetModel.AssetInfo.PluginSize == 0f)
				{
					this.assetModel.AssetInfo.PluginSize = (float)fileInfo.Length;
				}
				this.label_time.Text = fileInfo.LastWriteTime.ToString("yyyy-MM-dd") + this.GetSizeToView(this.assetModel.AssetInfo.PluginSize);
			}
			else
			{
				this.label_time.Text = this.GetSizeToView(this.assetModel.AssetInfo.PluginSize);
			}
			this.InitStar(this.assetModel.AssetInfo.Score);
		}

		private void InitEvent()
		{
			this.assetModel.DeleteSelf += this.View_DeleteSelf;
			this.assetModel.DownloadSelf += this.View_DownloadSelf;
			this.assetModel.RefreshRunModeEvent += this.RefreshRunMode;
			this.assetModel.RefreshUninstallModeEvent += this.RefreshUninstallMode;
			base.DestroyEvent += this.View_DestroyEvent;
		}

		private void UpdateButtonShow()
		{
			this.RunMode = this.assetModel.RunMode;
			this.UninstallMode = this.assetModel.UninstallMode;
		}

		private void RefreshUninstallMode(object sender, RefreshUninstallModeEventArgs e)
		{
			this.UninstallMode = e.UninstallMode;
		}

		private void RefreshRunMode(object sender, RefreshRunModeEventArgs e)
		{
			this.RunMode = e.RunMode;
		}

		private void View_DestroyEvent(object o, DestroyEventArgs args)
		{
			this.assetModel.DeleteSelf -= this.View_DeleteSelf;
			this.assetModel.DownloadSelf -= this.View_DownloadSelf;
			this.assetModel.RefreshRunModeEvent -= this.RefreshRunMode;
			this.assetModel.RefreshUninstallModeEvent -= this.RefreshUninstallMode;
		}

		private void View_DownloadSelf(object sender, PluginDownloadEventArgs e)
		{
			this.Destroy();
		}

		private void View_DeleteSelf(object sender, EventArgs e)
		{
			this.Destroy();
		}

		private void InitButton()
		{
			this.installButton = this.InitIntallButton();
			this.uninstallButton = this.InitUnintallButton();
			this.hbox_button.PackStart(this.installButton, false, false, 0U);
			if (this.assetModel.AssetInfo.OpenType != OperationType.exe.ToString() || !this.assetModel.AssetInfo.IsInstalled || this.assetModel.AssetInfo.IsUninstall)
			{
				this.hbox_button.PackEnd(this.uninstallButton, false, false, 0U);
			}
		}

		private ButtonView InitUnintallButton()
		{
			ButtonView buttonView = new ButtonView();
			buttonView.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.uninstall.png");
			buttonView.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.uninstallhover.png");
			buttonView.SetPressBack("Cocos.Launcher.Resource.LauncherResource.uninstallhover.png");
			buttonView.SetLableNormalColor(ConstantConfig.Colors.TabFontNormalColor);
			buttonView.SetSize(60, 24);
			buttonView.ButtonReleaseEvent += this.uninstallButton_ButtonReleaseEvent;
			buttonView.SetFontSize(12.0);
			return buttonView;
		}

		private ButtonView InitIntallButton()
		{
			ButtonView buttonView = new ButtonView();
			buttonView.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.smallbuttonBg.png");
			buttonView.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.smallbuttonBghover.png");
			buttonView.SetPressBack("Cocos.Launcher.Resource.LauncherResource.smallbuttonBghover.png");
			buttonView.SetLableNormalColor(ConstantConfig.Colors.MainLeftColor);
			buttonView.SetSize(60, 24);
			buttonView.ButtonReleaseEvent += this.InstallPlugin;
			return buttonView;
		}

		private void InitStar(double score)
		{
			if (score > 5.0)
			{
				score = 5.0;
			}
			else if (score < 0.0)
			{
				score = 0.0;
			}
			int num = (int)Math.Truncate(score);
			double num2 = score - (double)num;
			for (int i = 1; i <= 5; i++)
			{
				ImageBin imageBin = new ImageBin();
				imageBin.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.emptyStar.png"));
				if (i <= num)
				{
					imageBin.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.allstar.png"));
				}
				else if (i == num + 1)
				{
					if (num2 >= 0.3 && num2 <= 0.7)
					{
						imageBin.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.halfstar.png"));
					}
					else if (num2 > 0.7)
					{
						imageBin.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.allstar.png"));
					}
				}
				this.hbox_star.PackStart(imageBin, false, false, 0U);
			}
		}

		private string GetSizeToView(float originSize)
		{
			string result;
			if (originSize > 1000f)
			{
				float num = originSize / 1024f;
				result = " (" + Math.Round((double)num, 2).ToString() + "GB)";
			}
			else if (originSize < 1f)
			{
				float num = originSize * 1024f;
				result = " (" + Math.Round((double)num, 1).ToString() + "KB)";
			}
			else
			{
				result = " (" + Math.Round((double)originSize, 1).ToString() + "MB)";
			}
			return result;
		}

		public void UpdateInstallButtonText(string text)
		{
			this.installButton.SetLabelText(text);
		}

		public void UpdateUninstallButtonText(string text)
		{
			this.uninstallButton.SetLabelText(text);
		}

		public void InstallPlugin(object sender, ButtonReleaseEventArgs args)
		{
			switch (this.RunMode)
			{
			case RunModeEnum.Install:
				this.assetModel.Install(false);
				return;
			case RunModeEnum.Open:
				this.assetModel.Open();
				return;
			case RunModeEnum.Gain:
				this.assetModel.Gain();
				return;
			case RunModeEnum.Update:
				this.assetModel.Update(null);
				return;
			default:
				return;
			}
		}

		private void uninstallButton_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			switch (this.UninstallMode)
			{
			case UninstallModeEnum.Uninstall:
				this.assetModel.Uninstall();
				return;
			case UninstallModeEnum.Delete:
				this.assetModel.Delete();
				return;
			default:
				return;
			}
		}

		private EventBox eventbox1;

		private HBox hbox_all;

		private Gtk.Alignment alignment_image;

		private Gtk.Image image2;

		private Gtk.Alignment alignment_info;

		private VBox vbox_info;

		private Gtk.Alignment alignment_name;

		private Label label_name;

		private Gtk.Alignment alignment_star;

		private HBox hbox_star;

		private Label label_time;

		private Gtk.Alignment alignment_button;

		private HBox hbox_button;

		private ButtonView installButton;

		private ButtonView uninstallButton;

		private AssetModel assetModel;

		private RunModeEnum runMode;

		private UninstallModeEnum uninstallMode;
	}
}
