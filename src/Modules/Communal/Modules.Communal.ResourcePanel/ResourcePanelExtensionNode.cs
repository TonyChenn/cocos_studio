using System;
using Mono.Addins;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000007 RID: 7
	internal class ResourcePanelExtensionNode : TypeExtensionNode<ResourcePanelExtensionAttribute>
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002D65 File Offset: 0x00000F65
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002D6D File Offset: 0x00000F6D
		public new ResourcePanelExtensionAttribute Data { get; private set; }

		// Token: 0x06000032 RID: 50 RVA: 0x00002D78 File Offset: 0x00000F78
		protected override void Read(NodeElement elem)
		{
			base.Read(elem);
			object[] customAttributes = base.Type.GetCustomAttributes(typeof(ResourcePanelExtensionAttribute), false);
			if (customAttributes.Length > 0)
			{
				this.Data = (customAttributes[0] as ResourcePanelExtensionAttribute);
				return;
			}
			this.Data = base.Data;
		}
	}
}
