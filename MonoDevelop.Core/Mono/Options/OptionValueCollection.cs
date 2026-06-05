using System;
using System.Collections;
using System.Collections.Generic;

namespace Mono.Options
{
	// Token: 0x020000EF RID: 239
	public class OptionValueCollection : IList, ICollection, IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable
	{
		// Token: 0x06000859 RID: 2137 RVA: 0x00021683 File Offset: 0x0001F883
		internal OptionValueCollection(OptionContext c)
		{
			this.c = c;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0002169D File Offset: 0x0001F89D
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this.values).CopyTo(array, index);
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x000216AC File Offset: 0x0001F8AC
		bool ICollection.IsSynchronized
		{
			get
			{
				return ((ICollection)this.values).IsSynchronized;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x000216B9 File Offset: 0x0001F8B9
		object ICollection.SyncRoot
		{
			get
			{
				return ((ICollection)this.values).SyncRoot;
			}
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x000216C6 File Offset: 0x0001F8C6
		public void Add(string item)
		{
			this.values.Add(item);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000216D4 File Offset: 0x0001F8D4
		public void Clear()
		{
			this.values.Clear();
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x000216E1 File Offset: 0x0001F8E1
		public bool Contains(string item)
		{
			return this.values.Contains(item);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000216EF File Offset: 0x0001F8EF
		public void CopyTo(string[] array, int arrayIndex)
		{
			this.values.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000216FE File Offset: 0x0001F8FE
		public bool Remove(string item)
		{
			return this.values.Remove(item);
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0002170C File Offset: 0x0001F90C
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00021719 File Offset: 0x0001F919
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0002171C File Offset: 0x0001F91C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0002172E File Offset: 0x0001F92E
		public IEnumerator<string> GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00021740 File Offset: 0x0001F940
		int IList.Add(object value)
		{
			return ((IList)this.values).Add(value);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0002174E File Offset: 0x0001F94E
		bool IList.Contains(object value)
		{
			return ((IList)this.values).Contains(value);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0002175C File Offset: 0x0001F95C
		int IList.IndexOf(object value)
		{
			return ((IList)this.values).IndexOf(value);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0002176A File Offset: 0x0001F96A
		void IList.Insert(int index, object value)
		{
			((IList)this.values).Insert(index, value);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00021779 File Offset: 0x0001F979
		void IList.Remove(object value)
		{
			((IList)this.values).Remove(value);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00021787 File Offset: 0x0001F987
		void IList.RemoveAt(int index)
		{
			((IList)this.values).RemoveAt(index);
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x00021795 File Offset: 0x0001F995
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001C8 RID: 456
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				((IList)this.values)[index] = value;
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x000217B0 File Offset: 0x0001F9B0
		public int IndexOf(string item)
		{
			return this.values.IndexOf(item);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000217BE File Offset: 0x0001F9BE
		public void Insert(int index, string item)
		{
			this.values.Insert(index, item);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x000217CD File Offset: 0x0001F9CD
		public void RemoveAt(int index)
		{
			this.values.RemoveAt(index);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000217DC File Offset: 0x0001F9DC
		private void AssertValid(int index)
		{
			if (this.c.Option == null)
			{
				throw new InvalidOperationException("OptionContext.Option is null.");
			}
			if (index >= this.c.Option.MaxValueCount)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (this.c.Option.OptionValueType == OptionValueType.Required && index >= this.values.Count)
			{
				throw new OptionException(string.Format(this.c.OptionSet.MessageLocalizer("Missing required value for option '{0}'."), this.c.OptionName), this.c.OptionName);
			}
		}

		// Token: 0x170001C9 RID: 457
		public string this[int index]
		{
			get
			{
				this.AssertValid(index);
				if (index < this.values.Count)
				{
					return this.values[index];
				}
				return null;
			}
			set
			{
				this.values[index] = value;
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000218AF File Offset: 0x0001FAAF
		public List<string> ToList()
		{
			return new List<string>(this.values);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000218BC File Offset: 0x0001FABC
		public string[] ToArray()
		{
			return this.values.ToArray();
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000218C9 File Offset: 0x0001FAC9
		public override string ToString()
		{
			return string.Join(", ", this.values.ToArray());
		}

		// Token: 0x040002A9 RID: 681
		private List<string> values = new List<string>();

		// Token: 0x040002AA RID: 682
		private OptionContext c;
	}
}
