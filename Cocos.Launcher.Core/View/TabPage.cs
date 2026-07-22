using System;

namespace Cocos.Launcher.Core.View
{
	// Token: 0x0200004A RID: 74
	public class TabPage
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000A276 File Offset: 0x00008476
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000A27E File Offset: 0x0000847E
		public ITabContent TabContent { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000A287 File Offset: 0x00008487
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000A28F File Offset: 0x0000848F
		public ITabHead TabHead { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000A298 File Offset: 0x00008498
		public int Order
		{
			get
			{
				return this.TabContent.Order;
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A2A5 File Offset: 0x000084A5
		public TabPage(ITabContent content, ITabHead head)
		{
			this.TabContent = content;
			this.TabHead = head;
		}
	}
}
