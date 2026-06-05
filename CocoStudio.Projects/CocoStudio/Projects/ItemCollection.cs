using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CocoStudio.Projects
{
	// Token: 0x02000020 RID: 32
	public class ItemCollection<T> : ObservableCollection<T> where T : class
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00003FBC File Offset: 0x000021BC
		protected ItemCollection()
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003FC4 File Offset: 0x000021C4
		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			this.OnAdd(item);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003FD8 File Offset: 0x000021D8
		protected override void RemoveItem(int index)
		{
			T item = base[index];
			base.RemoveItem(index);
			this.OnRemove(item);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003FFC File Offset: 0x000021FC
		protected override void SetItem(int index, T item)
		{
			T item2 = base[index];
			base.SetItem(index, item);
			this.OnRemove(item2);
			this.OnAdd(item);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004028 File Offset: 0x00002228
		protected override void ClearItems()
		{
			List<T> list = new List<T>(this);
			base.ClearItems();
			foreach (T item in list)
			{
				this.OnRemove(item);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004084 File Offset: 0x00002284
		protected virtual void OnRemove(T item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000408B File Offset: 0x0000228B
		protected virtual void OnAdd(T item)
		{
			throw new NotImplementedException();
		}
	}
}
