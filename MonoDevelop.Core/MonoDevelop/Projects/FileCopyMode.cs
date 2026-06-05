using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000176 RID: 374
	public enum FileCopyMode
	{
		// Token: 0x04000440 RID: 1088
		[LocalizedDescription("Do not copy")]
		None,
		// Token: 0x04000441 RID: 1089
		[LocalizedDescription("Always copy")]
		Always,
		// Token: 0x04000442 RID: 1090
		[LocalizedDescription("Copy if newer")]
		PreserveNewest
	}
}
