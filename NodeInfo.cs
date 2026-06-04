using System;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000031 RID: 49
	public class NodeInfo
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x0000A224 File Offset: 0x00008424
		public NodeInfo()
		{
			this.Reset();
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000A232 File Offset: 0x00008432
		public void Reset()
		{
			this.Name = string.Empty;
			this.IconInfo = new IconInfo();
			this.StatusMessage = string.Empty;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000A255 File Offset: 0x00008455
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x0000A25D File Offset: 0x0000845D
		public string Name { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000A266 File Offset: 0x00008466
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x0000A26E File Offset: 0x0000846E
		public IconInfo IconInfo { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000A277 File Offset: 0x00008477
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x0000A27F File Offset: 0x0000847F
		public string StatusMessage { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000A288 File Offset: 0x00008488
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000A290 File Offset: 0x00008490
		public object DataItem { get; set; }

		// Token: 0x04000099 RID: 153
		public bool IsError;

		// Token: 0x0400009A RID: 154
		public NodeBuilder[] BuilderChain;
	}
}
