using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace MonoDevelop.Core.Web
{
	// Token: 0x0200025D RID: 605
	internal sealed class MemoryCache : IDisposable
	{
		// Token: 0x06001610 RID: 5648 RVA: 0x000593C2 File Offset: 0x000575C2
		internal MemoryCache()
		{
			this._timer = new Timer(new TimerCallback(this.RemoveExpiredEntries), null, MemoryCache._cleanupInterval, MemoryCache._cleanupInterval);
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001611 RID: 5649 RVA: 0x000593F7 File Offset: 0x000575F7
		internal static MemoryCache Instance
		{
			get
			{
				return MemoryCache._instance.Value;
			}
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00059404 File Offset: 0x00057604
		internal T GetOrAdd<T>(object cacheKey, Func<T> factory, TimeSpan expiration, bool absoluteExpiration = false) where T : class
		{
			MemoryCache.CacheItem value = new MemoryCache.CacheItem(factory, expiration, absoluteExpiration);
			MemoryCache.CacheItem orAdd = this._cache.GetOrAdd(cacheKey, value);
			orAdd.UpdateUsage(expiration);
			return (T)((object)orAdd.Value);
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x0005943C File Offset: 0x0005763C
		internal bool TryGetValue<T>(object cacheKey, out T value) where T : class
		{
			MemoryCache.CacheItem cacheItem;
			if (this._cache.TryGetValue(cacheKey, out cacheItem))
			{
				value = (T)((object)cacheItem.Value);
				return true;
			}
			value = default(T);
			return false;
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00059474 File Offset: 0x00057674
		internal void Remove(object cacheKey)
		{
			MemoryCache.CacheItem cacheItem;
			this._cache.TryRemove(cacheKey, out cacheItem);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00059490 File Offset: 0x00057690
		private void RemoveExpiredEntries(object state)
		{
			ICollection<object> keys = this._cache.Keys;
			foreach (object key in keys)
			{
				MemoryCache.CacheItem cacheItem;
				if (this._cache.TryGetValue(key, out cacheItem) && cacheItem.Expired)
				{
					this._cache.TryRemove(key, out cacheItem);
				}
			}
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00059504 File Offset: 0x00057704
		public void Dispose()
		{
			if (this._timer != null)
			{
				this._timer.Dispose();
			}
		}

		// Token: 0x040006A5 RID: 1701
		private static readonly Lazy<MemoryCache> _instance = new Lazy<MemoryCache>(() => new MemoryCache());

		// Token: 0x040006A6 RID: 1702
		private static readonly TimeSpan _cleanupInterval = TimeSpan.FromSeconds(10.0);

		// Token: 0x040006A7 RID: 1703
		private readonly ConcurrentDictionary<object, MemoryCache.CacheItem> _cache = new ConcurrentDictionary<object, MemoryCache.CacheItem>();

		// Token: 0x040006A8 RID: 1704
		private readonly Timer _timer;

		// Token: 0x0200025E RID: 606
		private sealed class CacheItem
		{
			// Token: 0x06001619 RID: 5657 RVA: 0x0005955C File Offset: 0x0005775C
			public CacheItem(Func<object> valueFactory, TimeSpan expires, bool absoluteExpiration)
			{
				this._valueFactory = new Lazy<object>(valueFactory);
				this._absoluteExpiration = absoluteExpiration;
				this._expires = DateTime.UtcNow.Ticks + expires.Ticks;
			}

			// Token: 0x170004AE RID: 1198
			// (get) Token: 0x0600161A RID: 5658 RVA: 0x0005959D File Offset: 0x0005779D
			public object Value
			{
				get
				{
					return this._valueFactory.Value;
				}
			}

			// Token: 0x0600161B RID: 5659 RVA: 0x000595AC File Offset: 0x000577AC
			public void UpdateUsage(TimeSpan slidingExpiration)
			{
				if (!this._absoluteExpiration)
				{
					this._expires = DateTime.UtcNow.Ticks + slidingExpiration.Ticks;
				}
			}

			// Token: 0x170004AF RID: 1199
			// (get) Token: 0x0600161C RID: 5660 RVA: 0x000595DC File Offset: 0x000577DC
			public bool Expired
			{
				get
				{
					long ticks = DateTime.UtcNow.Ticks;
					long num = Interlocked.Read(ref this._expires);
					return ticks > num;
				}
			}

			// Token: 0x040006AA RID: 1706
			private readonly Lazy<object> _valueFactory;

			// Token: 0x040006AB RID: 1707
			private readonly bool _absoluteExpiration;

			// Token: 0x040006AC RID: 1708
			private long _expires;
		}
	}
}
