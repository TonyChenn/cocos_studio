using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000135 RID: 309
	public class ProjectFileRenamedEventArgs : EventArgsChain<ProjectFileRenamedEventInfo>
	{
		// Token: 0x06000B99 RID: 2969 RVA: 0x0002B70F File Offset: 0x0002990F
		public ProjectFileRenamedEventArgs(Project project, ProjectFile file, FilePath oldName)
		{
			base.Add(new ProjectFileRenamedEventInfo(project, file, oldName));
		}
	}
}
