using System;

namespace CocoStudio.Model
{
	// Token: 0x020000B7 RID: 183
	public interface IPlayControl
	{
		// Token: 0x060005C1 RID: 1473
		bool HasData();

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060005C2 RID: 1474
		// (set) Token: 0x060005C3 RID: 1475
		bool IsPlaying { get; set; }
	}
}
