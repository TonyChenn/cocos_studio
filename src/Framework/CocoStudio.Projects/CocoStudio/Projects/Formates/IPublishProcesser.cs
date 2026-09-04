using System;
using System.Collections.Generic;
using CocoStudio.Model;
using Mono.Addins;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000015 RID: 21
	[TypeExtensionPoint]
	public interface IPublishProcesser
	{
		// Token: 0x06000069 RID: 105
		bool CanProcess(ResourceData resourceData);

		// Token: 0x0600006A RID: 106
		HashSet<ResourceData> Process(ResourceData resourceData);
	}
}
