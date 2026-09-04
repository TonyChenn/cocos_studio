using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x0200011E RID: 286
	public class KeyComparer<TElement, TKey> : IComparer<TElement>, IEqualityComparer<TElement>
	{
		// Token: 0x06000A21 RID: 2593 RVA: 0x0001E2E0 File Offset: 0x0001D2E0
		public KeyComparer(Func<TElement, TKey> keySelector, IComparer<TKey> keyComparer, IEqualityComparer<TKey> keyEqualityComparer)
		{
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			if (keyComparer == null)
			{
				throw new ArgumentNullException("keyComparer");
			}
			if (keyEqualityComparer == null)
			{
				throw new ArgumentNullException("keyEqualityComparer");
			}
			this.keySelector = keySelector;
			this.keyComparer = keyComparer;
			this.keyEqualityComparer = keyEqualityComparer;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0001E332 File Offset: 0x0001D332
		public int Compare(TElement x, TElement y)
		{
			return this.keyComparer.Compare(this.keySelector(x), this.keySelector(y));
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001E357 File Offset: 0x0001D357
		public bool Equals(TElement x, TElement y)
		{
			return this.keyEqualityComparer.Equals(this.keySelector(x), this.keySelector(y));
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0001E37C File Offset: 0x0001D37C
		public int GetHashCode(TElement obj)
		{
			return this.keyEqualityComparer.GetHashCode(this.keySelector(obj));
		}

		// Token: 0x04000374 RID: 884
		private readonly Func<TElement, TKey> keySelector;

		// Token: 0x04000375 RID: 885
		private readonly IComparer<TKey> keyComparer;

		// Token: 0x04000376 RID: 886
		private readonly IEqualityComparer<TKey> keyEqualityComparer;
	}
}
