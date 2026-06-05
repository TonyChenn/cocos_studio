using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000214 RID: 532
	public class ProjectItemEventInfo
	{
		// Token: 0x060013FD RID: 5117 RVA: 0x000530F9 File Offset: 0x000512F9
		public ProjectItemEventInfo(SolutionEntityItem solutionItem, ProjectItem item)
		{
			this.item = item;
			this.solutionItem = solutionItem;
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x0005310F File Offset: 0x0005130F
		public SolutionEntityItem SolutionItem
		{
			get
			{
				return this.solutionItem;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x00053117 File Offset: 0x00051317
		public ProjectItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x040005FF RID: 1535
		private ProjectItem item;

		// Token: 0x04000600 RID: 1536
		private SolutionEntityItem solutionItem;
	}
}
