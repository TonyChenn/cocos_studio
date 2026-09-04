using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a snapshot of the whole solution (multiple compilations).
	/// </summary>
	// Token: 0x02000071 RID: 113
	public interface ISolutionSnapshot
	{
		/// <summary>
		/// Gets the project content with the specified file name.
		/// Returns null if no such project exists in the solution.
		/// </summary>
		/// <remarks>
		/// This method is used by the <see cref="T:ICSharpCode.NRefactory.TypeSystem.ProjectReference" /> class.
		/// </remarks>
		// Token: 0x0600039E RID: 926
		IProjectContent GetProjectContent(string projectFileName);

		/// <summary>
		/// Gets the compilation for the specified project.
		/// The project must be a part of the solution (passed to the solution snapshot's constructor).
		/// </summary>
		// Token: 0x0600039F RID: 927
		ICompilation GetCompilation(IProjectContent project);
	}
}
