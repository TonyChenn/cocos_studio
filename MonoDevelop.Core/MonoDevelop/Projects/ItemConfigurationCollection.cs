using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoDevelop.Projects
{
	// Token: 0x02000146 RID: 326
	public class ItemConfigurationCollection<T> : ItemCollection<T>, IItemConfigurationCollection, ICollection<ItemConfiguration>, IEnumerable<ItemConfiguration>, IEnumerable where T : ItemConfiguration
	{
		// Token: 0x170002A3 RID: 675
		public T this[string name]
		{
			get
			{
				foreach (T result in this)
				{
					if (result.Id == name)
					{
						return result;
					}
				}
				return default(T);
			}
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x0002D408 File Offset: 0x0002B608
		public void Remove(string name)
		{
			for (int i = 0; i < base.Count; i++)
			{
				T t = base[i];
				if (t.Id == name)
				{
					base.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		ItemConfiguration IItemConfigurationCollection.this[string name]
		{
			get
			{
				return this[name];
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0002D459 File Offset: 0x0002B659
		bool ICollection<ItemConfiguration>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0002D58C File Offset: 0x0002B78C
		IEnumerator<ItemConfiguration> IEnumerable<ItemConfiguration>.GetEnumerator()
		{
			foreach (!0 ! in this)
			{
				ItemConfiguration item = !;
				yield return item;
			}
			yield break;
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x0002D5A8 File Offset: 0x0002B7A8
		void ICollection<ItemConfiguration>.Add(ItemConfiguration item)
		{
			base.Add((T)((object)item));
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0002D5B6 File Offset: 0x0002B7B6
		void ICollection<ItemConfiguration>.Clear()
		{
			base.Clear();
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0002D5BE File Offset: 0x0002B7BE
		bool ICollection<ItemConfiguration>.Contains(ItemConfiguration item)
		{
			return base.Contains((T)((object)item));
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0002D5CC File Offset: 0x0002B7CC
		void ICollection<ItemConfiguration>.CopyTo(ItemConfiguration[] array, int arrayIndex)
		{
			for (int i = 0; i < base.Count; i++)
			{
				array[arrayIndex + i] = base[i];
			}
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x0002D5FB File Offset: 0x0002B7FB
		bool ICollection<ItemConfiguration>.Remove(ItemConfiguration item)
		{
			return base.Remove((T)((object)item));
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0002D609 File Offset: 0x0002B809
		protected override void OnItemAdded(T conf)
		{
			if (this.ConfigurationAdded != null)
			{
				this.ConfigurationAdded(this, new ConfigurationEventArgs(null, conf));
			}
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0002D62B File Offset: 0x0002B82B
		protected override void OnItemRemoved(T conf)
		{
			if (this.ConfigurationRemoved != null)
			{
				this.ConfigurationRemoved(this, new ConfigurationEventArgs(null, conf));
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06000C3E RID: 3134 RVA: 0x0002D650 File Offset: 0x0002B850
		// (remove) Token: 0x06000C3F RID: 3135 RVA: 0x0002D688 File Offset: 0x0002B888
		public event ConfigurationEventHandler ConfigurationAdded;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06000C40 RID: 3136 RVA: 0x0002D6C0 File Offset: 0x0002B8C0
		// (remove) Token: 0x06000C41 RID: 3137 RVA: 0x0002D6F8 File Offset: 0x0002B8F8
		public event ConfigurationEventHandler ConfigurationRemoved;
	}
}
