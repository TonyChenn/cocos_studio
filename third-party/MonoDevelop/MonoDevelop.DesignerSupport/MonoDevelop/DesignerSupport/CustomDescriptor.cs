using System;
using System.ComponentModel;

namespace MonoDevelop.DesignerSupport
{
	public class CustomDescriptor : ICustomTypeDescriptor
	{
		public virtual object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		public virtual PropertyDescriptorCollection GetProperties(Attribute[] arr)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, arr, noCustomTypeDesc: true);
			PropertyDescriptor[] array = new PropertyDescriptor[properties.Count];
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDescriptor propertyDescriptor = properties[i];
				Attribute[] customAttributes = GetCustomAttributes(propertyDescriptor.Name);
				if (customAttributes != null)
				{
					array[i] = new CustomProperty(propertyDescriptor, customAttributes);
				}
				else
				{
					array[i] = propertyDescriptor;
				}
			}
			return new PropertyDescriptorCollection(array);
		}

		public virtual PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}

		public virtual EventDescriptorCollection GetEvents(Attribute[] arr)
		{
			return TypeDescriptor.GetEvents(this, arr, noCustomTypeDesc: true);
		}

		public virtual EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, noCustomTypeDesc: true);
		}

		public virtual object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, noCustomTypeDesc: true);
		}

		public virtual PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this, noCustomTypeDesc: true);
		}

		public virtual EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, noCustomTypeDesc: true);
		}

		public virtual TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, noCustomTypeDesc: true);
		}

		public virtual string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, noCustomTypeDesc: true);
		}

		public virtual string GetClassName()
		{
			return TypeDescriptor.GetClassName(this, noCustomTypeDesc: true);
		}

		public virtual AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, noCustomTypeDesc: true);
		}

		protected virtual Attribute[] GetCustomAttributes(string propertyName)
		{
			if (IsReadOnly(propertyName))
			{
				return new Attribute[1] { ReadOnlyAttribute.Yes };
			}
			return null;
		}

		protected virtual bool IsReadOnly(string propertyName)
		{
			return false;
		}
	}
}
