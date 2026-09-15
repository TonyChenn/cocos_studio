using System;
using System.ComponentModel;
using Cocos.Launcher.Control;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.UserStatistics;
using GLib;
using Gtk;
using Modules.Communal.CocoaChina;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core.View
{
	[ToolboxItem(true)]
	public class LoginView : HBox
	{
		public LoginView()
		{
			this.Initialize();
			this.InitEvent();
			this.Initlanguage();
		}

		private void Initialize()
		{
			this.loginService = new LoginService();
			this.loginService.LoginView = this;
			Services.LoginService = this.loginService;
			base.Spacing = 6;
			this.launcherlink_login = new LinkView();
			this.launcherlink_login.SetFontSize(14.0);
			this.launcherlink_login.HeightRequest = 20;
			this.launcherlink_login.SetBackGroundColor(ConstantConfig.Colors.MainTitleColor);
			this.launcherlink_login.SetForeGroundColor(ConstantConfig.Colors.LoginColor);
			base.PackStart(this.launcherlink_login, false, false, 0U);
			Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment.TopPadding = 6U;
			alignment.BottomPadding = 4U;
			VSeparator vseparator = new VSeparator();
			vseparator.HeightRequest = 10;
			vseparator.WidthRequest = 1;
			vseparator.ModifyBg(StateType.Normal, ConstantConfig.Colors.LoginColor);
			alignment.Add(vseparator);
			base.PackStart(alignment, false, false, 0U);
			this.launcherlink_register = new LinkView();
			this.launcherlink_register.SetFontSize(14.0);
			this.launcherlink_register.HeightRequest = 20;
			this.launcherlink_register.SetBackGroundColor(ConstantConfig.Colors.MainTitleColor);
			this.launcherlink_register.SetForeGroundColor(ConstantConfig.Colors.LoginColor);
			this.launcherlink_register.SetTag(ConstantConfig.Constant.RegisterUri);
			base.PackStart(this.launcherlink_register, false, false, 0U);
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.AutoLoginCocos();
				return;
			}
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
		}

		private void InitEvent()
		{
			this.launcherlink_login.LinkClicked += this.launcherlink_login_LinkClicked;
			this.launcherlink_register.LinkClicked += this.launcherlink_register_LinkClicked;
			this.loginService.LoginChanged += this.loginService_LoginChanged;
		}

		private void Initlanguage()
		{
			this.SetDefaultLoginValue();
		}

		private void SetDefaultLoginValue()
		{
			this.launcherlink_login.SetLableText(LanguageInfo.Launcher_Login);
			this.launcherlink_login.SetTag(LanguageInfo.Launcher_Login);
			this.launcherlink_register.SetLableText(LanguageInfo.Launcher_Signup);
		}

		public void ShowLoginWindow()
		{
			int num;
			int num2;
			this.launcherlink_login.GdkWindow.GetOrigin(out num, out num2);
			LoginWindow loginWindow = new LoginWindow(this.loginService);
			loginWindow.LoginCompleted += this.window_LoginCompleted;
			if (Platform.IsWindows)
			{
				loginWindow.Move(num - 162, num2);
			}
			else
			{
				loginWindow.Move(num - 195, num2 + 26);
			}
			loginWindow.RemoveWindowBorder();
			loginWindow.ShowAll();
		}

		private void ShowUserName()
		{
			string text = this.loginService.LoginInfo.UserName;
			if (text.Contains("@"))
			{
				text = text.Substring(0, text.IndexOf("@")) + "...";
			}
			this.launcherlink_login.SetLableText(text);
			this.launcherlink_login.SetTag(this.loginService.LoginInfo.UserName);
		}

		private void GetUserID()
		{
			Login login = new Login();
			login.OnResived += this.UserInfo_OnResived;
			login.SyncCocosUserInfo(this.loginService.LoginInfo.Access_token);
		}

		private void LoginSuccessed()
		{
			GLib.Timeout.Add(0U, delegate
			{
				this.ShowUserName();
				this.launcherlink_register.SetLableText(LanguageInfo.Launcher_LogOut);
				return false;
			});
		}

		private void QuitLogin()
		{
			this.loginService.LoginInfo.UserID = null;
			this.SetDefaultLoginValue();
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				Login login = new Login();
				login.SyncCocosLogOut(this.loginService.LoginInfo.Access_token);
			}
		}

		private void AutoLoginCocos()
		{
			if (this.loginService.LoginInfo.IsAutoLogin)
			{
				Login login = new Login();
				login.OnResived += this.login_OnResived;
				login.SyncLoginCocos(this.loginService.LoginInfo.UserName, this.loginService.LoginInfo.UserPassword);
			}
		}

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
					this.loginService.LoginInfo.UserName = e.User.UserName;
					this.loginService.LoginInfo.UserPassword = e.User.PassWord;
					this.loginService.LoginInfo.Access_token = e.User.Access_token;
					this.loginService.LoginInfo.Refresh_token = e.User.Refresh_token;
					this.GetUserID();
					Tracker.Add(ViewRegions.None, "StayLogin", "", "");
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("自动登录失败:", exception);
			}
		}

		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			if (e.IsNetworkingSuccessed)
			{
				this.AutoLoginCocos();
				CocoStudio.Core.Services.NetworkService.NetworkChanged -= this.NetworkService_NetworkChanged;
			}
		}

		private static void RegisterClicked(object sender, LinkClickedEventArgs e)
		{
			WebHelper.OnOpenWeb(sender, e);
			Tracker.Add(ViewRegions.None, "SignUp", "", "");
		}

		private void launcherlink_login_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			if (this.launcherlink_login.GetTag().ToString() == LanguageInfo.Launcher_Login)
			{
				this.ShowLoginWindow();
				return;
			}
			if (LanguageOption.CurrentLanguage == LanguageType.Chinese)
			{
				Redirect.RedirectCocos(this.loginService.LoginInfo.Access_token);
			}
		}

		private void launcherlink_register_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			if (this.launcherlink_register.GetLableText() == LanguageInfo.Launcher_Signup)
			{
				LoginView.RegisterClicked(sender, e);
				return;
			}
			this.loginService.IsLoginSuccessed = false;
		}

		private void window_LoginCompleted(object sender, EventArgs e)
		{
			this.GetUserID();
		}

		private void UserInfo_OnResived(object sender, CocoaUserArgs e)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(e.User.ErrorMsg))
				{
					this.loginService.LoginInfo.UserID = null;
				}
				else
				{
					this.loginService.LoginInfo.UserID = e.User.UserID;
					this.loginService.IsLoginSuccessed = true;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("获取用户ID失败", exception);
			}
		}

		private void loginService_LoginChanged(object sender, LoginChangedEventArgs e)
		{
			if (this.loginService.IsLoginSuccessed)
			{
				this.LoginSuccessed();
				return;
			}
			this.QuitLogin();
		}

		private LinkView launcherlink_register;

		private LinkView launcherlink_login;

		private LoginService loginService;
	}
}
