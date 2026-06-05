using System;
using System.Collections;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200005E RID: 94
	internal class ArrayListHandler : ICollectionHandler
	{
		// Token: 0x06000302 RID: 770 RVA: 0x0000B4F0 File Offset: 0x000096F0
		public Type GetItemType()
		{
			return typeof(object);
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000B4FC File Offset: 0x000096FC
		public bool CanCreateInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000B4FF File Offset: 0x000096FF
		public object CreateCollection(out object position, int size)
		{
			position = 0;
			if (size != -1)
			{
				return new ArrayList(size);
			}
			return new ArrayList();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000B519 File Offset: 0x00009719
		public void ResetCollection(object collection, out object position, int size)
		{
			position = 0;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000B523 File Offset: 0x00009723
		public void AddItem(ref object collection, ref object position, object item)
		{
			((ArrayList)collection).Add(item);
			position = (int)position + 1;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000B543 File Offset: 0x00009743
		public void SetItem(object collection, object position, object item)
		{
			((ArrayList)collection)[(int)position] = item;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000B557 File Offset: 0x00009757
		public void FinishCreation(ref object collection, object position)
		{
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000B559 File Offset: 0x00009759
		public bool IsEmpty(object collection)
		{
			return collection == null || ((ArrayList)collection).Count == 0;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000B56E File Offset: 0x0000976E
		public object GetInitialPosition(object collection)
		{
			return -1;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000B578 File Offset: 0x00009778
		public bool MoveNextItem(object collection, ref object position)
		{
			int num = (int)position;
			position = ++num;
			ArrayList arrayList = (ArrayList)collection;
			return num < arrayList.Count;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000B5A9 File Offset: 0x000097A9
		public object GetCurrentItem(object collection, object position)
		{
			return ((ArrayList)collection)[(int)position];
		}

		// Token: 0x04000117 RID: 279
		public static ArrayListHandler Instance = new ArrayListHandler();
	}
}
