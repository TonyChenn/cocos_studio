using System;

namespace CocoStudio.Core
{
	// Token: 0x02000033 RID: 51
	public class RecentDocumentChangeEventArgs : EventArgs
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00009618 File Offset: 0x00007818
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000962F File Offset: 0x0000782F
		public EnumRecentPrjChangeType ChangeType { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00009638 File Offset: 0x00007838
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000964F File Offset: 0x0000784F
		public CocosItemModel CocosItemModel { get; private set; }

		// Token: 0x060001F6 RID: 502 RVA: 0x00009658 File Offset: 0x00007858
		public RecentDocumentChangeEventArgs(CocosItemModel prj, EnumRecentPrjChangeType changeType)
		{
			this.CocosItemModel = prj;
			this.ChangeType = changeType;
		}
	}
}
