using System;
using System.ComponentModel;
using System.Globalization;

namespace ICSharpCode.NRefactory
{
	public class TextLocationConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(TextLocation) || base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string[] array = ((string)value).Split(new char[]
				{
					';',
					','
				});
				if (array.Length == 2)
				{
					return new TextLocation(int.Parse(array[0]), int.Parse(array[1]));
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is TextLocation)
			{
				TextLocation textLocation = (TextLocation)value;
				return textLocation.Line + ";" + textLocation.Column;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
