using System;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel
{
	internal class FrameExtensionNode : TypeExtensionNode<FrameExtensionAttribute>
	{
		public new FrameExtensionAttribute Data { get; private set; }

		protected override void Read(NodeElement elem)
		{
			base.Read(elem);
			object[] customAttributes = base.Type.GetCustomAttributes(typeof(FrameExtensionAttribute), false);
			if (customAttributes.Length > 0)
			{
				this.Data = (customAttributes[0] as FrameExtensionAttribute);
			}
			else
			{
				this.Data = base.Data;
			}
		}
	}
}
