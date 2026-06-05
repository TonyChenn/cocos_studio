using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200011A RID: 282
	public class SolutionItemModifiedEventInfo : SolutionItemEventArgs
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x00028319 File Offset: 0x00026519
		public SolutionItemModifiedEventInfo(SolutionItem item, string hint) : base(item)
		{
			this.hint = hint;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x00028329 File Offset: 0x00026529
		public string Hint
		{
			get
			{
				return this.hint;
			}
		}

		// Token: 0x0400032B RID: 811
		private string hint;
	}
}
