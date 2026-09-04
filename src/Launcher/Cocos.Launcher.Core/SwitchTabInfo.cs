using System;
using Gtk;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000036 RID: 54
	public class SwitchTabInfo
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00008F58 File Offset: 0x00007158
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00008F60 File Offset: 0x00007160
		public int Order { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00008F69 File Offset: 0x00007169
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00008F71 File Offset: 0x00007171
		public string Url { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00008F7A File Offset: 0x0000717A
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00008F82 File Offset: 0x00007182
		public Widget ParentWidget { get; set; }

		// Token: 0x060001F1 RID: 497 RVA: 0x00008F8B File Offset: 0x0000718B
		public SwitchTabInfo()
		{
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008F93 File Offset: 0x00007193
		public SwitchTabInfo(int order)
		{
			this.Order = order;
		}
	}
}
