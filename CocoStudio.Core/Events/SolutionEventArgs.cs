using System;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	// Token: 0x02000024 RID: 36
	public class SolutionEventArgs : WorkspaceItemEventArgs
	{
		// Token: 0x0600015B RID: 347 RVA: 0x00006282 File Offset: 0x00004482
		public SolutionEventArgs(Solution sol) : base(sol)
		{
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00006290 File Offset: 0x00004490
		public Solution Solution
		{
			get
			{
				return (Solution)base.Item;
			}
		}
	}
}
