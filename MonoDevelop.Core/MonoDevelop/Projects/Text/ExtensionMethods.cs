using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x020001FD RID: 509
	public static class ExtensionMethods
	{
		// Token: 0x0600135A RID: 4954 RVA: 0x0004F80C File Offset: 0x0004DA0C
		public static bool StartsWith<T>(this IEnumerable<T> t, IEnumerable<T> s)
		{
			bool result;
			using (IEnumerator<T> enumerator = t.GetEnumerator())
			{
				using (IEnumerator<T> enumerator2 = s.GetEnumerator())
				{
					bool flag;
					while ((flag = enumerator.MoveNext()) && enumerator2.MoveNext())
					{
						T t2 = enumerator.Current;
						if (!t2.Equals(enumerator2.Current))
						{
							return false;
						}
					}
					result = flag;
				}
			}
			return result;
		}
	}
}
