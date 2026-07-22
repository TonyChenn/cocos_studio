using System;

namespace CocoStudio.Model
{
	// Token: 0x020000BB RID: 187
	public interface IStretchSize
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060005D2 RID: 1490
		// (set) Token: 0x060005D3 RID: 1491
		bool StretchWidthEnable { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060005D4 RID: 1492
		// (set) Token: 0x060005D5 RID: 1493
		bool StretchHeightEnable { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060005D6 RID: 1494
		// (set) Token: 0x060005D7 RID: 1495
		bool CanShowStretch { get; set; }
	}
}
