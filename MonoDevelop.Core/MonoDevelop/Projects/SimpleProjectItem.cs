using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200017F RID: 383
	public class SimpleProjectItem : ProjectItem
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x00038E2E File Offset: 0x0003702E
		// (set) Token: 0x06000F25 RID: 3877 RVA: 0x00038E36 File Offset: 0x00037036
		[ItemProperty("Include")]
		public string Include { get; set; }
	}
}
