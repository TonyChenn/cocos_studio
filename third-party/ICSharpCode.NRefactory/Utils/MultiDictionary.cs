using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// A dictionary that allows multiple pairs with the same key.
	/// </summary>
	public class MultiDictionary<TKey, TValue> : ILookup<TKey, TValue>, IEnumerable<IGrouping<TKey, TValue>>, IEnumerable
	{
		public MultiDictionary()
		{
			this.dict = new Dictionary<TKey, List<TValue>>();
		}

		public MultiDictionary(IEqualityComparer<TKey> comparer)
		{
			this.dict = new Dictionary<TKey, List<TValue>>(comparer);
		}

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
		public bool RemoveAll(TKey key)
		{
			return this.dict.Remove(key);
		}

		public void Clear()
		{
			this.dict.Clear();
		}

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
		public int Count
		{
			get
			{
				return this.dict.Count;
			}
		}

		public ICollection<TKey> Keys
		{
			get
			{
				return this.dict.Keys;
			}
		}

		public IEnumerable<TValue> Values
		{
			get
			{
				return this.dict.Values.SelectMany((List<TValue> list) => list);
			}
		}

		IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key]
		{
			get
			{
				return this[key];
			}
		}

		bool ILookup<TKey, TValue>.Contains(TKey key)
		{
			return this.dict.ContainsKey(key);
		}

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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		private readonly Dictionary<TKey, List<TValue>> dict;

		private sealed class Grouping : IGrouping<TKey, TValue>, IEnumerable<TValue>, IEnumerable
		{
			public Grouping(TKey key, List<TValue> values)
			{
				this.key = key;
				this.values = values;
			}

			public TKey Key
			{
				get
				{
					return this.key;
				}
			}

			public IEnumerator<TValue> GetEnumerator()
			{
				return this.values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.values.GetEnumerator();
			}

			private readonly TKey key;

			private readonly List<TValue> values;
		}
	}
}
