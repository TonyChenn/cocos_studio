using System;
using CocoStudio.Model.DataModel;
using Xwt.Drawing;

namespace CocoStudio.Model
{
	// Token: 0x020000C1 RID: 193
	public interface IProjectFileCreator
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000612 RID: 1554
		string Name { get; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000613 RID: 1555
		NodeType FileType { get; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000614 RID: 1556
		string FileExtension { get; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000615 RID: 1557
		int Order { get; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000616 RID: 1558
		string LabelName { get; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000617 RID: 1559
		string Description { get; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000618 RID: 1560
		Image Icon { get; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000619 RID: 1561
		int MaxSize { get; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600061A RID: 1562
		bool CanEditSize { get; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600061B RID: 1563
		bool IsShowTrackPoint { get; }

		// Token: 0x0600061C RID: 1564
		GameFileData CreateGameProjectData();
	}
}
