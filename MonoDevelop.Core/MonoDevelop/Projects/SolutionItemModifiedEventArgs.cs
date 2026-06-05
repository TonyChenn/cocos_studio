using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000119 RID: 281
	public class SolutionItemModifiedEventArgs : EventArgsChain<SolutionItemModifiedEventInfo>
	{
		// Token: 0x06000A81 RID: 2689 RVA: 0x00028304 File Offset: 0x00026504
		public SolutionItemModifiedEventArgs(SolutionItem item, string hint)
		{
			base.Add(new SolutionItemModifiedEventInfo(item, hint));
		}
	}
}
