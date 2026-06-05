using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001D8 RID: 472
	public interface IMSBuildImportProvider
	{
		// Token: 0x06001202 RID: 4610
		void UpdateImports(SolutionEntityItem item, List<string> imports);
	}
}
