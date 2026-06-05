using System;
using System.IO;
using Mono.Addins;
using MonoDevelop.Projects.Policies;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x020001A0 RID: 416
	internal class PolicyResourceNode : ExtensionNode
	{
		// Token: 0x06000FDF RID: 4063 RVA: 0x0003ADAB File Offset: 0x00038FAB
		public StreamReader GetStream()
		{
			return new StreamReader(base.Addin.GetResource(this.resource, true));
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0003ADC4 File Offset: 0x00038FC4
		// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x0003ADCC File Offset: 0x00038FCC
		public PolicyKey[] AddedKeys { get; set; }

		// Token: 0x0400049E RID: 1182
		[NodeAttribute]
		protected string resource;
	}
}
