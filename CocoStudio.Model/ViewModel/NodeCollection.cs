using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000127 RID: 295
	public class NodeCollection : ObservableCollection<AbstractNodeObject>, IInsertableList
	{
		// Token: 0x06000B0E RID: 2830 RVA: 0x0002B7FB File Offset: 0x000299FB
		public NodeCollection(AbstractNodeObject parentObject)
		{
			this.parentObject = parentObject;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0002B810 File Offset: 0x00029A10
		protected override void InsertItem(int index, AbstractNodeObject item)
		{
			if (item == null)
			{
				throw new ArgumentException("Child should not be null.");
			}
			this.parentObject.InsertChild(index, item);
			item.Parent = this.parentObject;
			base.InsertItem(index, item);
			item.AfterAdded();
			item.BindingRecorder(null);
			item.AncestorObjectChanged(item, NotifyCollectionChangedAction.Add);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0002B870 File Offset: 0x00029A70
		protected override void RemoveItem(int index)
		{
			AbstractNodeObject abstractNodeObject = base.Items[index];
			if (abstractNodeObject == null)
			{
				throw new ArgumentException("Child should not be null.");
			}
			abstractNodeObject.BeforeRemoved();
			abstractNodeObject.AncestorObjectChanged(abstractNodeObject, NotifyCollectionChangedAction.Remove);
			abstractNodeObject.Parent = null;
			this.parentObject.RemoveChild(abstractNodeObject);
			base.RemoveItem(index);
		}

		// Token: 0x040004A2 RID: 1186
		private AbstractNodeObject parentObject;
	}
}
