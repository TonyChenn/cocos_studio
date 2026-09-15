using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// References another project content in the same solution.
	/// Using the <see cref="T:ICSharpCode.NRefactory.TypeSystem.ProjectReference" /> class requires that you 
	/// </summary>
	[Serializable]
	public class ProjectReference : IAssemblyReference
	{
		/// <summary>
		/// Creates a new reference to the specified project (must be part of the same solution).
		/// </summary>
		/// <param name="projectFileName">Full path to the file name. Must be identical to <see cref="P:ICSharpCode.NRefactory.TypeSystem.IProjectContent.ProjectFileName" /> of the target project; do not use a relative path.</param>
		public ProjectReference(string projectFileName)
		{
			this.projectFileName = projectFileName;
		}

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

		public override string ToString()
		{
			return string.Format("[ProjectReference {0}]", this.projectFileName);
		}

		private readonly string projectFileName;
	}
}
