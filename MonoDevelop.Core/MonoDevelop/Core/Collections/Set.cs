using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoDevelop.Core.Collections
{
	// Token: 0x02000091 RID: 145
	public class Set<T> : ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x060004B1 RID: 1201 RVA: 0x0001099A File Offset: 0x0000EB9A
		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable)this.dict.Keys).GetEnumerator();
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000109AC File Offset: 0x0000EBAC
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.dict.Keys.GetEnumerator();
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000109C3 File Offset: 0x0000EBC3
		public bool Add(T item)
		{
			if (!this.dict.ContainsKey(item))
			{
				this.dict[item] = this;
				return true;
			}
			return false;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000109E3 File Offset: 0x0000EBE3
		void ICollection<!0>.Add(T item)
		{
			this.Add(item);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000109ED File Offset: 0x0000EBED
		public void Clear()
		{
			this.dict.Clear();
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000109FA File Offset: 0x0000EBFA
		public bool Contains(T item)
		{
			return this.dict.ContainsKey(item);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00010A08 File Offset: 0x0000EC08
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.dict.Keys.CopyTo(array, arrayIndex);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00010A1C File Offset: 0x0000EC1C
		public bool Remove(T item)
		{
			return this.dict.Remove(item);
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00010A2A File Offset: 0x0000EC2A
		public int Count
		{
			get
			{
				return this.dict.Count;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x00010A37 File Offset: 0x0000EC37
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400018F RID: 399
		private Dictionary<T, Set<T>> dict = new Dictionary<T, Set<T>>();
	}
}
