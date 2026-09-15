using System;

namespace Cocos.Launcher.Core
{
	public class LoginChangedEventArgs : EventArgs
	{
		public bool IsLogin { get; private set; }

		public LoginChangedEventArgs(bool isLogin)
		{
			this.IsLogin = isLogin;
		}
	}
}
