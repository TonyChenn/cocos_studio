using System;
using System.Threading;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x0200011F RID: 287
	public static class LazyInit
	{
		// Token: 0x06000A25 RID: 2597 RVA: 0x0001E398 File Offset: 0x0001D398
		public static T VolatileRead<T>(ref T location) where T : class
		{
			T result = location;
			Thread.MemoryBarrier();
			return result;
		}

		/// <summary>
		/// Atomically performs the following operation:
		/// - If target is null: stores newValue in target and returns newValue.
		/// - If target is not null: returns target.
		/// </summary>
		// Token: 0x06000A26 RID: 2598 RVA: 0x0001E3B4 File Offset: 0x0001D3B4
		public static T GetOrSet<T>(ref T target, T newValue) where T : class
		{
			T t = Interlocked.CompareExchange<T>(ref target, newValue, default(T));
			T result;
			if ((result = t) == null)
			{
				result = newValue;
			}
			return result;
		}
	}
}
