using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	public class KeyComparer<TElement, TKey> : IComparer<TElement>, IEqualityComparer<TElement>
	{
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

		public int Compare(TElement x, TElement y)
		{
			return this.keyComparer.Compare(this.keySelector(x), this.keySelector(y));
		}

		public bool Equals(TElement x, TElement y)
		{
			return this.keyEqualityComparer.Equals(this.keySelector(x), this.keySelector(y));
		}

		public int GetHashCode(TElement obj)
		{
			return this.keyEqualityComparer.GetHashCode(this.keySelector(obj));
		}

		private readonly Func<TElement, TKey> keySelector;

		private readonly IComparer<TKey> keyComparer;

		private readonly IEqualityComparer<TKey> keyEqualityComparer;
	}
}
