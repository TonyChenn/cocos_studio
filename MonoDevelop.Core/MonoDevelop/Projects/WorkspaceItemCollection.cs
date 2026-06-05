using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonoDevelop.Projects
{
	// Token: 0x0200016B RID: 363
	public class WorkspaceItemCollection : Collection<WorkspaceItem>
	{
		// Token: 0x06000E55 RID: 3669 RVA: 0x000351D0 File Offset: 0x000333D0
		public WorkspaceItemCollection()
		{
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x000351D8 File Offset: 0x000333D8
		public WorkspaceItemCollection(IList<WorkspaceItem> list) : base(list)
		{
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x000351E1 File Offset: 0x000333E1
		internal WorkspaceItemCollection(Workspace parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x000351F0 File Offset: 0x000333F0
		public WorkspaceItem[] ToArray()
		{
			WorkspaceItem[] array = new WorkspaceItem[base.Count];
			base.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00035214 File Offset: 0x00033414
		internal void Replace(WorkspaceItem item, WorkspaceItem newItem)
		{
			int index = base.IndexOf(item);
			base.Items[index] = newItem;
			if (this.parent != null)
			{
				item.ParentWorkspace = null;
				newItem.ParentWorkspace = this.parent;
			}
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00035254 File Offset: 0x00033454
		protected override void ClearItems()
		{
			if (this.parent != null)
			{
				List<WorkspaceItem> list = new List<WorkspaceItem>(this);
				using (List<WorkspaceItem>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						WorkspaceItem workspaceItem = enumerator.Current;
						workspaceItem.ParentWorkspace = null;
						this.parent.NotifyItemRemoved(new WorkspaceItemChangeEventArgs(workspaceItem, false));
					}
					return;
				}
			}
			base.ClearItems();
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x000352CC File Offset: 0x000334CC
		protected override void InsertItem(int index, WorkspaceItem item)
		{
			base.InsertItem(index, item);
			if (this.parent != null)
			{
				item.ParentWorkspace = this.parent;
				this.parent.NotifyItemAdded(new WorkspaceItemChangeEventArgs(item, false));
			}
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x000352FC File Offset: 0x000334FC
		protected override void RemoveItem(int index)
		{
			WorkspaceItem workspaceItem = base[index];
			base.RemoveItem(index);
			if (this.parent != null)
			{
				workspaceItem.ParentWorkspace = this.parent;
				this.parent.NotifyItemRemoved(new WorkspaceItemChangeEventArgs(workspaceItem, false));
			}
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00035340 File Offset: 0x00033540
		protected override void SetItem(int index, WorkspaceItem item)
		{
			WorkspaceItem item2 = base[index];
			base.SetItem(index, item);
			if (this.parent != null)
			{
				item.ParentWorkspace = this.parent;
				this.parent.NotifyItemRemoved(new WorkspaceItemChangeEventArgs(item2, false));
				this.parent.NotifyItemAdded(new WorkspaceItemChangeEventArgs(item, false));
			}
		}

		// Token: 0x04000420 RID: 1056
		private Workspace parent;
	}
}
