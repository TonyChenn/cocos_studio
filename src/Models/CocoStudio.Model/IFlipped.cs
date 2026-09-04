using System;

namespace CocoStudio.Model
{
	// Token: 0x020000AE RID: 174
	public interface IFlipped
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000596 RID: 1430
		// (set) Token: 0x06000597 RID: 1431
		bool FlipX { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000598 RID: 1432
		// (set) Token: 0x06000599 RID: 1433
		bool FlipY { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600059A RID: 1434
		bool IsReverse { get; }
	}
}
