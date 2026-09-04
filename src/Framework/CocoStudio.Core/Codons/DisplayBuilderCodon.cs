using System;
using Mono.Addins;

namespace CocoStudio.Core.Codons
{
	// Token: 0x0200000A RID: 10
	public class DisplayBuilderCodon : TypeExtensionNode
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003584 File Offset: 0x00001784
		public object Binding
		{
			get
			{
				return base.GetInstance();
			}
		}
	}
}
