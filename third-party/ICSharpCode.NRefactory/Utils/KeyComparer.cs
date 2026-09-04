using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x0200011D RID: 285
	public static class KeyComparer
	{
		// Token: 0x06000A1D RID: 2589 RVA: 0x0001E2A8 File Offset: 0x0001D2A8
		public static KeyComparer<TElement, TKey> Create<TElement, TKey>(Func<TElement, TKey> keySelector)
		{
			return new KeyComparer<TElement, TKey>(keySelector, Comparer<TKey>.Default, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0001E2BA File Offset: 0x0001D2BA
		public static KeyComparer<TElement, TKey> Create<TElement, TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, IEqualityComparer<TKey> equalityComparer)
		{
			return new KeyComparer<TElement, TKey>(keySelector, comparer, equalityComparer);
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0001E2C4 File Offset: 0x0001D2C4
		public static IComparer<TElement> Create<TElement, TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
		{
			return new KeyComparer<TElement, TKey>(keySelector, comparer, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0001E2D2 File Offset: 0x0001D2D2
		public static IEqualityComparer<TElement> Create<TElement, TKey>(Func<TElement, TKey> keySelector, IEqualityComparer<TKey> equalityComparer)
		{
			return new KeyComparer<TElement, TKey>(keySelector, Comparer<TKey>.Default, equalityComparer);
		}
	}
}
