using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000170 RID: 368
	public class SolutionFolderItemCollection : ItemCollection<SolutionItem>
	{
		// Token: 0x06000E69 RID: 3689 RVA: 0x00035782 File Offset: 0x00033982
		public SolutionFolderItemCollection()
		{
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0003578A File Offset: 0x0003398A
		internal SolutionFolderItemCollection(SolutionFolder parentFolder)
		{
			this.parentFolder = parentFolder;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0003579C File Offset: 0x0003399C
		internal void Replace(SolutionItem item, SolutionItem newItem)
		{
			int index = base.IndexOf(item);
			base.Items[index] = newItem;
			newItem.ParentFolder = this.parentFolder;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000357CC File Offset: 0x000339CC
		protected override void OnItemAdded(SolutionItem item)
		{
			if (this.parentFolder != null)
			{
				if (item.ParentFolder != null && item.ParentSolution != null)
				{
					if (item.ParentSolution == this.parentFolder.ParentSolution)
					{
						SolutionFolder solutionFolder = item.ParentFolder;
						item.ParentFolder = null;
						solutionFolder.Items.InternalRemove(item);
						solutionFolder.NotifyItemRemoved(item, false);
						item.ParentFolder = this.parentFolder;
						this.parentFolder.NotifyItemAdded(item, false);
						return;
					}
					item.ParentFolder.Items.Remove(item);
				}
				item.ParentFolder = this.parentFolder;
				this.parentFolder.NotifyItemAdded(item, true);
			}
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00035870 File Offset: 0x00033A70
		protected override void OnItemRemoved(SolutionItem item)
		{
			if (this.parentFolder != null)
			{
				item.ParentFolder = null;
				this.parentFolder.NotifyItemRemoved(item, true);
			}
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0003588E File Offset: 0x00033A8E
		internal void InternalAdd(SolutionItem item)
		{
			base.Items.Add(item);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0003589C File Offset: 0x00033A9C
		internal void InternalRemove(SolutionItem item)
		{
			base.Items.Remove(item);
		}

		// Token: 0x04000423 RID: 1059
		private SolutionFolder parentFolder;
	}
}
