using System;
using System.Collections.Concurrent;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Allows caching values for a specific compilation.
	/// A CacheManager consists of a for shared instances (shared among all threads working with that resolve context).
	/// </summary>
	/// <remarks>This class is thread-safe</remarks>
	public sealed class CacheManager
	{
		public object GetShared(object key)
		{
			object result;
			this.sharedDict.TryGetValue(key, out result);
			return result;
		}

		public object GetOrAddShared(object key, Func<object, object> valueFactory)
		{
			return this.sharedDict.GetOrAdd(key, valueFactory);
		}

		public object GetOrAddShared(object key, object value)
		{
			return this.sharedDict.GetOrAdd(key, value);
		}

		public void SetShared(object key, object value)
		{
			this.sharedDict[key] = value;
		}

		private readonly ConcurrentDictionary<object, object> sharedDict = new ConcurrentDictionary<object, object>(ReferenceComparer.Instance);
	}
}
