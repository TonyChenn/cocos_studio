using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Model;

namespace Modules.Communal.PropertyGrid
{
	public class PropertyItem
	{
		public static object FirstObject
		{
			get
			{
				object result;
				if (PropertyItem.Objects == null || PropertyItem.Objects.Count == 0)
				{
					result = null;
				}
				else
				{
					result = PropertyItem.Objects[0];
				}
				return result;
			}
		}

		public static IReadOnlyList<object> Objects { get; internal set; }

		public string Name { get; private set; }

		public object FirstValue
		{
			get
			{
				return this.Values[0];
			}
			set
			{
				this.Values[0] = value;
			}
		}

		public PropertyValueItem Values { get; private set; }

		public AttributeCollection Attributes { get; private set; }

		public OperationMask? RequestOperation { get; private set; }

		public PropertyItem(PropertyDescriptor propDesc)
		{
			this.Name = propDesc.Name;
			this.Attributes = propDesc.Attributes;
			this.Values = new PropertyValueItem(this.Name);
			this.RequestOperation = null;
			RequestOperationModeAttribute requestOperationModeAttribute = propDesc.Attributes[typeof(RequestOperationModeAttribute)] as RequestOperationModeAttribute;
			if (requestOperationModeAttribute != null)
			{
				this.RequestOperation = new OperationMask?(requestOperationModeAttribute.RequestMode);
			}
		}

		public T GetValue<T>(int index)
		{
			object obj = this.Values[index];
			return (T)((object)obj);
		}
	}
}
