using System;
using System.Collections.ObjectModel;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000168 RID: 360
	public class SolutionFolderFileCollection : Collection<FilePath>
	{
		// Token: 0x06000E4D RID: 3661 RVA: 0x000350A5 File Offset: 0x000332A5
		internal SolutionFolderFileCollection(SolutionFolder parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x000350B4 File Offset: 0x000332B4
		protected override void ClearItems()
		{
			FilePath[] array = new FilePath[base.Count];
			base.CopyTo(array, 0);
			base.ClearItems();
			this.parent.NotifyFilesRemoved(array);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x000350E8 File Offset: 0x000332E8
		protected override void InsertItem(int index, FilePath item)
		{
			base.InsertItem(index, item);
			this.parent.NotifyFilesAdded(new FilePath[]
			{
				item
			});
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00035120 File Offset: 0x00033320
		protected override void RemoveItem(int index)
		{
			FilePath filePath = base[index];
			base.RemoveItem(index);
			this.parent.NotifyFilesRemoved(new FilePath[]
			{
				filePath
			});
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0003515C File Offset: 0x0003335C
		protected override void SetItem(int index, FilePath item)
		{
			FilePath filePath = base[index];
			base.SetItem(index, item);
			this.parent.NotifyFilesRemoved(new FilePath[]
			{
				filePath
			});
			this.parent.NotifyFilesAdded(new FilePath[]
			{
				item
			});
		}

		// Token: 0x0400041E RID: 1054
		private SolutionFolder parent;
	}
}
