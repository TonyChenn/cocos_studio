using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001CD RID: 461
	public interface MSBuildPropertySet
	{
		// Token: 0x060011A1 RID: 4513
		MSBuildProperty GetProperty(string name);

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060011A2 RID: 4514
		IEnumerable<MSBuildProperty> Properties { get; }

		// Token: 0x060011A3 RID: 4515
		MSBuildProperty SetPropertyValue(string name, string value, bool preserveExistingCase, bool isXml = false);

		// Token: 0x060011A4 RID: 4516
		string GetPropertyValue(string name, bool isXml = false);

		// Token: 0x060011A5 RID: 4517
		bool RemoveProperty(string name);

		// Token: 0x060011A6 RID: 4518
		void RemoveAllProperties();

		// Token: 0x060011A7 RID: 4519
		void UnMerge(MSBuildPropertySet baseGrp, ISet<string> propertiesToExclude);
	}
}
