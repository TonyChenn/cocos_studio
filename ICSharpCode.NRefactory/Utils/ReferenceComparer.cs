using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x02000125 RID: 293
	public sealed class ReferenceComparer : IEqualityComparer<object>
	{
		// Token: 0x06000A5B RID: 2651 RVA: 0x0001EBC8 File Offset: 0x0001DBC8
		public bool Equals(object x, object y)
		{
			return x == y;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0001EBCE File Offset: 0x0001DBCE
		public int GetHashCode(object obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}

		// Token: 0x04000382 RID: 898
		public static readonly ReferenceComparer Instance = new ReferenceComparer();
	}
}
