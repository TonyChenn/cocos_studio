using System;
using System.Collections.Generic;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x02000069 RID: 105
	public class CocosItemCreateInfo
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000B528 File Offset: 0x00009728
		// (set) Token: 0x06000306 RID: 774 RVA: 0x0000B530 File Offset: 0x00009730
		public FilePath FileName { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000B539 File Offset: 0x00009739
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000B541 File Offset: 0x00009741
		public string ContentType { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000B54A File Offset: 0x0000974A
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0000B552 File Offset: 0x00009752
		public IList<ResourceItem> SelectedResourceItem { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0000B55B File Offset: 0x0000975B
		// (set) Token: 0x0600030C RID: 780 RVA: 0x0000B563 File Offset: 0x00009763
		public float Width { get; private set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000B56C File Offset: 0x0000976C
		// (set) Token: 0x0600030E RID: 782 RVA: 0x0000B574 File Offset: 0x00009774
		public float Height { get; private set; }

		// Token: 0x0600030F RID: 783 RVA: 0x0000B57D File Offset: 0x0000977D
		public CocosItemCreateInfo(FilePath fileName, IList<ResourceItem> selectedItems = null, float width = 0f, float height = 0f)
		{
			this.FileName = fileName;
			this.SelectedResourceItem = selectedItems;
			this.Width = width;
			this.Height = height;
		}
	}
}
