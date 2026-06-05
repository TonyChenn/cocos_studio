using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x0200026A RID: 618
	public interface IMSBuildGlobalPropertyProvider
	{
		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06001659 RID: 5721
		// (remove) Token: 0x0600165A RID: 5722
		event EventHandler GlobalPropertiesChanged;

		// Token: 0x0600165B RID: 5723
		IDictionary<string, string> GetGlobalProperties();
	}
}
