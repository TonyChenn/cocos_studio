using System;
using CocoStudio.Model;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x0200003C RID: 60
	[TypeExtensionPoint]
	public interface IResource
	{
		// Token: 0x0600017B RID: 379
		ResourceData GetResourceData();
	}
}
