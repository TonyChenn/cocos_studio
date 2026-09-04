using System;

namespace CocoStudio.Model
{
	// Token: 0x020000B6 RID: 182
	public interface IListViewType
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060005BB RID: 1467
		// (set) Token: 0x060005BC RID: 1468
		ListViewDirectionType DirectionType { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060005BD RID: 1469
		// (set) Token: 0x060005BE RID: 1470
		ListViewHorizontal HorizontalType { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060005BF RID: 1471
		// (set) Token: 0x060005C0 RID: 1472
		ListViewVertical VerticalType { get; set; }
	}
}
