using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200005C RID: 92
	public interface ICollectionHandler
	{
		// Token: 0x060002EB RID: 747
		Type GetItemType();

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002EC RID: 748
		bool CanCreateInstance { get; }

		// Token: 0x060002ED RID: 749
		object CreateCollection(out object position, int size);

		// Token: 0x060002EE RID: 750
		void ResetCollection(object collection, out object position, int size);

		// Token: 0x060002EF RID: 751
		void AddItem(ref object collection, ref object position, object item);

		// Token: 0x060002F0 RID: 752
		void SetItem(object collection, object position, object item);

		// Token: 0x060002F1 RID: 753
		void FinishCreation(ref object collection, object position);

		// Token: 0x060002F2 RID: 754
		bool IsEmpty(object collection);

		// Token: 0x060002F3 RID: 755
		object GetInitialPosition(object collection);

		// Token: 0x060002F4 RID: 756
		bool MoveNextItem(object collection, ref object position);

		// Token: 0x060002F5 RID: 757
		object GetCurrentItem(object collection, object position);
	}
}
