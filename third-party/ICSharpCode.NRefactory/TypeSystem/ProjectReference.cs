using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// References another project content in the same solution.
	/// Using the <see cref="T:ICSharpCode.NRefactory.TypeSystem.ProjectReference" /> class requires that you 
	/// </summary>
	// Token: 0x020000F7 RID: 247
	[Serializable]
	public class ProjectReference : IAssemblyReference
	{
		/// <summary>
		/// Creates a new reference to the specified project (must be part of the same solution).
		/// </summary>
		/// <param name="projectFileName">Full path to the file name. Must be identical to <see cref="P:ICSharpCode.NRefactory.TypeSystem.IProjectContent.ProjectFileName" /> of the target project; do not use a relative path.</param>
		// Token: 0x06000924 RID: 2340 RVA: 0x000188C5 File Offset: 0x000178C5
		public ProjectReference(string projectFileName)
		{
			this.projectFileName = projectFileName;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x000188D4 File Offset: 0x000178D4
		public IAssembly Resolve(ITypeResolveContext context)
		{
			ISolutionSnapshot solutionSnapshot = context.Compilation.SolutionSnapshot;
			IProjectContent projectContent = solutionSnapshot.GetProjectContent(this.projectFileName);
			if (projectContent != null)
			{
				return projectContent.Resolve(context);
			}
			return null;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00018906 File Offset: 0x00017906
		public override string ToString()
		{
			return string.Format("[ProjectReference {0}]", this.projectFileName);
		}

		// Token: 0x040002EB RID: 747
		private readonly string projectFileName;
	}
}
