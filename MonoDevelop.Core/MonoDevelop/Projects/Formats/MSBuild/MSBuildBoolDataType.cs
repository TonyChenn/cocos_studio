using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001C0 RID: 448
	internal class MSBuildBoolDataType : PrimitiveDataType
	{
		// Token: 0x0600113C RID: 4412 RVA: 0x0004621B File Offset: 0x0004441B
		public MSBuildBoolDataType() : base(typeof(bool))
		{
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0004622D File Offset: 0x0004442D
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			return new MSBuildBoolDataValue(base.Name, (bool)value);
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00046240 File Offset: 0x00044440
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			return string.Equals(((DataValue)data).Value, "true", StringComparison.OrdinalIgnoreCase);
		}
	}
}
