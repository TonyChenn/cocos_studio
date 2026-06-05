using System;
using System.Collections.Generic;

namespace CocoStudio.Projects
{
	// Token: 0x02000072 RID: 114
	public class ResourceInfoDragData
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000CA64 File Offset: 0x0000AC64
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0000CA6C File Offset: 0x0000AC6C
		public IList<ResourceItem> Items { get; private set; }

		// Token: 0x06000388 RID: 904 RVA: 0x0000CA75 File Offset: 0x0000AC75
		public ResourceInfoDragData(IList<ResourceItem> ResourceList)
		{
			this.Items = ResourceList;
		}
	}
}
