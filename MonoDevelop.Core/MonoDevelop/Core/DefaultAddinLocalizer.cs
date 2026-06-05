using System;
using Mono.Addins.Localization;

namespace MonoDevelop.Core
{
	// Token: 0x02000057 RID: 87
	internal class DefaultAddinLocalizer : IAddinLocalizer
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0000B08C File Offset: 0x0000928C
		public string GetString(string id)
		{
			return GettextCatalog.GetString(id);
		}
	}
}
