using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001BB RID: 443
	internal class MSBuildElementOrder : Dictionary<string, int>
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x00041E8C File Offset: 0x0004008C
		public MSBuildElementOrder(params string[] elements)
		{
			for (int i = 0; i < elements.Length; i++)
			{
				base[elements[i]] = i;
			}
		}
	}
}
