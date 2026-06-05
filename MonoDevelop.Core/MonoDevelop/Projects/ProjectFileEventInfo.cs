using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000133 RID: 307
	public class ProjectFileEventInfo
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x0002B6B4 File Offset: 0x000298B4
		// (set) Token: 0x06000B8E RID: 2958 RVA: 0x0002B6BC File Offset: 0x000298BC
		public Project Project { get; private set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x0002B6C5 File Offset: 0x000298C5
		// (set) Token: 0x06000B90 RID: 2960 RVA: 0x0002B6CD File Offset: 0x000298CD
		public ProjectFile ProjectFile { get; private set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0002B6D6 File Offset: 0x000298D6
		// (set) Token: 0x06000B92 RID: 2962 RVA: 0x0002B6DE File Offset: 0x000298DE
		public string Property { get; private set; }

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002B6E7 File Offset: 0x000298E7
		public ProjectFileEventInfo(Project project, ProjectFile file, string property)
		{
			this.Property = property;
			this.ProjectFile = file;
			this.Project = project;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002B704 File Offset: 0x00029904
		public ProjectFileEventInfo(Project project, ProjectFile file) : this(project, file, null)
		{
		}
	}
}
