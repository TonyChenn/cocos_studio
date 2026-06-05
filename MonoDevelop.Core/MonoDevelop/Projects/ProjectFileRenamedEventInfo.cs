using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000136 RID: 310
	public class ProjectFileRenamedEventInfo : ProjectFileEventInfo
	{
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0002B725 File Offset: 0x00029925
		public FilePath OldName
		{
			get
			{
				return this.oldName;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x0002B72D File Offset: 0x0002992D
		public FilePath NewName
		{
			get
			{
				return base.ProjectFile.FilePath;
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002B73A File Offset: 0x0002993A
		public ProjectFileRenamedEventInfo(Project project, ProjectFile file, FilePath oldName) : base(project, file)
		{
			this.oldName = oldName;
		}

		// Token: 0x0400037B RID: 891
		private FilePath oldName;
	}
}
