using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200013B RID: 315
	public class ProjectReferenceEventArgs : EventArgs
	{
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x0002C187 File Offset: 0x0002A387
		public Project Project
		{
			get
			{
				return this.project;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x0002C18F File Offset: 0x0002A38F
		public ProjectReference ProjectReference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0002C197 File Offset: 0x0002A397
		public ProjectReferenceEventArgs(Project project, ProjectReference reference)
		{
			this.project = project;
			this.reference = reference;
		}

		// Token: 0x04000390 RID: 912
		private Project project;

		// Token: 0x04000391 RID: 913
		private ProjectReference reference;
	}
}
