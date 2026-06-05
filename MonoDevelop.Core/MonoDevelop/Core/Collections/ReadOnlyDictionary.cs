using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoDevelop.Core.Collections
{
	// Token: 0x02000092 RID: 146
	public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		// Token: 0x060004BC RID: 1212 RVA: 0x00010A4D File Offset: 0x0000EC4D
		public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
		{
			this.dictionary = dictionary;
		}

		// Token: 0x170000F5 RID: 245
		public TValue this[TKey key]
		{
			get
			{
				return this.dictionary[key];
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00010A6A File Offset: 0x0000EC6A
		public ICollection<TValue> Values
		{
			get
			{
				return this.dictionary.Values;
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00010A77 File Offset: 0x0000EC77
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.dictionary.TryGetValue(key, out value);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00010A86 File Offset: 0x0000EC86
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.dictionary.Contains(item);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00010A94 File Offset: 0x0000EC94
		public bool ContainsKey(TKey key)
		{
			return this.dictionary.ContainsKey(key);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00010AA2 File Offset: 0x0000ECA2
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.dictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00010AB1 File Offset: 0x0000ECB1
		public int Count
		{
			get
			{
				return this.dictionary.Count;
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00010ABE File Offset: 0x0000ECBE
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.dictionary.GetEnumerator();
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00010ACB File Offset: 0x0000ECCB
		public ICollection<TKey> Keys
		{
			get
			{
				return this.dictionary.Keys;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00010AD8 File Offset: 0x0000ECD8
		bool ICollection<KeyValuePair<!0, !1>>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00010ADB File Offset: 0x0000ECDB
		void ICollection<KeyValuePair<!0, !1>>.Add(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException("The dictionary is read-only.");
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00010AE7 File Offset: 0x0000ECE7
		void IDictionary<!0, !1>.Add(TKey key, TValue value)
		{
			throw new NotSupportedException("The dictionary is read-only.");
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00010AF3 File Offset: 0x0000ECF3
		void ICollection<KeyValuePair<!0, !1>>.Clear()
		{
			throw new NotSupportedException("The dictionary is read-only.");
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00010AFF File Offset: 0x0000ECFF
		bool ICollection<KeyValuePair<!0, !1>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException("The dictionary is read-only.");
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00010B0B File Offset: 0x0000ED0B
		bool IDictionary<!0, !1>.Remove(TKey key)
		{
			throw new NotSupportedException("The dictionary is read-only.");
		}

		// Token: 0x170000FA RID: 250
		TValue IDictionary<!0, !1>.this[TKey key]
		{
			get
			{
				return this.dictionary[key];
			}
			set
			{
				throw new NotSupportedException("The dictionary is read-only.");
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00010B31 File Offset: 0x0000ED31
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.dictionary.GetEnumerator();
		}

		// Token: 0x04000190 RID: 400
		private IDictionary<TKey, TValue> dictionary;
	}
}
