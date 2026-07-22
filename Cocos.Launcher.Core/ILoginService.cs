using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000012 RID: 18
	public interface ILoginService
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000087 RID: 135
		bool IsLoginSuccessed { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000088 RID: 136
		LoginInfo LoginInfo { get; }

		// Token: 0x06000089 RID: 137
		void ShowLoginWindow();

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600008A RID: 138
		// (remove) Token: 0x0600008B RID: 139
		event EventHandler<LoginChangedEventArgs> LoginChanged;
	}
}
