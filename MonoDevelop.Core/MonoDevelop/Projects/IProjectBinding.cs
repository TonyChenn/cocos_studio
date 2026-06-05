using System;
using System.Xml;

namespace MonoDevelop.Projects
{
	// Token: 0x0200011F RID: 287
	public interface IProjectBinding
	{
		/// <remarks>
		/// Returns the project type name
		/// </remarks>
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000AA7 RID: 2727
		string Name { get; }

		/// <remarks>
		/// Creates a Project out of the given ProjetCreateInformation object.
		/// Each project binding must provide a representation of the project
		/// it 'controls'.
		/// </remarks>
		// Token: 0x06000AA8 RID: 2728
		Project CreateProject(ProjectCreateInformation info, XmlElement projectOptions);

		/// <remarks>
		/// Creates a Project for a single source file. If the file is not
		/// valid for this project type, it must return null.
		/// </remarks>
		// Token: 0x06000AA9 RID: 2729
		Project CreateSingleFileProject(string sourceFile);

		// Token: 0x06000AAA RID: 2730
		bool CanCreateSingleFileProject(string sourceFile);
	}
}
