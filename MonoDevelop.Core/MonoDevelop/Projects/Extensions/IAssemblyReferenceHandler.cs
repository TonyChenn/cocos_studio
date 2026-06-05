using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Extensions
{
	/// <summary>
	/// This interface can be implemented by a ISolutionItemHandler class to provide
	/// a the list of assembly references for a project. It must not include project references.
	/// </summary>
	// Token: 0x020001A7 RID: 423
	public interface IAssemblyReferenceHandler
	{
		// Token: 0x06000FF9 RID: 4089
		IEnumerable<string> GetAssemblyReferences(ConfigurationSelector configuration);
	}
}
