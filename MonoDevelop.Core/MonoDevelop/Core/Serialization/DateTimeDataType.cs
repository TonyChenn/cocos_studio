using System;
using System.Xml;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000082 RID: 130
	internal class DateTimeDataType : PrimitiveDataType
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x0000F05B File Offset: 0x0000D25B
		public DateTimeDataType() : base(typeof(DateTime))
		{
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000F06D File Offset: 0x0000D26D
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			return new DataValue(base.Name, XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.Local));
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000F086 File Offset: 0x0000D286
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			return XmlConvert.ToDateTime(((DataValue)data).Value, XmlDateTimeSerializationMode.Local);
		}
	}
}
