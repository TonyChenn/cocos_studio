using System;
using System.ComponentModel;
using System.Diagnostics;
using CocoStudio.Basic;
using Gdk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;
using Xwt.Drawing;

namespace Gtk
{
	[ToolboxItem(true)]
	public class AboutContentWidget : Bin
	{
		public AboutContentWidget()
		{
			this.Build();
			this.InitButtons();
			this.InitStyle();
		}

		private void InitStyle()
		{
			this.label_version.Text = string.Format(LanguageInfo.Dialog_About_Version, "2.3.3.0");
			this.label_authority.Text = LanguageInfo.Helper_Authorize + " Beijing Chukong Aipu Technology Co., Ltd";
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				Xwt.Drawing.Image icon = ImageIcon.GetIcon("Gtk.Resource.About.cocos_logo.png");
				this.imagebin_logo.SetImageView(icon);
				this.label_version.SetFontSize(13.0);
				this.label_version.SetFontSize(13.0);
				this.label_copyRight.SetFontSize(13.0);
				this.label_authority.SetFontSize(13.0);
			}
			else
			{
				Xwt.Drawing.Image icon = ImageIcon.GetIcon("Gtk.Resource.About.CocosStudio_logo.png");
				this.imagebin_logo.SetImageView(icon);
				this.vbox_text.Spacing = 6;
				this.alignment_text.TopPadding = 5U;
				this.alignment_image.HeightRequest = 130;
				this.alignment_image.RightPadding = 10U;
				this.alignment_main.TopPadding = 10U;
				this.alignment_main.BottomPadding = 10U;
			}
		}

		private void InitButtons()
		{
			IDialogButton dialogButton = this.CreateButton(LanguageInfo.Dialog_About_Web);
			dialogButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.HandleBtnOfficialClicked);
			this.alignment_btnOfficial.Add(dialogButton.GetWidget());
			IDialogButton dialogButton2 = this.CreateButton(LanguageInfo.Dialog_About_Weibo);
			dialogButton2.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.HandleBtnWeiboClicked);
			this.alignment_btnWeibo.Add(dialogButton2.GetWidget());
			IDialogButton dialogButton3 = this.CreateButton(LanguageInfo.Dialog_About_Forum);
			dialogButton3.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.HandleBtnForumClicked);
			this.alignment_btnForum.Add(dialogButton3.GetWidget());
			this.hbox_button.ShowAll();
		}

		private IDialogButton CreateButton(string text)
		{
			IDialogButton dialogButton;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				GeneralLauncherButton generalLauncherButton = new GeneralLauncherButton();
				generalLauncherButton.SetButtonStyle(true);
				dialogButton = generalLauncherButton;
			}
			else
			{
				dialogButton = new CsMessageBoxButton();
				dialogButton.GetWidget().Name = "MainButton";
			}
			dialogButton.Text = text;
			return dialogButton;
		}

		private void OpenWeb(string link)
		{
			try
			{
				Uri uri = new Uri(link, UriKind.RelativeOrAbsolute);
				Process.Start(uri.ToString());
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("打开网页{0}时出错", link), exception);
			}
		}

		private void HandleBtnOfficialClicked(object sender, EventArgs e)
		{
			this.OpenWeb("http://www.cocos.com/");
		}

		private void HandleBtnWeiboClicked(object sender, EventArgs e)
		{
			this.OpenWeb("http://weibo.com/cocos2dx");
		}

		private void HandleBtnForumClicked(object sender, EventArgs e)
		{
			this.OpenWeb("http://www.cocoachina.com/bbs/thread.php?fid=48");
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Gtk.AboutContentWidget";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.BorderWidth = 35U;
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.hbox_main.Spacing = 20;
			this.alignment_image = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_image.WidthRequest = 150;
			this.alignment_image.HeightRequest = 170;
			this.alignment_image.Name = "alignment_image";
			this.vbox_image = new VBox();
			this.vbox_image.Name = "vbox_image";
			this.alignment_imageTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageTop.Name = "alignment_imageTop";
			this.vbox_image.Add(this.alignment_imageTop);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_image[this.alignment_imageTop];
			boxChild.Position = 0;
			this.hbox_image = new HBox();
			this.hbox_image.Name = "hbox_image";
			this.alignment_imageLeft = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageLeft.Name = "alignment_imageLeft";
			this.hbox_image.Add(this.alignment_imageLeft);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_image[this.alignment_imageLeft];
			boxChild2.Position = 0;
			this.imagebin_logo = new ImageBin();
			this.imagebin_logo.Events = EventMask.ButtonPressMask;
			this.imagebin_logo.Name = "imagebin_logo";
			this.hbox_image.Add(this.imagebin_logo);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_image[this.imagebin_logo];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.alignment_imageRight = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageRight.Name = "alignment_imageRight";
			this.hbox_image.Add(this.alignment_imageRight);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_image[this.alignment_imageRight];
			boxChild4.Position = 2;
			this.vbox_image.Add(this.hbox_image);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_image[this.hbox_image];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.alignment_imageBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageBottom.Name = "alignment_imageBottom";
			this.vbox_image.Add(this.alignment_imageBottom);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_image[this.alignment_imageBottom];
			boxChild6.Position = 2;
			this.alignment_image.Add(this.vbox_image);
			this.hbox_main.Add(this.alignment_image);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_main[this.alignment_image];
			boxChild7.Position = 0;
			this.vbox_right = new VBox();
			this.vbox_right.Name = "vbox_right";
			this.vbox_right.Spacing = 10;
			this.alignment_text = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_text.Name = "alignment_text";
			this.alignment_text.TopPadding = 20U;
			this.vbox_text = new VBox();
			this.vbox_text.Name = "vbox_text";
			this.vbox_text.Spacing = 10;
			this.label_version = new Label();
			this.label_version.Name = "label_version";
			this.label_version.Xalign = 0f;
			this.label_version.LabelProp = Catalog.GetString("版本：v2.2");
			this.vbox_text.Add(this.label_version);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_text[this.label_version];
			boxChild8.Position = 0;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.label_copyRight = new Label();
			this.label_copyRight.Name = "label_copyRight";
			this.label_copyRight.Xalign = 0f;
			this.label_copyRight.LabelProp = Catalog.GetString("Copyright © Chukong Aipu 2015");
			this.vbox_text.Add(this.label_copyRight);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_text[this.label_copyRight];
			boxChild9.Position = 1;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.label_authority = new Label();
			this.label_authority.Name = "label_authority";
			this.label_authority.Xalign = 0f;
			this.label_authority.LabelProp = Catalog.GetString("授权 Beijing Chukong Aipu Technology Co., Ltd");
			this.vbox_text.Add(this.label_authority);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox_text[this.label_authority];
			boxChild10.Position = 2;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.alignment_text.Add(this.vbox_text);
			this.vbox_right.Add(this.alignment_text);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_right[this.alignment_text];
			boxChild11.Position = 0;
			this.alignment_button = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_button.Name = "alignment_button";
			this.hbox_button = new HBox();
			this.hbox_button.Name = "hbox_button";
			this.hbox_button.Spacing = 15;
			this.alignment_btnOfficial = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_btnOfficial.WidthRequest = 80;
			this.alignment_btnOfficial.HeightRequest = 26;
			this.alignment_btnOfficial.Name = "alignment_btnOfficial";
			this.hbox_button.Add(this.alignment_btnOfficial);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.hbox_button[this.alignment_btnOfficial];
			boxChild12.Position = 0;
			boxChild12.Expand = false;
			this.alignment_btnWeibo = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_btnWeibo.WidthRequest = 80;
			this.alignment_btnWeibo.HeightRequest = 26;
			this.alignment_btnWeibo.Name = "alignment_btnWeibo";
			this.hbox_button.Add(this.alignment_btnWeibo);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.hbox_button[this.alignment_btnWeibo];
			boxChild13.Position = 1;
			boxChild13.Expand = false;
			this.alignment_btnForum = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_btnForum.WidthRequest = 80;
			this.alignment_btnForum.HeightRequest = 26;
			this.alignment_btnForum.Name = "alignment_btnForum";
			this.hbox_button.Add(this.alignment_btnForum);
			Box.BoxChild boxChild14 = (Box.BoxChild)this.hbox_button[this.alignment_btnForum];
			boxChild14.Position = 2;
			boxChild14.Expand = false;
			this.alignment_button.Add(this.hbox_button);
			this.vbox_right.Add(this.alignment_button);
			Box.BoxChild boxChild15 = (Box.BoxChild)this.vbox_right[this.alignment_button];
			boxChild15.Position = 1;
			boxChild15.Expand = false;
			this.hbox_main.Add(this.vbox_right);
			Box.BoxChild boxChild16 = (Box.BoxChild)this.hbox_main[this.vbox_right];
			boxChild16.Position = 1;
			boxChild16.Expand = false;
			this.alignment_main.Add(this.hbox_main);
			base.Add(this.alignment_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private const string officialLink = "http://www.cocos.com/";

		private const string weiboLink = "http://weibo.com/cocos2dx";

		private const string forumLink = "http://www.cocoachina.com/bbs/thread.php?fid=48";

		private Alignment alignment_main;

		private HBox hbox_main;

		private Alignment alignment_image;

		private VBox vbox_image;

		private Alignment alignment_imageTop;

		private HBox hbox_image;

		private Alignment alignment_imageLeft;

		private ImageBin imagebin_logo;

		private Alignment alignment_imageRight;

		private Alignment alignment_imageBottom;

		private VBox vbox_right;

		private Alignment alignment_text;

		private VBox vbox_text;

		private Label label_version;

		private Label label_copyRight;

		private Label label_authority;

		private Alignment alignment_button;

		private HBox hbox_button;

		private Alignment alignment_btnOfficial;

		private Alignment alignment_btnWeibo;

		private Alignment alignment_btnForum;
	}
}
