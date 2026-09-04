using System;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x0200007A RID: 122
	[DataItem("ResourceGroup")]
	public class SolutionItemCollection : ItemCollection<SolutionEntityItem>
	{
		// Token: 0x0600039C RID: 924 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
		public SolutionItemCollection()
		{
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
		public SolutionItemCollection(SolutionFolder parentSolutionFolder)
		{
			this.parentSolutionFolder = parentSolutionFolder;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000CE07 File Offset: 0x0000B007
		protected override void OnAdd(SolutionEntityItem item)
		{
			if (this.parentSolutionFolder != null)
			{
				item.ParentFolder = this.parentSolutionFolder;
				item.ParentSolution = this.parentSolutionFolder.ParentSolution;
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000CE2E File Offset: 0x0000B02E
		protected override void OnRemove(SolutionEntityItem item)
		{
			item.ParentFolder = null;
			item.ParentSolution = null;
		}

		// Token: 0x040000EB RID: 235
		private SolutionFolder parentSolutionFolder;
	}
}
