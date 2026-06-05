using System;
using System.Reflection;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000075 RID: 117
	internal class IndexedCollectionHandler : GenericCollectionHandler
	{
		// Token: 0x060003CC RID: 972 RVA: 0x0000E6A9 File Offset: 0x0000C8A9
		internal IndexedCollectionHandler(Type type, Type elemType, MethodInfo addMethod, PropertyInfo indexerProp, PropertyInfo countProp) : base(type, elemType, addMethod)
		{
			this.indexer = indexerProp;
			this.count = countProp;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000E6C4 File Offset: 0x0000C8C4
		public override void SetItem(object collection, object position, object item)
		{
			this.itemParam[0] = position;
			this.indexer.SetValue(collection, item, this.itemParam);
			this.itemParam[0] = null;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000E6EB File Offset: 0x0000C8EB
		public override object GetInitialPosition(object collection)
		{
			return -1;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000E6F4 File Offset: 0x0000C8F4
		public override bool MoveNextItem(object collection, ref object position)
		{
			int num = (int)position + 1;
			position = num;
			return num < (int)this.count.GetValue(collection, null);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000E728 File Offset: 0x0000C928
		public override object GetCurrentItem(object collection, object position)
		{
			this.itemParam[0] = position;
			object value = this.indexer.GetValue(collection, this.itemParam);
			this.itemParam[0] = null;
			return value;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000E75B File Offset: 0x0000C95B
		public override bool IsEmpty(object collection)
		{
			return collection == null || (int)this.count.GetValue(collection, null) == 0;
		}

		// Token: 0x0400014E RID: 334
		private PropertyInfo indexer;

		// Token: 0x0400014F RID: 335
		private PropertyInfo count;
	}
}
