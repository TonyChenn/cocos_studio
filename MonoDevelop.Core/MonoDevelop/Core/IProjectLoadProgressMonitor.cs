using System;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Core
{
	// Token: 0x0200022A RID: 554
	public interface IProjectLoadProgressMonitor : IProgressMonitor, IDisposable
	{
		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060014BB RID: 5307
		// (set) Token: 0x060014BC RID: 5308
		Solution CurrentSolution { get; set; }

		// Token: 0x060014BD RID: 5309
		MigrationType ShouldMigrateProject();
	}
}
