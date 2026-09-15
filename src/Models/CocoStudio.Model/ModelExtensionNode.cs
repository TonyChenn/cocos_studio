using System;
using Mono.Addins;

namespace CocoStudio.Model
{
	public class ModelExtensionNode : TypeExtensionNode<ModelExtensionAttribute>, IComparable<ModelExtensionNode>
	{
		public new ModelExtensionAttribute Data { get; private set; }

		protected override void Read(NodeElement elem)
		{
			base.Read(elem);
			object[] customAttributes = base.Type.GetCustomAttributes(typeof(ModelExtensionAttribute), false);
			if (customAttributes.Length > 0)
			{
				this.Data = (customAttributes[0] as ModelExtensionAttribute);
			}
			else
			{
				this.Data = base.Data;
			}
		}

		public int CompareTo(ModelExtensionNode other)
		{
			int result;
			if (this.Data.IsDefault && !other.Data.IsDefault)
			{
				result = -1;
			}
			else if (!this.Data.IsDefault && other.Data.IsDefault)
			{
				result = 1;
			}
			else
			{
				result = this.Data.Order.CompareTo(other.Data.Order);
			}
			return result;
		}
	}
}
