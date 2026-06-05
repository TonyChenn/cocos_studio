using System;
using System.Collections;

namespace MonoDevelop.Projects.Utility
{
	// Token: 0x02000202 RID: 514
	internal class DiffUtility
	{
		// Token: 0x06001391 RID: 5009 RVA: 0x00050B85 File Offset: 0x0004ED85
		public static int GetAddedItems(IList original, IList changed, IList result)
		{
			return DiffUtility.GetAddedItems(original, changed, result, Comparer.Default);
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00050B94 File Offset: 0x0004ED94
		public static int GetAddedItems(IList original, IList changed, IList result, IComparer comparer)
		{
			int num = 0;
			if (changed != null && result != null)
			{
				if (original == null)
				{
					foreach (object value in changed)
					{
						result.Add(value);
					}
					num = changed.Count;
				}
				else
				{
					foreach (object value2 in changed)
					{
						if (!DiffUtility.Contains(original, value2, comparer))
						{
							result.Add(value2);
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00050C58 File Offset: 0x0004EE58
		public static int GetRemovedItems(IList original, IList changed, IList result)
		{
			return DiffUtility.GetRemovedItems(original, changed, result, Comparer.Default);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00050C67 File Offset: 0x0004EE67
		public static int GetRemovedItems(IList original, IList changed, IList result, IComparer comparer)
		{
			return DiffUtility.GetAddedItems(changed, original, result, comparer);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00050C74 File Offset: 0x0004EE74
		private static bool Contains(IList list, object value, IComparer comparer)
		{
			foreach (object x in list)
			{
				if (comparer.Compare(x, value) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00050CD0 File Offset: 0x0004EED0
		public static int Compare(IList a, IList b)
		{
			return DiffUtility.Compare(a, b, Comparer.Default);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00050CE0 File Offset: 0x0004EEE0
		public static int Compare(IList a, IList b, IComparer comparer)
		{
			if (a != null && b != null)
			{
				int num = (a.Count < b.Count) ? a.Count : b.Count;
				for (int i = 0; i < num; i++)
				{
					if (a[i] is IComparable && b[i] is IComparable)
					{
						int num2 = comparer.Compare(a[i], b[i]);
						if (num2 != 0)
						{
							return num2;
						}
					}
				}
				return a.Count - b.Count;
			}
			if (a == b)
			{
				return 0;
			}
			if (a != null)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00050D6C File Offset: 0x0004EF6C
		public static int Compare(SortedList a, SortedList b)
		{
			return DiffUtility.Compare(a, b, Comparer.Default);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00050D7C File Offset: 0x0004EF7C
		public static int Compare(SortedList a, SortedList b, IComparer comparer)
		{
			if (a != null && b != null)
			{
				int num = (a.Count < b.Count) ? a.Count : b.Count;
				for (int i = 0; i < num; i++)
				{
					int result;
					if ((result = comparer.Compare(a.GetByIndex(i), b.GetByIndex(i))) != 0)
					{
						return result;
					}
				}
				return a.Count - b.Count;
			}
			if (a == b)
			{
				return 0;
			}
			if (a != null)
			{
				return 1;
			}
			return -1;
		}
	}
}
