using System;

namespace CocoStudio.Model
{
	// Token: 0x0200000D RID: 13
	public interface ILayoutSizeWithScale9 : ILayoutSize
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000058 RID: 88
		// (set) Token: 0x06000059 RID: 89
		bool Scale9Enable { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005A RID: 90
		// (set) Token: 0x0600005B RID: 91
		int LeftEage { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005C RID: 92
		// (set) Token: 0x0600005D RID: 93
		int RightEage { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600005E RID: 94
		// (set) Token: 0x0600005F RID: 95
		int TopEage { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000060 RID: 96
		// (set) Token: 0x06000061 RID: 97
		int BottomEage { get; set; }
	}
}
