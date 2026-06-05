using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Contains extension methods for use within NRefactory.
	/// </summary>
	// Token: 0x0200010D RID: 269
	internal static class ExtensionMethods
	{
		// Token: 0x060009AC RID: 2476 RVA: 0x0001AFD8 File Offset: 0x00019FD8
		public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> input)
		{
			foreach (T item in input)
			{
				target.Add(item);
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0001B048 File Offset: 0x0001A048
		public static Predicate<T> And<T>(this Predicate<T> filter1, Predicate<T> filter2)
		{
			if (filter1 == null)
			{
				return filter2;
			}
			if (filter2 == null)
			{
				return filter1;
			}
			return (T m) => filter1(m) && filter2(m);
		}
	}
}
