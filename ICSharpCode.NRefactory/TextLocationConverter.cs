using System;
using System.ComponentModel;
using System.Globalization;

namespace ICSharpCode.NRefactory
{
	// Token: 0x02000053 RID: 83
	public class TextLocationConverter : TypeConverter
	{
		// Token: 0x0600025F RID: 607 RVA: 0x00006D3A File Offset: 0x00005D3A
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00006D58 File Offset: 0x00005D58
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(TextLocation) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00006D78 File Offset: 0x00005D78
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

		// Token: 0x06000262 RID: 610 RVA: 0x00006DD8 File Offset: 0x00005DD8
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
