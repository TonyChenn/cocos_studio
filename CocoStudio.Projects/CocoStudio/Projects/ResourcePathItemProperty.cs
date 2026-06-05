using System;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000074 RID: 116
	public class ResourcePathItemProperty : ItemPropertyAttribute
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0000CAB2 File Offset: 0x0000ACB2
		public ResourcePathItemProperty()
		{
			base.SerializationDataType = typeof(PathDataType);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000CACA File Offset: 0x0000ACCA
		public ResourcePathItemProperty(string name) : base(name)
		{
			base.SerializationDataType = typeof(PathDataType);
		}
	}
}
