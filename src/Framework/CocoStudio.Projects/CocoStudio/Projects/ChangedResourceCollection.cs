using System;
using System.Collections.Generic;
using CocoStudio.Model;

namespace CocoStudio.Projects
{
	// Token: 0x02000052 RID: 82
	public class ChangedResourceCollection : Dictionary<ResourceData, ResourceFile>
	{
		// Token: 0x06000245 RID: 581 RVA: 0x0000905D File Offset: 0x0000725D
		public ChangedResourceCollection(ResourceData oldData, ResourceItem resourceFile)
		{
			base.Add(oldData, resourceFile as ResourceFile);
		}
	}
}
