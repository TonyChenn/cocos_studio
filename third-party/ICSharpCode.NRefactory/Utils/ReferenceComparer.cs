using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ICSharpCode.NRefactory.Utils
{
	public sealed class ReferenceComparer : IEqualityComparer<object>
	{
		public bool Equals(object x, object y)
		{
			return x == y;
		}

		public int GetHashCode(object obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}

		public static readonly ReferenceComparer Instance = new ReferenceComparer();
	}
}
