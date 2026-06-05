using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200011C RID: 284
	public class SolutionItemRenamedEventArgs : SolutionItemEventArgs
	{
		// Token: 0x06000A88 RID: 2696 RVA: 0x00028331 File Offset: 0x00026531
		public SolutionItemRenamedEventArgs(SolutionItem node, string oldName, string newName) : base(node)
		{
			this.oldName = oldName;
			this.newName = newName;
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00028348 File Offset: 0x00026548
		public string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00028350 File Offset: 0x00026550
		public string NewName
		{
			get
			{
				return this.newName;
			}
		}

		// Token: 0x0400032C RID: 812
		private string oldName;

		// Token: 0x0400032D RID: 813
		private string newName;
	}
}
