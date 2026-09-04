using System;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Event
{
	// Token: 0x02000070 RID: 112
	public class AdjustRenderOrderEventArgs
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x00013468 File Offset: 0x00011668
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x0001347F File Offset: 0x0001167F
		public NodeObject Node { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x00013488 File Offset: 0x00011688
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x0001349F File Offset: 0x0001169F
		public MoveOrderType Action { get; private set; }

		// Token: 0x060003F1 RID: 1009 RVA: 0x000134A8 File Offset: 0x000116A8
		public AdjustRenderOrderEventArgs(NodeObject node, MoveOrderType action)
		{
			this.Node = node;
			this.Action = action;
		}
	}
}
