using System;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000076 RID: 118
	public class RelativeResourcePathItemProperty : ItemPropertyAttribute
	{
		// Token: 0x06000392 RID: 914 RVA: 0x0000CC14 File Offset: 0x0000AE14
		public RelativeResourcePathItemProperty()
		{
			base.SerializationDataType = typeof(RelativePathDataType);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000CC2C File Offset: 0x0000AE2C
		public RelativeResourcePathItemProperty(string name) : base(name)
		{
			base.SerializationDataType = typeof(RelativePathDataType);
		}
	}
}
