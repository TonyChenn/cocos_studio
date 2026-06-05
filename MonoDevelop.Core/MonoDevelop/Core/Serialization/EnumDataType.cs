using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000072 RID: 114
	public class EnumDataType : DataType
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x0000E45C File Offset: 0x0000C65C
		public EnumDataType(Type propType) : base(propType)
		{
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0000E465 File Offset: 0x0000C665
		public override bool IsSimpleType
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0000E468 File Offset: 0x0000C668
		public override bool CanCreateInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000E46B File Offset: 0x0000C66B
		public override bool CanReuseInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000E46E File Offset: 0x0000C66E
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			return new DataValue(base.Name, value.ToString());
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000E481 File Offset: 0x0000C681
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			return Enum.Parse(base.ValueType, ((DataValue)data).Value, true);
		}
	}
}
