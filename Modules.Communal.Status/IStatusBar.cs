using System;
using CocoStudio.Core;

namespace Modules.Communal.Status
{
	// Token: 0x02000005 RID: 5
	public interface IStatusBar : IService
	{
		// Token: 0x0600000D RID: 13
		void ShowWarningInfo(IStatusWarningInfo info);

		// Token: 0x0600000E RID: 14
		void HideWarningInfo();
	}
}
