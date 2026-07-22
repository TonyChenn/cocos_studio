using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000038 RID: 56
	public interface IOutputService
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060001F6 RID: 502
		// (remove) Token: 0x060001F7 RID: 503
		event Action<string> Output;

		// Token: 0x060001F8 RID: 504
		void Info(string info);
	}
}
