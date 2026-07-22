using System;

namespace CocoStudio.Model
{
	// Token: 0x020000B9 RID: 185
	public interface IScale9
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060005C5 RID: 1477
		// (set) Token: 0x060005C6 RID: 1478
		bool Scale9Enable { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060005C7 RID: 1479
		// (set) Token: 0x060005C8 RID: 1480
		int LeftEage { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060005C9 RID: 1481
		// (set) Token: 0x060005CA RID: 1482
		int RightEage { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060005CB RID: 1483
		// (set) Token: 0x060005CC RID: 1484
		int TopEage { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060005CD RID: 1485
		// (set) Token: 0x060005CE RID: 1486
		int BottomEage { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060005CF RID: 1487
		SizeF ResourceSize { get; }
	}
}
