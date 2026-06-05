using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200005D RID: 93
	internal class ArrayHandler : ICollectionHandler
	{
		// Token: 0x060002F6 RID: 758 RVA: 0x0000B37C File Offset: 0x0000957C
		public ArrayHandler(Type type)
		{
			this._elementType = type.GetElementType();
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000B390 File Offset: 0x00009590
		public Type GetItemType()
		{
			return this._elementType;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000B398 File Offset: 0x00009598
		public bool CanCreateInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000B39B File Offset: 0x0000959B
		public object CreateCollection(out object position, int size)
		{
			position = 0;
			return Array.CreateInstance(this._elementType, (size != -1) ? size : 5);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000B3B8 File Offset: 0x000095B8
		public void ResetCollection(object collection, out object position, int size)
		{
			throw new InvalidOperationException("Array instance could not be reused.");
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000B3C4 File Offset: 0x000095C4
		public void AddItem(ref object collection, ref object position, object item)
		{
			int num = (int)position;
			Array array = (Array)collection;
			if (num >= array.Length)
			{
				Array array2 = Array.CreateInstance(this._elementType, array.Length + 5);
				Array.Copy(array, array2, array.Length);
				collection = array2;
				array2.SetValue(item, num);
			}
			else
			{
				array.SetValue(item, num);
			}
			position = num + 1;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000B42C File Offset: 0x0000962C
		public void SetItem(object collection, object position, object item)
		{
			int index = (int)position;
			((Array)collection).SetValue(item, index);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000B450 File Offset: 0x00009650
		public void FinishCreation(ref object collection, object position)
		{
			int num = (int)position;
			Array array = (Array)collection;
			if (num < array.Length)
			{
				Array array2 = Array.CreateInstance(this._elementType, num);
				Array.Copy(array, array2, num);
				collection = array2;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000B48D File Offset: 0x0000968D
		public bool IsEmpty(object collection)
		{
			return collection == null || ((Array)collection).Length == 0;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000B4A2 File Offset: 0x000096A2
		public object GetInitialPosition(object collection)
		{
			return -1;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000B4AC File Offset: 0x000096AC
		public bool MoveNextItem(object collection, ref object position)
		{
			int num = (int)position;
			position = ++num;
			Array array = (Array)collection;
			return num < array.Length;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000B4DD File Offset: 0x000096DD
		public object GetCurrentItem(object collection, object position)
		{
			return ((Array)collection).GetValue((int)position);
		}

		// Token: 0x04000116 RID: 278
		private Type _elementType;
	}
}
