using System;
using Mono.Addins;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x0200021D RID: 541
	public class PolicyTypeAttribute : CustomExtensionAttribute
	{
		// Token: 0x06001464 RID: 5220 RVA: 0x00054471 File Offset: 0x00052671
		public PolicyTypeAttribute()
		{
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x00054479 File Offset: 0x00052679
		public PolicyTypeAttribute([NodeAttribute("description")] string description)
		{
			this.Description = description;
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x00054488 File Offset: 0x00052688
		// (set) Token: 0x06001467 RID: 5223 RVA: 0x00054490 File Offset: 0x00052690
		[NodeAttribute("description")]
		public string Description { get; set; }
	}
}
