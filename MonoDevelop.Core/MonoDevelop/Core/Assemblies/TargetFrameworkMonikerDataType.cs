using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x02000216 RID: 534
	internal class TargetFrameworkMonikerDataType : DataType
	{
		// Token: 0x0600141F RID: 5151 RVA: 0x0005364C File Offset: 0x0005184C
		public TargetFrameworkMonikerDataType(Type dataType) : base(dataType)
		{
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x00053655 File Offset: 0x00051855
		public override bool IsSimpleType
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x00053658 File Offset: 0x00051858
		public override bool CanCreateInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0005365B File Offset: 0x0005185B
		public override bool CanReuseInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0005365E File Offset: 0x0005185E
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			return new DataValue(base.Name, ((TargetFrameworkMoniker)value).ToString());
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00053676 File Offset: 0x00051876
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			return TargetFrameworkMoniker.Parse(((DataValue)data).Value);
		}
	}
}
