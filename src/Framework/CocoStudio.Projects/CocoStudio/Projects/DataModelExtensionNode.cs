using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	public class DataModelExtensionNode : TypeExtensionNode<DataModelExtensionAttribute>
	{
		public new DataModelExtensionAttribute Data { get; private set; }

		protected override void Read(NodeElement elem)
		{
			base.Read(elem);
			object[] customAttributes = base.Type.GetCustomAttributes(typeof(DataModelExtensionAttribute), false);
			if (customAttributes.Length > 0)
			{
				this.Data = (customAttributes[0] as DataModelExtensionAttribute);
				return;
			}
			this.Data = base.Data;
		}
	}
}
