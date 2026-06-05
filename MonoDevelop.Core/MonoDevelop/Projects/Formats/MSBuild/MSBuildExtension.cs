using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x0200024E RID: 590
	public class MSBuildExtension
	{
		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060015B3 RID: 5555 RVA: 0x00057DDB File Offset: 0x00055FDB
		// (set) Token: 0x060015B4 RID: 5556 RVA: 0x00057DE3 File Offset: 0x00055FE3
		public MSBuildProjectHandler Handler { get; set; }

		// Token: 0x060015B5 RID: 5557 RVA: 0x00057DEC File Offset: 0x00055FEC
		public virtual void LoadProject(IProgressMonitor monitor, SolutionEntityItem item, MSBuildProject project)
		{
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x00057DEE File Offset: 0x00055FEE
		public virtual void SaveProject(IProgressMonitor monitor, SolutionEntityItem item, MSBuildProject project)
		{
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x00057DF0 File Offset: 0x00055FF0
		public virtual object GetService(Type t)
		{
			return null;
		}
	}
}
