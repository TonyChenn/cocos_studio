using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonoDevelop.Projects
{
	// Token: 0x0200013D RID: 317
	public class ItemCollection<T> : Collection<T>
	{
		// Token: 0x06000BE8 RID: 3048 RVA: 0x0002C5E4 File Offset: 0x0002A7E4
		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			this.OnItemAdded(item);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0002C5F8 File Offset: 0x0002A7F8
		protected override void RemoveItem(int index)
		{
			T item = base[index];
			base.RemoveItem(index);
			this.OnItemRemoved(item);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0002C61C File Offset: 0x0002A81C
		protected override void SetItem(int index, T item)
		{
			T item2 = base[index];
			base.SetItem(index, item);
			this.OnItemRemoved(item2);
			this.OnItemAdded(item);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0002C648 File Offset: 0x0002A848
		protected override void ClearItems()
		{
			List<T> list = new List<T>(this);
			base.ClearItems();
			foreach (T item in list)
			{
				this.OnItemRemoved(item);
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0002C6A4 File Offset: 0x0002A8A4
		protected virtual void OnItemAdded(T item)
		{
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0002C6A6 File Offset: 0x0002A8A6
		protected virtual void OnItemRemoved(T item)
		{
		}
	}
}
