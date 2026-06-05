using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x0200018B RID: 395
	public class CompiledAssemblyExtension : ProjectServiceExtension
	{
		// Token: 0x06000F63 RID: 3939 RVA: 0x0003A022 File Offset: 0x00038222
		public override bool IsSolutionItemFile(string fileName)
		{
			return fileName.ToLower().EndsWith(".exe") || fileName.ToLower().EndsWith(".dll") || base.IsSolutionItemFile(fileName);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0003A054 File Offset: 0x00038254
		protected override SolutionEntityItem LoadSolutionItem(IProgressMonitor monitor, string fileName)
		{
			if (fileName.ToLower().EndsWith(".exe") || fileName.ToLower().EndsWith(".dll"))
			{
				CompiledAssemblyProject compiledAssemblyProject = new CompiledAssemblyProject();
				compiledAssemblyProject.LoadFrom(fileName);
				return compiledAssemblyProject;
			}
			return base.LoadSolutionItem(monitor, fileName);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0003A0A1 File Offset: 0x000382A1
		public override void Save(IProgressMonitor monitor, SolutionEntityItem item)
		{
			base.Save(monitor, item);
		}
	}
}
