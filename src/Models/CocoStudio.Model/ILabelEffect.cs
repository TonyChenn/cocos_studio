using System;
using System.Drawing;

namespace CocoStudio.Model
{
	// Token: 0x020000AF RID: 175
	public interface ILabelEffect
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600059B RID: 1435
		// (set) Token: 0x0600059C RID: 1436
		bool ShadowEnabled { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600059D RID: 1437
		// (set) Token: 0x0600059E RID: 1438
		float ShadowOffsetX { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600059F RID: 1439
		// (set) Token: 0x060005A0 RID: 1440
		float ShadowOffsetY { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060005A1 RID: 1441
		// (set) Token: 0x060005A2 RID: 1442
		int ShadowBlurRadius { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060005A3 RID: 1443
		// (set) Token: 0x060005A4 RID: 1444
		Color ShadowColor { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060005A5 RID: 1445
		// (set) Token: 0x060005A6 RID: 1446
		bool OutlineEnabled { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060005A7 RID: 1447
		// (set) Token: 0x060005A8 RID: 1448
		Color OutlineColor { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060005A9 RID: 1449
		// (set) Token: 0x060005AA RID: 1450
		int OutlineSize { get; set; }
	}
}
