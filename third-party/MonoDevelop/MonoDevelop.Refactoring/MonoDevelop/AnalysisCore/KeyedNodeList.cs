using System;
using System.Collections.Generic;

namespace MonoDevelop.AnalysisCore
{
	internal class KeyedNodeList<K, V>
	{
		private Dictionary<K, List<V>> dict = new Dictionary<K, List<V>>();

		public List<V> Get(K key)
		{
			dict.TryGetValue(key, out var value);
			return value;
		}

		public void Remove(K key, V value)
		{
			if (!dict.TryGetValue(key, out var value2) || !value2.Remove(value))
			{
				throw new Exception("Item missing");
			}
			if (value2.Count == 0)
			{
				dict.Remove(key);
			}
		}

		public void Add(K key, V value)
		{
			if (!dict.TryGetValue(key, out var value2))
			{
				value2 = (dict[key] = new List<V>());
			}
			value2.Add(value);
		}
	}
}
