using System;
using System.Collections.Generic;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	// Token: 0x02000020 RID: 32
	public class SelectedResourceItemsChangedArgs
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000154 RID: 340 RVA: 0x0000621C File Offset: 0x0000441C
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00006233 File Offset: 0x00004433
		public List<ResourceItem> CurrentSelectedItems { get; private set; }

		// Token: 0x06000156 RID: 342 RVA: 0x0000623C File Offset: 0x0000443C
		public SelectedResourceItemsChangedArgs(List<ResourceItem> currenSelectedItems)
		{
			this.CurrentSelectedItems = currenSelectedItems;
		}
	}
}
