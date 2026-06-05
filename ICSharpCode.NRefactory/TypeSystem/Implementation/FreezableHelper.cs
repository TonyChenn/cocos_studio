using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x0200009E RID: 158
	public static class FreezableHelper
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x0000BED4 File Offset: 0x0000AED4
		public static void ThrowIfFrozen(IFreezable freezable)
		{
			if (freezable.IsFrozen)
			{
				throw new InvalidOperationException("Cannot mutate frozen " + freezable.GetType().Name);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000BEFC File Offset: 0x0000AEFC
		public static IList<T> FreezeListAndElements<T>(IList<T> list)
		{
			if (list != null)
			{
				foreach (T t in list)
				{
					FreezableHelper.Freeze(t);
				}
			}
			return FreezableHelper.FreezeList<T>(list);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000BF54 File Offset: 0x0000AF54
		public static IList<T> FreezeList<T>(IList<T> list)
		{
			if (list == null || list.Count == 0)
			{
				return EmptyList<T>.Instance;
			}
			if (list.IsReadOnly)
			{
				return list;
			}
			return new ReadOnlyCollection<T>(list.ToArray<T>());
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000BF7C File Offset: 0x0000AF7C
		public static void Freeze(object item)
		{
			IFreezable freezable = item as IFreezable;
			if (freezable != null)
			{
				freezable.Freeze();
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000BF99 File Offset: 0x0000AF99
		public static T FreezeAndReturn<T>(T item) where T : IFreezable
		{
			item.Freeze();
			return item;
		}

		/// <summary>
		/// If the item is not frozen, this method creates and returns a frozen clone.
		/// If the item is already frozen, it is returned without creating a clone.
		/// </summary>
		// Token: 0x060004F3 RID: 1267 RVA: 0x0000BFA9 File Offset: 0x0000AFA9
		public static T GetFrozenClone<T>(T item) where T : IFreezable, ICloneable
		{
			if (!item.IsFrozen)
			{
				item = (T)((object)item.Clone());
				item.Freeze();
			}
			return item;
		}
	}
}
