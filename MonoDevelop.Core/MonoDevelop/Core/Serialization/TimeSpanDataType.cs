using System;
using System.Globalization;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000083 RID: 131
	internal class TimeSpanDataType : PrimitiveDataType
	{
		// Token: 0x0600044C RID: 1100 RVA: 0x0000F09E File Offset: 0x0000D29E
		public TimeSpanDataType() : base(typeof(TimeSpan))
		{
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000F0B0 File Offset: 0x0000D2B0
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			return new DataValue(base.Name, ((TimeSpan)value).Ticks.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000F0E3 File Offset: 0x0000D2E3
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			return TimeSpan.FromTicks(long.Parse(((DataValue)data).Value, CultureInfo.InvariantCulture));
		}
	}
}
