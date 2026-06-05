using System;
using System.Collections.Generic;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	// Token: 0x0200001B RID: 27
	public class AddResourcesArgs : EventArgs
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000613C File Offset: 0x0000433C
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00006153 File Offset: 0x00004353
		public ResourceFolder Parent { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000615C File Offset: 0x0000435C
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00006173 File Offset: 0x00004373
		public bool IsExpand { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000617C File Offset: 0x0000437C
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00006193 File Offset: 0x00004393
		public IEnumerable<ResourceItem> AddItems { get; private set; }

		// Token: 0x0600014E RID: 334 RVA: 0x0000619C File Offset: 0x0000439C
		public AddResourcesArgs(ResourceFolder parent, IEnumerable<ResourceItem> addItems, bool isExpand = true)
		{
			this.Parent = parent;
			this.AddItems = addItems;
			this.IsExpand = isExpand;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000061C0 File Offset: 0x000043C0
		public AddResourcesArgs(ResourceFolder parent, ResourceItem addItem, bool isExpand = true)
		{
			this.Parent = parent;
			this.AddItems = new List<ResourceItem>
			{
				addItem
			};
			this.IsExpand = isExpand;
		}
	}
}
