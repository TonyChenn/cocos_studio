using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200017E RID: 382
	public class ProjectItemCollection : ProjectItemCollection<ProjectItem>
	{
		// Token: 0x06000F22 RID: 3874 RVA: 0x00038E1D File Offset: 0x0003701D
		public ProjectItemCollection()
		{
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00038E25 File Offset: 0x00037025
		internal ProjectItemCollection(SolutionEntityItem parent) : base(parent)
		{
		}
	}
}
