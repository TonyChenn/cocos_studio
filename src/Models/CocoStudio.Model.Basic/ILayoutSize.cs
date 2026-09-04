using System;

namespace CocoStudio.Model
{
	// Token: 0x0200000C RID: 12
	public interface ILayoutSize
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004C RID: 76
		// (set) Token: 0x0600004D RID: 77
		bool PercentWidthEnable { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004E RID: 78
		// (set) Token: 0x0600004F RID: 79
		bool PercentHeightEnable { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000050 RID: 80
		// (set) Token: 0x06000051 RID: 81
		float PercentWidth { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000052 RID: 82
		// (set) Token: 0x06000053 RID: 83
		float PercentHeight { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000054 RID: 84
		// (set) Token: 0x06000055 RID: 85
		float SizeWidth { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000056 RID: 86
		// (set) Token: 0x06000057 RID: 87
		float SizeHeight { get; set; }
	}
}
