using System;
using System.Timers;
using Cocos.Launcher.Core.View;
using CocoStudio.Basic;
using CocoStudio.Core;
using GLib;
using Modules.Communal.CocoaChina;

namespace Cocos.Launcher.Core
{
	public class LoginService : ILoginService
	{
		public LoginInfo LoginInfo
		{
			get
			{
				if (this.loginInfo == null)
				{
					this.loginInfo = new LoginInfo();
				}
				return this.loginInfo;
			}
		}

		public bool IsLoginSuccessed
		{
			get
			{
				return this.isLoginSuccessed;
			}
			internal set
			{
				this.isLoginSuccessed = value;
				this.OnLoginChanged(value);
				if (value)
				{
					this.timer.Start();
					return;
				}
				this.timer.Stop();
			}
		}

		public LoginView LoginView { get; set; }

		public event EventHandler<LoginChangedEventArgs> LoginChanged;

		public LoginService()
		{
			this.timer = new Timer();
			this.timer.Interval = (double)this.defaultShowTime;
			this.timer.Elapsed += this.Timer_Tick;
		}

		private void OnLoginChanged(bool isLogin)
		{
			Timeout.Add(0U, delegate
			{
				if (this.LoginChanged != null)
				{
					this.LoginChanged(this, new LoginChangedEventArgs(isLogin));
				}
				return false;
			});
		}

		public void ShowLoginWindow()
		{
			this.LoginView.ShowLoginWindow();
		}

		private void Timer_Tick(object sender, ElapsedEventArgs e)
		{
			if (!CocoStudio.Core.Services.NetworkService.IsOK)
			{
				return;
			}
			Login login = new Login();
			login.OnResived += this.UpdateToken_OnResived;
			login.SyncCocosUpdateToken(this.LoginInfo.Refresh_token);
		}

		private void UpdateToken_OnResived(object sender, CocoaUserArgs e)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(e.User.ErrorMsg))
				{
					if (this.LoginView != null)
					{
						this.IsLoginSuccessed = false;
					}
				}
				else
				{
					this.LoginInfo.Access_token = e.User.Access_token;
					this.LoginInfo.Refresh_token = e.User.Refresh_token;
					this.OnLoginChanged(this.IsLoginSuccessed);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Timer登录失败", exception);
			}
		}

		private Timer timer;

		private int defaultShowTime = 3600000;

		private LoginInfo loginInfo;

		private bool isLoginSuccessed;
	}
}
