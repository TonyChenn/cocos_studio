using System;

namespace CocoStudio.Projects
{
	// Token: 0x02000073 RID: 115
	public class ResourceItemCollection : ItemCollection<ResourceItem>
	{
		// Token: 0x06000389 RID: 905 RVA: 0x0000CA84 File Offset: 0x0000AC84
		private ResourceItemCollection()
		{
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000CA8C File Offset: 0x0000AC8C
		public ResourceItemCollection(ResourceItem parentItem)
		{
			this.parentItem = parentItem;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000CA9B File Offset: 0x0000AC9B
		protected override void OnAdd(ResourceItem item)
		{
			item.Parent = this.parentItem;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000CAA9 File Offset: 0x0000ACA9
		protected override void OnRemove(ResourceItem item)
		{
			item.Parent = null;
		}

		// Token: 0x040000D8 RID: 216
		private ResourceItem parentItem;
	}
}
