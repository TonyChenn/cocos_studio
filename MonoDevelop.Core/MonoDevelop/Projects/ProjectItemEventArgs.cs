using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000213 RID: 531
	public class ProjectItemEventArgs : EventArgsChain<ProjectItemEventInfo>
	{
		// Token: 0x060013FB RID: 5115 RVA: 0x000530DC File Offset: 0x000512DC
		public ProjectItemEventArgs()
		{
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x000530E4 File Offset: 0x000512E4
		public ProjectItemEventArgs(SolutionEntityItem solutionItem, ProjectItem item)
		{
			base.Add(new ProjectItemEventInfo(solutionItem, item));
		}
	}
}
