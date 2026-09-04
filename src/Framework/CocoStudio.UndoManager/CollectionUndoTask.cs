using System;
using System.Collections;
using CocoStudio.UndoManager.Recorder;

namespace CocoStudio.UndoManager
{
	// Token: 0x0200000F RID: 15
	public class CollectionUndoTask : UndoTask
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002CF0 File Offset: 0x00000EF0
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002D07 File Offset: 0x00000F07
		public IEnumerable NewItems { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002D10 File Offset: 0x00000F10
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002D27 File Offset: 0x00000F27
		public IEnumerable OldItems { get; private set; }

		// Token: 0x06000061 RID: 97 RVA: 0x00002D30 File Offset: 0x00000F30
		public CollectionUndoTask(BaseRecorder recorder, IEnumerable newItems, IEnumerable oldItems, Action<object> execute, Action<object> unExecute = null, Predicate<object> canExecute = null) : base(recorder, execute, unExecute, canExecute)
		{
			this.NewItems = newItems;
			this.OldItems = oldItems;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002D52 File Offset: 0x00000F52
		public override void Dispose()
		{
			base.Dispose();
			this.DisposeItems(this.NewItems);
			this.NewItems = null;
			this.DisposeItems(this.OldItems);
			this.OldItems = null;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002D88 File Offset: 0x00000F88
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
