using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200017D RID: 381
	[Flags]
	public enum ProjectItemFlags
	{
		// Token: 0x04000462 RID: 1122
		None = 0,
		/// <summary>
		/// The item is for internal use and will not be shown to the user
		/// </summary>
		// Token: 0x04000463 RID: 1123
		Hidden = 1,
		/// <summary>
		/// The item will not be saved to the project file
		/// </summary>
		// Token: 0x04000464 RID: 1124
		DontPersist = 2
	}
}
