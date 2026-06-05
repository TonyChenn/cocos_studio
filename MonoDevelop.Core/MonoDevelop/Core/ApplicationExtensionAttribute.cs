using System;
using Mono.Addins;

namespace MonoDevelop.Core
{
	// Token: 0x02000220 RID: 544
	public class ApplicationExtensionAttribute : CustomExtensionAttribute
	{
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x000545CC File Offset: 0x000527CC
		// (set) Token: 0x0600146D RID: 5229 RVA: 0x000545D4 File Offset: 0x000527D4
		[NodeAttribute("description")]
		public string Description { get; set; }
	}
}
