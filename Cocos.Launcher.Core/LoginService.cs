using System;
using System.Timers;
using Cocos.Launcher.Core.View;
using CocoStudio.Basic;
using CocoStudio.Core;
using GLib;
using Modules.Communal.CocoaChina;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000040 RID: 64
	public class LoginService : ILoginService
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000097AD File Offset: 0x000079AD
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000097C8 File Offset: 0x000079C8
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000097D0 File Offset: 0x000079D0
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000097FA File Offset: 0x000079FA
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00009802 File Offset: 0x00007A02
		public LoginView LoginView { get; set; }

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600022D RID: 557 RVA: 0x0000980C File Offset: 0x00007A0C
		// (remove) Token: 0x0600022E RID: 558 RVA: 0x00009844 File Offset: 0x00007A44
		public event EventHandler<LoginChangedEventArgs> LoginChanged;

		// Token: 0x0600022F RID: 559 RVA: 0x0000987C File Offset: 0x00007A7C
		public LoginService()
		{
			this.timer = new Timer();
			this.timer.Interval = (double)this.defaultShowTime;
			this.timer.Elapsed += this.Timer_Tick;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009908 File Offset: 0x00007B08
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

		// Token: 0x06000231 RID: 561 RVA: 0x0000993C File Offset: 0x00007B3C
		public void ShowLoginWindow()
		{
			this.LoginView.ShowLoginWindow();
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000994C File Offset: 0x00007B4C
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

		// Token: 0x06000233 RID: 563 RVA: 0x00009990 File Offset: 0x00007B90
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

		// Token: 0x040000DA RID: 218
		private Timer timer;

		// Token: 0x040000DB RID: 219
		private int defaultShowTime = 3600000;

		// Token: 0x040000DC RID: 220
		private LoginInfo loginInfo;

		// Token: 0x040000DD RID: 221
		private bool isLoginSuccessed;
	}
}
