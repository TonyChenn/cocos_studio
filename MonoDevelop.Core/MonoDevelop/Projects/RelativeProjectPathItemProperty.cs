using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000103 RID: 259
	public class RelativeProjectPathItemProperty : ItemPropertyAttribute
	{
		// Token: 0x0600095E RID: 2398 RVA: 0x000257AC File Offset: 0x000239AC
		public RelativeProjectPathItemProperty()
		{
			base.SerializationDataType = typeof(RelativePathDataType);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x000257C4 File Offset: 0x000239C4
		public RelativeProjectPathItemProperty(string name) : base(name)
		{
			base.SerializationDataType = typeof(RelativePathDataType);
		}
	}
}
