using System;
using System.Threading;

namespace ICSharpCode.NRefactory.Utils
{
	public static class LazyInit
	{
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
