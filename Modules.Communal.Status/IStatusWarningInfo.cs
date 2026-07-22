using System;

namespace Modules.Communal.Status
{
	// Token: 0x02000007 RID: 7
	public interface IStatusWarningInfo
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002F RID: 47
		string Tooltip { get; }

		// Token: 0x06000030 RID: 48
		void OnClick();
	}
}
