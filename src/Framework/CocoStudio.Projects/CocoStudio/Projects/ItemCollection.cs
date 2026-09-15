using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CocoStudio.Projects
{
	public class ItemCollection<T> : ObservableCollection<T> where T : class
	{
		protected ItemCollection()
		{
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			this.OnAdd(item);
		}

		protected override void RemoveItem(int index)
		{
			T item = base[index];
			base.RemoveItem(index);
			this.OnRemove(item);
		}

		protected override void SetItem(int index, T item)
		{
			T item2 = base[index];
			base.SetItem(index, item);
			this.OnRemove(item2);
			this.OnAdd(item);
		}

		protected override void ClearItems()
		{
			List<T> list = new List<T>(this);
			base.ClearItems();
			foreach (T item in list)
			{
				this.OnRemove(item);
			}
		}

		protected virtual void OnRemove(T item)
		{
			throw new NotImplementedException();
		}

		protected virtual void OnAdd(T item)
		{
			throw new NotImplementedException();
		}
	}
}
