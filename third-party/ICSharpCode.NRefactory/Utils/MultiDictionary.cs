using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// A dictionary that allows multiple pairs with the same key.
	/// </summary>
	// Token: 0x02000120 RID: 288
	public class MultiDictionary<TKey, TValue> : ILookup<TKey, TValue>, IEnumerable<IGrouping<TKey, TValue>>, IEnumerable
	{
		// Token: 0x06000A27 RID: 2599 RVA: 0x0001E3DD File Offset: 0x0001D3DD
		public MultiDictionary()
		{
			this.dict = new Dictionary<TKey, List<TValue>>();
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0001E3F0 File Offset: 0x0001D3F0
		public MultiDictionary(IEqualityComparer<TKey> comparer)
		{
			this.dict = new Dictionary<TKey, List<TValue>>(comparer);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0001E404 File Offset: 0x0001D404
		public void Add(TKey key, TValue value)
		{
			List<TValue> list;
			if (!this.dict.TryGetValue(key, out list))
			{
				list = new List<TValue>();
				this.dict.Add(key, list);
			}
			list.Add(value);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0001E43C File Offset: 0x0001D43C
		public bool Remove(TKey key, TValue value)
		{
			List<TValue> list;
			if (this.dict.TryGetValue(key, out list) && list.Remove(value))
			{
				if (list.Count == 0)
				{
					this.dict.Remove(key);
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes all entries with the specified key.
		/// </summary>
		/// <returns>Returns true if at least one entry was removed.</returns>
		// Token: 0x06000A2B RID: 2603 RVA: 0x0001E47A File Offset: 0x0001D47A
		public bool RemoveAll(TKey key)
		{
			return this.dict.Remove(key);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0001E488 File Offset: 0x0001D488
		public void Clear()
		{
			this.dict.Clear();
		}

		// Token: 0x170003E3 RID: 995
		public IList<TValue> this[TKey key]
		{
			get
			{
				List<TValue> result;
				if (this.dict.TryGetValue(key, out result))
				{
					return result;
				}
				return EmptyList<TValue>.Instance;
			}
		}

		/// <summary>
		/// Returns the number of different keys.
		/// </summary>
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0001E4BC File Offset: 0x0001D4BC
		public int Count
		{
			get
			{
				return this.dict.Count;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0001E4C9 File Offset: 0x0001D4C9
		public ICollection<TKey> Keys
		{
			get
			{
				return this.dict.Keys;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0001E4D9 File Offset: 0x0001D4D9
		public IEnumerable<TValue> Values
		{
			get
			{
				return this.dict.Values.SelectMany((List<TValue> list) => list);
			}
		}

		// Token: 0x170003E7 RID: 999
		IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key]
		{
			get
			{
				return this[key];
			}
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0001E511 File Offset: 0x0001D511
		bool ILookup<TKey, TValue>.Contains(TKey key)
		{
			return this.dict.ContainsKey(key);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0001E670 File Offset: 0x0001D670
		public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
		{
			foreach (KeyValuePair<TKey, List<TValue>> pair in this.dict)
			{
				KeyValuePair<TKey, List<TValue>> keyValuePair = pair;
				TKey key = keyValuePair.Key;
				KeyValuePair<TKey, List<TValue>> keyValuePair2 = pair;
				yield return new MultiDictionary<TKey, TValue>.Grouping(key, keyValuePair2.Value);
			}
			yield break;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0001E68C File Offset: 0x0001D68C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000377 RID: 887
		private readonly Dictionary<TKey, List<TValue>> dict;

		// Token: 0x02000121 RID: 289
		private sealed class Grouping : IGrouping<TKey, TValue>, IEnumerable<TValue>, IEnumerable
		{
			// Token: 0x06000A36 RID: 2614 RVA: 0x0001E694 File Offset: 0x0001D694
			public Grouping(TKey key, List<TValue> values)
			{
				this.key = key;
				this.values = values;
			}

			// Token: 0x170003E8 RID: 1000
			// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0001E6AA File Offset: 0x0001D6AA
			public TKey Key
			{
				get
				{
					return this.key;
				}
			}

			// Token: 0x06000A38 RID: 2616 RVA: 0x0001E6B2 File Offset: 0x0001D6B2
			public IEnumerator<TValue> GetEnumerator()
			{
				return this.values.GetEnumerator();
			}

			// Token: 0x06000A39 RID: 2617 RVA: 0x0001E6C4 File Offset: 0x0001D6C4
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.values.GetEnumerator();
			}

			// Token: 0x04000379 RID: 889
			private readonly TKey key;

			// Token: 0x0400037A RID: 890
			private readonly List<TValue> values;
		}
	}
}
