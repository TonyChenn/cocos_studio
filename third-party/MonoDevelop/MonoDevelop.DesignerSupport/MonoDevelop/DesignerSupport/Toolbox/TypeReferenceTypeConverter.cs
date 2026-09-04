using System;
using System.ComponentModel;
using System.Globalization;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public class TypeReferenceTypeConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return ((TypeReference)value).TypeName;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (!(destinationType == typeof(string)))
			{
				return base.CanConvertTo(context, destinationType);
			}
			return true;
		}
	}
}
