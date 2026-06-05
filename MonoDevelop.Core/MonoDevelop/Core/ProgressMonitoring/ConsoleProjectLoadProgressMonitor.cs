using System;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x0200022C RID: 556
	public class ConsoleProjectLoadProgressMonitor : WrappedProgressMonitor, IProjectLoadProgressMonitor, IProgressMonitor, IDisposable
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060014D0 RID: 5328 RVA: 0x0005587F File Offset: 0x00053A7F
		// (set) Token: 0x060014D1 RID: 5329 RVA: 0x00055887 File Offset: 0x00053A87
		public Solution CurrentSolution { get; set; }

		// Token: 0x060014D2 RID: 5330 RVA: 0x00055890 File Offset: 0x00053A90
		public ConsoleProjectLoadProgressMonitor(IProgressMonitor monitor) : base(monitor)
		{
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00055899 File Offset: 0x00053A99
		protected override void Dispose(bool disposing)
		{
			this.CurrentSolution = null;
			base.Dispose(disposing);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x000558A9 File Offset: 0x00053AA9
		public MigrationType ShouldMigrateProject()
		{
			Console.WriteLine("Warning: One or more projects in this solution cannot be ");
			Console.WriteLine("compiled unless they are migrated to a newer format. Please ");
			Console.WriteLine("open the solution in an IDE and migrate the project so that ");
			Console.WriteLine("the solution can be compiled.");
			return MigrationType.Ignore;
		}
	}
}
