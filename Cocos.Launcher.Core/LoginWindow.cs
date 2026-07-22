using System;
using AppKit;
using Cocos.Launcher.Control;
using CocoStudio.Basic;
using Gtk;
using Modules.Communal.CocoaChina;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200005E RID: 94
	public class LoginWindow : Window
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000339 RID: 825 RVA: 0x0000D560 File Offset: 0x0000B760
		// (remove) Token: 0x0600033A RID: 826 RVA: 0x0000D598 File Offset: 0x0000B798
		public event EventHandler<EventArgs> LoginCompleted;

		// Token: 0x0600033B RID: 827 RVA: 0x0000D5D0 File Offset: 0x0000B7D0
		public LoginWindow(LoginService loginService) : base(WindowType.Toplevel)
		{
			this.Build();
			this.loginService = loginService;
			base.TransientFor = Services.MainWindow;
			this.CenterToParentWindow(Services.MainWindow);
			this.InitValue();
			this.InitStyle();
			this.InitEvent();
			this.button.GrabDefault();
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000D624 File Offset: 0x0000B824
		private void InitValue()
		{
			this.launcherlink_forgetPassword = new LinkView();
			if (LanguageOption.CurrentLanguage == LanguageType.Chinese || LanguageOption.CurrentLanguage == LanguageType.Traditional)
			{
				base.SetSizeRequest(230, 230);
				this.hbox1.PackStart(this.launcherlink_forgetPassword, false, false, 0U);
				((Box.BoxChild)this.hbox1[this.launcherlink_forgetPassword]).Position = 2;
			}
			else
			{
				base.SetSizeRequest(230, 252);
				this.vbox4.PackStart(this.launcherlink_forgetPassword, false, false, 0U);
				((Box.BoxChild)this.vbox4[this.launcherlink_forgetPassword]).Position = 2;
			}
			this.checkbutton_AutoLogin.Label = LanguageInfo.Launcher_RememberMe;
			this.label_hint.Text = LanguageInfo.Launcher_LogintoCocos;
			this.entry_username.Name = "WhiteEntry";
			this.entry_password.Name = "WhiteEntry";
			this.entry_username.PlaceholderText = LanguageInfo.Launcher_UserAndEmail;
			this.entry_password.PlaceholderText = LanguageInfo.Launcher_Password;
			this.entry_password.IsPassword = true;
			this.launcherlink_forgetPassword.SetLableText(LanguageInfo.Launcher_ForgotPassword);
			this.launcherlink_forgetPassword.SetTag(ConstantConfig.Constant.ForgotPasswordUri);
			this.lable_login = new Label();
			this.lable_login.SetFontSize(16.0);
			this.lable_login.ModifyFg(StateType.Normal, ConstantConfig.Colors.MainContentColor);
			this.lable_login.Text = LanguageInfo.Launcher_Login;
			this.lable_login.SetSizeRequest(190, 35);
			this.fixed5.Add(this.lable_login);
			this.button = new Button();
			this.button.CanDefault = true;
			this.button.CanFocus = true;
			this.button.Clicked += this.button_Clicked;
			this.button.SetSizeRequest(0, 0);
			this.button.GrabDefault();
			this.fixed5.Add(this.button);
			if (!string.IsNullOrWhiteSpace(this.loginService.LoginInfo.UserName))
			{
				this.entry_username.Text = this.loginService.LoginInfo.UserName;
				this.entry_username.IsPlaceholder = false;
				if (this.loginService.LoginInfo.IsAutoLogin)
				{
					this.entry_password.Text = this.loginService.LoginInfo.UserPassword;
					this.entry_password.IsPlaceholder = false;
				}
				else
				{
					this.entry_password.Text = LanguageInfo.Launcher_Password;
					this.entry_password.IsPlaceholder = true;
				}
			}
			else
			{
				this.entry_username.IsPlaceholder = true;
				this.entry_password.IsPlaceholder = true;
			}
			this.checkbutton_AutoLogin.Active = this.loginService.LoginInfo.IsAutoLogin;
			this.label_error.Text = string.Empty;
			this.button.GrabFocus();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000D914 File Offset: 0x0000BB14
		private void InitEvent()
		{
			base.FocusOutEvent += this.LoginWindow_FocusOutEvent;
			this.launcherlink_forgetPassword.LinkClicked += WebHelper.OnOpenWeb;
			this.eventbox_login.EnterNotifyEvent += this.eventbox_login_EnterNotifyEvent;
			this.eventbox_login.LeaveNotifyEvent += this.eventbox_login_LeaveNotifyEvent;
			this.eventbox_login.ButtonReleaseEvent += this.eventbox_login_ButtonReleaseEvent;
			this.entry_username.Changed += this.entry_username_Changed;
			this.entry_password.Changed += this.entry_username_Changed;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000D9C0 File Offset: 0x0000BBC0
		private void InitStyle()
		{
			if (Platform.IsWindows)
			{
				if (LanguageOption.CurrentLanguage == LanguageType.Chinese || LanguageOption.CurrentLanguage == LanguageType.Traditional)
				{
					this.image_back.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.loginBackground1.png"));
				}
				else
				{
					this.image_back.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.loginBackground2.png"));
				}
			}
			this.image_userName.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.username.png"));
			this.image_password.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.password.png"));
			this.image_login.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.login1.png"));
			this.label_error.ModifyFg(StateType.Normal, ConstantConfig.Colors.ErrorColor);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000DA68 File Offset: 0x0000BC68
		public void RemoveWindowBorder()
		{
			if (Platform.IsWindows)
			{
				base.Decorated = false;
				return;
			}
			NSWindowStyle style = NSWindowStyle.Titled | NSWindowStyle.DocModal;
			NativeGdkMac.SetNSWindowStyle(base.GdkWindow, style);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000DA94 File Offset: 0x0000BC94
		private void SaveInfo()
		{
			this.loginService.LoginInfo.IsAutoLogin = this.checkbutton_AutoLogin.Active;
			this.loginService.LoginInfo.UserName = this.entry_username.Text;
			this.loginService.LoginInfo.UserPassword = this.entry_password.Text;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		private void Login()
		{
			this.button.GrabFocus();
			string text = string.Empty;
			if (this.entry_username.IsPlaceholder)
			{
				text = LanguageInfo.Launcher_EmptyUserName;
			}
			else if (this.entry_password.IsPlaceholder)
			{
				text = LanguageInfo.Launcher_EmptyPassword;
			}
			if (text != string.Empty)
			{
				this.label_error.Text = text;
				return;
			}
			string name = this.entry_username.Text.Trim();
			string text2 = this.entry_password.Text;
			Login login = new Login();
			login.OnResived += this.login_OnResived;
			login.SyncLoginCocos(name, text2);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000DB91 File Offset: 0x0000BD91
		private void LoginWindow_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.Destroy();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000DB99 File Offset: 0x0000BD99
		private void eventbox_login_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			this.image_login.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.login1.png"));
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000DBB0 File Offset: 0x0000BDB0
		private void eventbox_login_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			this.image_login.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.login2.png"));
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000DBC7 File Offset: 0x0000BDC7
		private void entry_username_Changed(object sender, EventArgs e)
		{
			this.label_error.Text = string.Empty;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000DBD9 File Offset: 0x0000BDD9
		private void eventbox_login_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button != 1U)
			{
				return;
			}
			this.Login();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000DBF0 File Offset: 0x0000BDF0
		private void login_OnResived(object sender, CocoaUserArgs e)
		{
			try
			{
				Login login = sender as Login;
				if (login != null)
				{
					login.OnResived -= this.login_OnResived;
				}
				if (string.IsNullOrWhiteSpace(e.User.ErrorMsg))
				{
					this.loginService.LoginInfo.Access_token = e.User.Access_token;
					this.loginService.LoginInfo.Refresh_token = e.User.Refresh_token;
					this.SaveInfo();
					this.loginService.LoginInfo.Save();
					if (this.LoginCompleted != null)
					{
						this.LoginCompleted(this, e);
					}
					base.FocusOutEvent -= this.LoginWindow_FocusOutEvent;
					this.Destroy();
				}
				else if (LanguageOption.CurrentLanguage == LanguageType.Chinese)
				{
					this.label_error.Text = e.User.ErrorMsg;
				}
				else
				{
					this.label_error.Text = LanguageInfo.Launcher_FailedToLogin;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("登录失败", exception);
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000DD00 File Offset: 0x0000BF00
		private void button_Clicked(object sender, EventArgs e)
		{
			this.Login();
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000DD08 File Offset: 0x0000BF08
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Cocos.Launcher.Core.LoginWindow";
			base.Title = Catalog.GetString("LoginWindow");
			base.WindowPosition = WindowPosition.CenterOnParent;
			this.eventbox1 = new EventBox();
			this.eventbox1.Name = "eventbox1";
			this.fixed2 = new Fixed();
			this.fixed2.Name = "fixed2";
			this.fixed2.HasWindow = false;
			this.image_back = new ImageBin();
			this.image_back.Name = "image_back";
			this.fixed2.Add(this.image_back);
			this.alignment1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.alignment1.LeftPadding = 20U;
			this.alignment1.TopPadding = 20U;
			this.vbox2 = new VBox();
			this.vbox2.WidthRequest = 195;
			this.vbox2.HeightRequest = 0;
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 12;
			this.vbox5 = new VBox();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 3;
			this.vbox3 = new VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			this.label_hint = new Label();
			this.label_hint.Name = "label_hint";
			this.label_hint.Xalign = 0f;
			this.label_hint.LabelProp = Catalog.GetString("用开发者平台或社区帐号登录");
			this.vbox3.Add(this.label_hint);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox3[this.label_hint];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.fixed3 = new Fixed();
			this.fixed3.WidthRequest = 190;
			this.fixed3.HeightRequest = 35;
			this.fixed3.Name = "fixed3";
			this.fixed3.HasWindow = false;
			this.image_userName = new ImageBin();
			this.image_userName.WidthRequest = 190;
			this.image_userName.HeightRequest = 35;
			this.image_userName.Name = "image_userName";
			this.fixed3.Add(this.image_userName);
			this.entry_username = new PlaceholderEntry();
			this.entry_username.WidthRequest = 152;
			this.entry_username.HeightRequest = 29;
			this.entry_username.CanFocus = true;
			this.entry_username.Name = "entry_username";
			this.entry_username.IsEditable = true;
			this.entry_username.InvisibleChar = '●';
			this.entry_username.IsPlaceholder = false;
			this.entry_username.IsPassword = false;
			this.fixed3.Add(this.entry_username);
			Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.fixed3[this.entry_username];
			fixedChild.X = 34;
			fixedChild.Y = 3;
			this.vbox3.Add(this.fixed3);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox3[this.fixed3];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.fixed4 = new Fixed();
			this.fixed4.WidthRequest = 190;
			this.fixed4.HeightRequest = 35;
			this.fixed4.Name = "fixed4";
			this.fixed4.HasWindow = false;
			this.image_password = new ImageBin();
			this.image_password.WidthRequest = 190;
			this.image_password.HeightRequest = 35;
			this.image_password.Name = "image_password";
			this.fixed4.Add(this.image_password);
			this.entry_password = new PlaceholderEntry();
			this.entry_password.WidthRequest = 152;
			this.entry_password.HeightRequest = 29;
			this.entry_password.CanFocus = true;
			this.entry_password.Name = "entry_password";
			this.entry_password.IsEditable = true;
			this.entry_password.InvisibleChar = '●';
			this.entry_password.IsPlaceholder = false;
			this.entry_password.IsPassword = false;
			this.fixed4.Add(this.entry_password);
			Fixed.FixedChild fixedChild2 = (Fixed.FixedChild)this.fixed4[this.entry_password];
			fixedChild2.X = 34;
			fixedChild2.Y = 3;
			this.vbox3.Add(this.fixed4);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox3[this.fixed4];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.vbox5.Add(this.vbox3);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox5[this.vbox3];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.vbox4 = new VBox();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			this.label_error = new Label();
			this.label_error.Name = "label_error";
			this.label_error.Xalign = 0f;
			this.label_error.LabelProp = Catalog.GetString("错误提示");
			this.vbox4.Add(this.label_error);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox4[this.label_error];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.checkbutton_AutoLogin = new CheckboxView();
			this.checkbutton_AutoLogin.Name = "checkbutton_AutoLogin";
			this.checkbutton_AutoLogin.Active = false;
			this.hbox1.Add(this.checkbutton_AutoLogin);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox1[this.checkbutton_AutoLogin];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment2.WidthRequest = 64;
			this.alignment2.Name = "alignment2";
			this.hbox1.Add(this.alignment2);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox1[this.alignment2];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.vbox4.Add(this.hbox1);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox4[this.hbox1];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.vbox5.Add(this.vbox4);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox5[this.vbox4];
			boxChild9.Position = 1;
			this.vbox2.Add(this.vbox5);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox2[this.vbox5];
			boxChild10.Position = 0;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.eventbox_login = new EventBox();
			this.eventbox_login.Name = "eventbox_login";
			this.fixed5 = new Fixed();
			this.fixed5.WidthRequest = 190;
			this.fixed5.HeightRequest = 35;
			this.fixed5.Name = "fixed5";
			this.fixed5.HasWindow = false;
			this.image_login = new ImageBin();
			this.image_login.WidthRequest = 190;
			this.image_login.HeightRequest = 35;
			this.image_login.Name = "image_login";
			this.fixed5.Add(this.image_login);
			this.eventbox_login.Add(this.fixed5);
			this.vbox2.Add(this.eventbox_login);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox2[this.eventbox_login];
			boxChild11.PackType = PackType.End;
			boxChild11.Position = 1;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			this.alignment1.Add(this.vbox2);
			this.fixed2.Add(this.alignment1);
			this.eventbox1.Add(this.fixed2);
			base.Add(this.eventbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 215;
			base.DefaultHeight = 220;
			base.Show();
		}

		// Token: 0x04000133 RID: 307
		private Label lable_login;

		// Token: 0x04000134 RID: 308
		private Button button;

		// Token: 0x04000136 RID: 310
		private LinkView launcherlink_forgetPassword;

		// Token: 0x04000137 RID: 311
		private LoginService loginService;

		// Token: 0x04000138 RID: 312
		private EventBox eventbox1;

		// Token: 0x04000139 RID: 313
		private Fixed fixed2;

		// Token: 0x0400013A RID: 314
		private ImageBin image_back;

		// Token: 0x0400013B RID: 315
		private Alignment alignment1;

		// Token: 0x0400013C RID: 316
		private VBox vbox2;

		// Token: 0x0400013D RID: 317
		private VBox vbox5;

		// Token: 0x0400013E RID: 318
		private VBox vbox3;

		// Token: 0x0400013F RID: 319
		private Label label_hint;

		// Token: 0x04000140 RID: 320
		private Fixed fixed3;

		// Token: 0x04000141 RID: 321
		private ImageBin image_userName;

		// Token: 0x04000142 RID: 322
		private PlaceholderEntry entry_username;

		// Token: 0x04000143 RID: 323
		private Fixed fixed4;

		// Token: 0x04000144 RID: 324
		private ImageBin image_password;

		// Token: 0x04000145 RID: 325
		private PlaceholderEntry entry_password;

		// Token: 0x04000146 RID: 326
		private VBox vbox4;

		// Token: 0x04000147 RID: 327
		private Label label_error;

		// Token: 0x04000148 RID: 328
		private HBox hbox1;

		// Token: 0x04000149 RID: 329
		private CheckboxView checkbutton_AutoLogin;

		// Token: 0x0400014A RID: 330
		private Alignment alignment2;

		// Token: 0x0400014B RID: 331
		private EventBox eventbox_login;

		// Token: 0x0400014C RID: 332
		private Fixed fixed5;

		// Token: 0x0400014D RID: 333
		private ImageBin image_login;
	}
}
