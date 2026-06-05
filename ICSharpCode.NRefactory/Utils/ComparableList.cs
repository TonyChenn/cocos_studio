using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// A list that can be compared to other ComparableLists for equality.
	/// Can not be used to store null values.
	/// </summary>
	// Token: 0x02000145 RID: 325
	public sealed class ComparableList<T> : IList<!0>, ICollection<!0>, IEnumerable<!0>, IEnumerable, IEquatable<ComparableList<T>>
	{
		// Token: 0x06000B1A RID: 2842 RVA: 0x00022474 File Offset: 0x00021474
		public ComparableList()
		{
			this.elements = new List<T>();
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00022487 File Offset: 0x00021487
		public ComparableList(IEnumerable<T> values)
		{
			this.elements = new List<T>(values);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0002249B File Offset: 0x0002149B
		public int IndexOf(T item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.elements.IndexOf(item);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x000224BC File Offset: 0x000214BC
		public void Insert(int index, T item)
		{
			this.elements.Insert(index, item);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x000224CB File Offset: 0x000214CB
		public void RemoveAt(int index)
		{
			this.elements.RemoveAt(index);
		}

		// Token: 0x17000425 RID: 1061
		public T this[int index]
		{
			get
			{
				return this.elements[index];
			}
			set
			{
				this.elements[index] = value;
			}
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x000224F6 File Offset: 0x000214F6
		public void Add(T item)
		{
			this.elements.Add(item);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00022504 File Offset: 0x00021504
		public void Clear()
		{
			this.elements.Clear();
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00022511 File Offset: 0x00021511
		public bool Contains(T item)
		{
			return this.elements.Contains(item);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0002251F File Offset: 0x0002151F
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.elements.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0002252E File Offset: 0x0002152E
		public bool Remove(T item)
		{
			return this.elements.Remove(item);
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0002253C File Offset: 0x0002153C
		public int Count
		{
			get
			{
				return this.elements.Count;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00022549 File Offset: 0x00021549
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0002254C File Offset: 0x0002154C
		public IEnumerator<T> GetEnumerator()
		{
			return this.elements.GetEnumerator();
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0002255E File Offset: 0x0002155E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00022566 File Offset: 0x00021566
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ComparableList<T>);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00022574 File Offset: 0x00021574
		public bool Equals(ComparableList<T> obj)
		{
			if (obj == null || this.Count != obj.Count)
			{
				return false;
			}
			for (int i = 0; i < this.Count; i++)
			{
				T t = this[i];
				if (!t.Equals(obj[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x000225D4 File Offset: 0x000215D4
		public override int GetHashCode()
		{
			int num = 19;
			foreach (T t in this)
			{
				num *= 31;
				num += t.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00022630 File Offset: 0x00021630
		public static bool operator ==(ComparableList<T> item1, ComparableList<T> item2)
		{
			if (object.ReferenceEquals(item1, null))
			{
				return object.ReferenceEquals(item2, null);
			}
			return item1.Equals(item2);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0002264A File Offset: 0x0002164A
		public static bool operator !=(ComparableList<T> item1, ComparableList<T> item2)
		{
			return !(item1 == item2);
		}

		// Token: 0x040003E5 RID: 997
		private List<T> elements;
	}
}
