using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001C2 RID: 450
	internal class MSBuildNullableBoolDataType : PrimitiveDataType
	{
		// Token: 0x06001142 RID: 4418 RVA: 0x0004628D File Offset: 0x0004448D
		public MSBuildNullableBoolDataType() : base(typeof(bool))
		{
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0004629F File Offset: 0x0004449F
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			return new MSBuildNullableBoolDataValue(base.Name, (bool?)value);
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x000462B4 File Offset: 0x000444B4
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			DataValue dataValue = (DataValue)data;
			if (string.IsNullOrEmpty(dataValue.Value))
			{
				return null;
			}
			return new bool?(string.Equals(dataValue.Value, "true", StringComparison.OrdinalIgnoreCase));
		}
	}
}
