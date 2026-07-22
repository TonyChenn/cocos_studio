using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200003F RID: 63
	public class LoginChangedEventArgs : EventArgs
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000978D File Offset: 0x0000798D
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00009795 File Offset: 0x00007995
		public bool IsLogin { get; private set; }

		// Token: 0x06000227 RID: 551 RVA: 0x0000979E File Offset: 0x0000799E
		public LoginChangedEventArgs(bool isLogin)
		{
			this.IsLogin = isLogin;
		}
	}
}
