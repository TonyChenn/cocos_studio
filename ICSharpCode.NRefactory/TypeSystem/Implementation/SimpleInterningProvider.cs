using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Simple interning provider.
	/// </summary>
	// Token: 0x020000DA RID: 218
	public sealed class SimpleInterningProvider : InterningProvider
	{
		// Token: 0x06000810 RID: 2064 RVA: 0x0001566C File Offset: 0x0001466C
		public override ISupportsInterning Intern(ISupportsInterning obj)
		{
			if (obj == null)
			{
				return null;
			}
			FreezableHelper.Freeze(obj);
			ISupportsInterning result;
			if (this.supportsInternDict.TryGetValue(obj, out result))
			{
				return result;
			}
			this.supportsInternDict.Add(obj, obj);
			return obj;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x000156A4 File Offset: 0x000146A4
		public override string Intern(string text)
		{
			if (text == null)
			{
				return null;
			}
			object obj;
			if (this.byValueDict.TryGetValue(text, out obj))
			{
				return (string)obj;
			}
			return text;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x000156D0 File Offset: 0x000146D0
		public override object InternValue(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			object result;
			if (this.byValueDict.TryGetValue(obj, out result))
			{
				return result;
			}
			return obj;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000156F8 File Offset: 0x000146F8
		public override IList<T> InternList<T>(IList<T> list)
		{
			if (list == null)
			{
				return null;
			}
			if (list.Count == 0)
			{
				return EmptyList<T>.Instance;
			}
			if (!list.IsReadOnly)
			{
				list = new ReadOnlyCollection<T>(list);
			}
			IEnumerable enumerable;
			if (this.listDict.TryGetValue(list, out enumerable))
			{
				list = (IList<T>)enumerable;
			}
			else
			{
				this.listDict.Add(list, list);
			}
			return list;
		}

		// Token: 0x04000256 RID: 598
		private Dictionary<object, object> byValueDict = new Dictionary<object, object>();

		// Token: 0x04000257 RID: 599
		private Dictionary<ISupportsInterning, ISupportsInterning> supportsInternDict = new Dictionary<ISupportsInterning, ISupportsInterning>(new SimpleInterningProvider.InterningComparer());

		// Token: 0x04000258 RID: 600
		private Dictionary<IEnumerable, IEnumerable> listDict = new Dictionary<IEnumerable, IEnumerable>(new SimpleInterningProvider.ListComparer());

		// Token: 0x020000DB RID: 219
		private sealed class InterningComparer : IEqualityComparer<ISupportsInterning>
		{
			// Token: 0x06000815 RID: 2069 RVA: 0x00015783 File Offset: 0x00014783
			public bool Equals(ISupportsInterning x, ISupportsInterning y)
			{
				return x.EqualsForInterning(y);
			}

			// Token: 0x06000816 RID: 2070 RVA: 0x0001578C File Offset: 0x0001478C
			public int GetHashCode(ISupportsInterning obj)
			{
				return obj.GetHashCodeForInterning();
			}
		}

		// Token: 0x020000DC RID: 220
		private sealed class ListComparer : IEqualityComparer<IEnumerable>
		{
			// Token: 0x06000818 RID: 2072 RVA: 0x0001579C File Offset: 0x0001479C
			public bool Equals(IEnumerable a, IEnumerable b)
			{
				if (a.GetType() != b.GetType())
				{
					return false;
				}
				IEnumerator enumerator = a.GetEnumerator();
				IEnumerator enumerator2 = b.GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (!enumerator2.MoveNext() || enumerator.Current != enumerator2.Current)
					{
						return false;
					}
				}
				return !enumerator2.MoveNext();
			}

			// Token: 0x06000819 RID: 2073 RVA: 0x000157FC File Offset: 0x000147FC
			public int GetHashCode(IEnumerable obj)
			{
				int num = obj.GetType().GetHashCode();
				foreach (object o in obj)
				{
					num *= 27;
					num += RuntimeHelpers.GetHashCode(o);
				}
				return num;
			}
		}
	}
}
