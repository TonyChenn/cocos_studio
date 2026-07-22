using System;

namespace CocoStudio.Model
{
	// Token: 0x020000B2 RID: 178
	public interface ILayoutMargin
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060005AB RID: 1451
		// (set) Token: 0x060005AC RID: 1452
		HorizontalBerthEdge HorizontalEdge { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060005AD RID: 1453
		// (set) Token: 0x060005AE RID: 1454
		VerticalBerthEdge VerticalEdge { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060005AF RID: 1455
		// (set) Token: 0x060005B0 RID: 1456
		bool PercentHorizontalEnable { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060005B1 RID: 1457
		// (set) Token: 0x060005B2 RID: 1458
		bool PercentVertialEnable { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060005B3 RID: 1459
		// (set) Token: 0x060005B4 RID: 1460
		float PercentHorizontalMargin { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060005B5 RID: 1461
		// (set) Token: 0x060005B6 RID: 1462
		float PercentVerticalMargin { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060005B7 RID: 1463
		// (set) Token: 0x060005B8 RID: 1464
		float HorizontalMargin { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060005B9 RID: 1465
		// (set) Token: 0x060005BA RID: 1466
		float VerticalMargin { get; set; }
	}
}
