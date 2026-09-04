using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MonoDevelop.DesignerSupport
{
	internal class CustomProperty : PropertyDescriptor
	{
		private PropertyDescriptor prop;

		private Attribute[] customAtts;

		public override Type ComponentType => prop.ComponentType;

		public override TypeConverter Converter => prop.Converter;

		public override bool IsLocalizable => prop.IsLocalizable;

		public override bool IsReadOnly => true;

		public override Type PropertyType => prop.PropertyType;

		protected override Attribute[] AttributeArray
		{
			get
			{
				List<Attribute> list = new List<Attribute>();
				foreach (Attribute attribute in prop.Attributes)
				{
					list.Add(attribute);
				}
				list.AddRange(customAtts);
				return list.ToArray();
			}
		}

		public CustomProperty(PropertyDescriptor prop, Attribute[] customAtts)
			: base(prop)
		{
			this.prop = prop;
			this.customAtts = customAtts;
		}

		public override void AddValueChanged(object component, EventHandler handler)
		{
			prop.AddValueChanged(component, handler);
		}

		public override void RemoveValueChanged(object component, EventHandler handler)
		{
			prop.RemoveValueChanged(component, handler);
		}

		public override object GetValue(object component)
		{
			return prop.GetValue(component);
		}

		public override void SetValue(object component, object value)
		{
			prop.SetValue(component, value);
		}

		public override void ResetValue(object component)
		{
			prop.ResetValue(component);
		}

		public override bool CanResetValue(object component)
		{
			return prop.CanResetValue(component);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return prop.ShouldSerializeValue(component);
		}

		public override bool Equals(object o)
		{
			return prop.Equals(o);
		}

		public override int GetHashCode()
		{
			return prop.GetHashCode();
		}

		public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			return prop.GetChildProperties(instance, filter);
		}

		public override object GetEditor(Type editorBaseType)
		{
			return prop.GetEditor(editorBaseType);
		}
	}
}
