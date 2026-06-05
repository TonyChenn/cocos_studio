using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200016D RID: 365
	public class SolutionEventArgs : WorkspaceItemEventArgs
	{
		// Token: 0x06000E60 RID: 3680 RVA: 0x000353AD File Offset: 0x000335AD
		public SolutionEventArgs(Solution sol) : base(sol)
		{
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x000353B6 File Offset: 0x000335B6
		public Solution Solution
		{
			get
			{
				return (Solution)base.Item;
			}
		}
	}
}
