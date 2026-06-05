using System;
using System.Collections.Concurrent;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Allows caching values for a specific compilation.
	/// A CacheManager consists of a for shared instances (shared among all threads working with that resolve context).
	/// </summary>
	/// <remarks>This class is thread-safe</remarks>
	// Token: 0x02000109 RID: 265
	public sealed class CacheManager
	{
		// Token: 0x0600098D RID: 2445 RVA: 0x0001993C File Offset: 0x0001893C
		public object GetShared(object key)
		{
			object result;
			this.sharedDict.TryGetValue(key, out result);
			return result;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00019959 File Offset: 0x00018959
		public object GetOrAddShared(object key, Func<object, object> valueFactory)
		{
			return this.sharedDict.GetOrAdd(key, valueFactory);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00019968 File Offset: 0x00018968
		public object GetOrAddShared(object key, object value)
		{
			return this.sharedDict.GetOrAdd(key, value);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00019977 File Offset: 0x00018977
		public void SetShared(object key, object value)
		{
			this.sharedDict[key] = value;
		}

		// Token: 0x04000316 RID: 790
		private readonly ConcurrentDictionary<object, object> sharedDict = new ConcurrentDictionary<object, object>(ReferenceComparer.Instance);
	}
}
