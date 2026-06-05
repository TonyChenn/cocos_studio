using System;
using System.Globalization;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000081 RID: 129
	public class PrimitiveDataType : DataType
	{
		// Token: 0x06000443 RID: 1091 RVA: 0x0000EFBC File Offset: 0x0000D1BC
		public PrimitiveDataType(Type propType) : base(propType)
		{
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000EFC5 File Offset: 0x0000D1C5
		public override bool IsSimpleType
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x0000EFC8 File Offset: 0x0000D1C8
		public override bool CanCreateInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000EFCB File Offset: 0x0000D1CB
		public override bool CanReuseInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000EFD0 File Offset: 0x0000D1D0
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			if (!(value is IConvertible))
			{
				return new DataValue(base.Name, value.ToString());
			}
			if (value is float)
			{
				double num = Convert.ToDouble(value);
				return new DataValue(base.Name, num.ToString("F4", CultureInfo.InvariantCulture));
			}
			return new DataValue(base.Name, ((IConvertible)value).ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000F03E File Offset: 0x0000D23E
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			return Convert.ChangeType(((DataValue)data).Value, base.ValueType, CultureInfo.InvariantCulture);
		}
	}
}
