using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000117 RID: 279
	public class SolutionItemChangeEventArgs : SolutionItemEventArgs
	{
		// Token: 0x06000A79 RID: 2681 RVA: 0x000282DA File Offset: 0x000264DA
		public SolutionItemChangeEventArgs(SolutionItem item, Solution parentSolution, bool reloading) : base(item, parentSolution)
		{
			this.reloading = reloading;
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x000282EB File Offset: 0x000264EB
		public bool Reloading
		{
			get
			{
				return this.reloading;
			}
		}

		/// <summary>
		/// When Reloading is true, it returns the original solution item that is being reloaded
		/// </summary>
		/// <value>The replaced item.</value>
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x000282F3 File Offset: 0x000264F3
		// (set) Token: 0x06000A7C RID: 2684 RVA: 0x000282FB File Offset: 0x000264FB
		public SolutionItem ReplacedItem { get; internal set; }

		// Token: 0x04000329 RID: 809
		private bool reloading;
	}
}
