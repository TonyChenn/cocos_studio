using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000137 RID: 311
	public enum ReferenceType
	{
		// Token: 0x0400037D RID: 893
		Assembly,
		// Token: 0x0400037E RID: 894
		Project,
		// Token: 0x0400037F RID: 895
		Package,
		// Token: 0x04000380 RID: 896
		Custom,
		// Token: 0x04000381 RID: 897
		[Obsolete("Use Package")]
		Gac = 2
	}
}
