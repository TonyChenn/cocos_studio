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
	public sealed class SimpleInterningProvider : InterningProvider
	{
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

		private Dictionary<object, object> byValueDict = new Dictionary<object, object>();

		private Dictionary<ISupportsInterning, ISupportsInterning> supportsInternDict = new Dictionary<ISupportsInterning, ISupportsInterning>(new SimpleInterningProvider.InterningComparer());

		private Dictionary<IEnumerable, IEnumerable> listDict = new Dictionary<IEnumerable, IEnumerable>(new SimpleInterningProvider.ListComparer());

		private sealed class InterningComparer : IEqualityComparer<ISupportsInterning>
		{
			public bool Equals(ISupportsInterning x, ISupportsInterning y)
			{
				return x.EqualsForInterning(y);
			}

			public int GetHashCode(ISupportsInterning obj)
			{
				return obj.GetHashCodeForInterning();
			}
		}

		private sealed class ListComparer : IEqualityComparer<IEnumerable>
		{
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
