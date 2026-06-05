using System;
using System.Collections;
using System.Reflection;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000076 RID: 118
	internal class EnumerableCollectionHandler : GenericCollectionHandler
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x0000E777 File Offset: 0x0000C977
		internal EnumerableCollectionHandler(Type type, Type elemType, MethodInfo addMethod, PropertyInfo count) : base(type, elemType, addMethod)
		{
			this.count = count;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000E78A File Offset: 0x0000C98A
		public override void SetItem(object collection, object position, object item)
		{
			base.AddItem(ref collection, ref position, item);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000E797 File Offset: 0x0000C997
		public override object GetInitialPosition(object collection)
		{
			return ((IEnumerable)collection).GetEnumerator();
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000E7A4 File Offset: 0x0000C9A4
		public override bool MoveNextItem(object collection, ref object position)
		{
			return ((IEnumerator)position).MoveNext();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000E7B2 File Offset: 0x0000C9B2
		public override object GetCurrentItem(object collection, object position)
		{
			return ((IEnumerator)position).Current;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000E7C0 File Offset: 0x0000C9C0
		public override bool IsEmpty(object collection)
		{
			if (collection == null)
			{
				return true;
			}
			if (this.count != null)
			{
				return (int)this.count.GetValue(collection, null) == 0;
			}
			IEnumerator enumerator = ((IEnumerable)collection).GetEnumerator();
			return !enumerator.MoveNext();
		}

		// Token: 0x04000150 RID: 336
		private PropertyInfo count;
	}
}
