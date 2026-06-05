using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200012A RID: 298
	public class ProjectEventArgs : EventArgs
	{
		// Token: 0x06000B2F RID: 2863 RVA: 0x0002A659 File Offset: 0x00028859
		public ProjectEventArgs(Project project)
		{
			this.project = project;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0002A668 File Offset: 0x00028868
		public Project Project
		{
			get
			{
				return this.project;
			}
		}

		// Token: 0x04000353 RID: 851
		private Project project;
	}
}
