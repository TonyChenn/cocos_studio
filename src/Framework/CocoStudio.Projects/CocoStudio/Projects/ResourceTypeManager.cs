using System;
using System.Collections.Generic;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000078 RID: 120
	internal class ResourceTypeManager
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000397 RID: 919 RVA: 0x0000CD4C File Offset: 0x0000AF4C
		// (set) Token: 0x06000398 RID: 920 RVA: 0x0000CD54 File Offset: 0x0000AF54
		public List<Type> ResourceTypeList { get; private set; }

		// Token: 0x06000399 RID: 921 RVA: 0x0000CD5D File Offset: 0x0000AF5D
		public ResourceTypeManager()
		{
			this.ResourceTypeList = new List<Type>();
			this.CollectResourceType();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000CD78 File Offset: 0x0000AF78
		private void CollectResourceType()
		{
			ExtensionNodeList extensionNodes = AddinManager.GetExtensionNodes(typeof(IResource));
			foreach (object obj in extensionNodes)
			{
				TypeExtensionNode typeExtensionNode = (TypeExtensionNode)obj;
				this.ResourceTypeList.Add(typeExtensionNode.Type);
			}
		}
	}
}
