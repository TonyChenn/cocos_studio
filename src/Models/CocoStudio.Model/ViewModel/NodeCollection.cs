using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	public class NodeCollection : ObservableCollection<AbstractNodeObject>, IInsertableList
	{
		public NodeCollection(AbstractNodeObject parentObject)
		{
			this.parentObject = parentObject;
		}

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

		private AbstractNodeObject parentObject;
	}
}
