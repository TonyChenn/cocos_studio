using System;
using Mono.Addins;

namespace Modules.Communal.ResourcePanel
{
	internal class ResourcePanelExtensionNode : TypeExtensionNode<ResourcePanelExtensionAttribute>
	{
		public new ResourcePanelExtensionAttribute Data { get; private set; }

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
