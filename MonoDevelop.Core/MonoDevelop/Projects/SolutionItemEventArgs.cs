using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000115 RID: 277
	public class SolutionItemEventArgs : EventArgs
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00028296 File Offset: 0x00026496
		public SolutionItem SolutionItem
		{
			get
			{
				return this.entry;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0002829E File Offset: 0x0002649E
		public Solution Solution
		{
			get
			{
				return this.solution ?? this.entry.ParentSolution;
			}
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x000282B5 File Offset: 0x000264B5
		public SolutionItemEventArgs(SolutionItem entry)
		{
			this.entry = entry;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x000282C4 File Offset: 0x000264C4
		public SolutionItemEventArgs(SolutionItem entry, Solution solution)
		{
			this.solution = solution;
			this.entry = entry;
		}

		// Token: 0x04000327 RID: 807
		private SolutionItem entry;

		// Token: 0x04000328 RID: 808
		private Solution solution;
	}
}
