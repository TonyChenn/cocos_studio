using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000BE RID: 190
	internal class NotSupportedFrameworkBackend : TargetFrameworkBackend
	{
		// Token: 0x0600067D RID: 1661 RVA: 0x00019026 File Offset: 0x00017226
		public override bool SupportsRuntime(TargetRuntime runtime)
		{
			return false;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00019029 File Offset: 0x00017229
		public override IEnumerable<string> GetFrameworkFolders()
		{
			return null;
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0001902C File Offset: 0x0001722C
		public override bool IsInstalled
		{
			get
			{
				return false;
			}
		}
	}
}
