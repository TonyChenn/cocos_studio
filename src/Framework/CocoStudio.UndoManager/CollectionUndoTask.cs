using System;
using System.Collections;
using CocoStudio.UndoManager.Recorder;

namespace CocoStudio.UndoManager
{
	public class CollectionUndoTask : UndoTask
	{
		public IEnumerable NewItems { get; private set; }

		public IEnumerable OldItems { get; private set; }

		public CollectionUndoTask(BaseRecorder recorder, IEnumerable newItems, IEnumerable oldItems, Action<object> execute, Action<object> unExecute = null, Predicate<object> canExecute = null) : base(recorder, execute, unExecute, canExecute)
		{
			this.NewItems = newItems;
			this.OldItems = oldItems;
		}

		public override void Dispose()
		{
			base.Dispose();
			this.DisposeItems(this.NewItems);
			this.NewItems = null;
			this.DisposeItems(this.OldItems);
			this.OldItems = null;
		}

		private void DisposeItems(IEnumerable items)
		{
			if (items != null)
			{
				foreach (object obj in items)
				{
					IDisposable disposable = obj as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
	}
}
