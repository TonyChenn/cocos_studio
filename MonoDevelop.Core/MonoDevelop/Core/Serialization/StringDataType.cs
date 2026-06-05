using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000085 RID: 133
	public class StringDataType : PrimitiveDataType
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x0000F1A3 File Offset: 0x0000D3A3
		public StringDataType() : base(typeof(string))
		{
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000F1B5 File Offset: 0x0000D3B5
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			return new DataValue(base.Name, (string)value);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000F1C8 File Offset: 0x0000D3C8
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			DataValue dataValue = data as DataValue;
			if (dataValue != null)
			{
				return dataValue.Value;
			}
			DataItem dataItem = (DataItem)data;
			if (dataItem.HasItemData)
			{
				throw new InvalidOperationException("Found complex element, expecting primitive");
			}
			return "";
		}
	}
}
