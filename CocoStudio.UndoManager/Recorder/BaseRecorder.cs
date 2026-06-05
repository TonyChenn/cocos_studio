using System;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager.Recorder
{
	// Token: 0x02000008 RID: 8
	public abstract class BaseRecorder : IDisposable
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002494 File Offset: 0x00000694
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000024AB File Offset: 0x000006AB
		public static bool IsCreateDefaultRecorder
		{
			get
			{
				return BaseRecorder.isCreateDefaultRecorder;
			}
			set
			{
				BaseRecorder.isCreateDefaultRecorder = value;
				LogConfig.Logger.Debug("BaseRecorder: IsCreateDefaultRecorder: " + value);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000024D0 File Offset: 0x000006D0
		public static bool IsUndoing
		{
			get
			{
				return TaskServiceSingleton.Instance.IsUndoing;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000024EC File Offset: 0x000006EC
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002503 File Offset: 0x00000703
		[Obsolete]
		internal string TaskGroupName { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000250C File Offset: 0x0000070C
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002524 File Offset: 0x00000724
		public bool IsAutoRecord
		{
			get
			{
				return this.isAutoRecord;
			}
			set
			{
				this.isAutoRecord = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002530 File Offset: 0x00000730
		internal bool NeedRecordCallback
		{
			get
			{
				return this.objectItem is IRecordableCallback;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000022 RID: 34 RVA: 0x00002550 File Offset: 0x00000750
		// (remove) Token: 0x06000023 RID: 35 RVA: 0x0000258C File Offset: 0x0000078C
		public event EventHandler<RecorderCreatedEventArgs> RecorderCreatedEvent;

		// Token: 0x06000024 RID: 36 RVA: 0x000025C8 File Offset: 0x000007C8
		public BaseRecorder(INotifyStateChanged objectItem, string taskGroupName = null)
		{
			this.IsAutoRecord = true;
			this.objectItem = objectItem;
			this.TaskGroupName = taskGroupName;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000025EC File Offset: 0x000007EC
		internal void Redoing(RecorderTaskEventArgs args)
		{
			IRecordableCallback recordableCallback = this.objectItem as IRecordableCallback;
			if (recordableCallback != null)
			{
				recordableCallback.Redoing(args);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002618 File Offset: 0x00000818
		internal void Redone(RecorderTaskEventArgs args)
		{
			IRecordableCallback recordableCallback = this.objectItem as IRecordableCallback;
			if (recordableCallback != null)
			{
				recordableCallback.Redone(args);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002644 File Offset: 0x00000844
		internal void Undoing(RecorderTaskEventArgs args)
		{
			IRecordableCallback recordableCallback = this.objectItem as IRecordableCallback;
			if (recordableCallback != null)
			{
				recordableCallback.Undoing(args);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002670 File Offset: 0x00000870
		internal void Undone(RecorderTaskEventArgs args)
		{
			IRecordableCallback recordableCallback = this.objectItem as IRecordableCallback;
			if (recordableCallback != null)
			{
				recordableCallback.Undone(args);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000269C File Offset: 0x0000089C
		protected virtual void OnStart(bool isCreateRecorder)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000269F File Offset: 0x0000089F
		protected virtual void OnStop(bool isUpdateOldValues = false)
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000026A4 File Offset: 0x000008A4
		public void AddRecord(UndoTask undoTask)
		{
			CompositeTaskManager.Instance.AddRecord(undoTask);
			if (this.RecorderCreatedEvent != null)
			{
				this.RecorderCreatedEvent(this, new RecorderCreatedEventArgs(this.objectItem, undoTask));
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000026E8 File Offset: 0x000008E8
		public void Start(bool isCreateRecorder = true, bool isSoleRecoder = false)
		{
			if (this.IsAutoRecord)
			{
				throw new InvalidOperationException("This recorder is already opened.");
			}
			this.IsAutoRecord = true;
			this.OnStart(isCreateRecorder);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002720 File Offset: 0x00000920
		public void Stop(bool isUpdateOldValues = false)
		{
			if (!this.IsAutoRecord)
			{
				throw new InvalidOperationException("This recorder is not opened.");
			}
			this.IsAutoRecord = false;
			this.OnStop(isUpdateOldValues);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002754 File Offset: 0x00000954
		~BaseRecorder()
		{
			this.Dispose();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002788 File Offset: 0x00000988
		public virtual void Dispose()
		{
			this.objectItem = null;
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000004 RID: 4
		private static bool isCreateDefaultRecorder = true;

		// Token: 0x04000005 RID: 5
		protected INotifyStateChanged objectItem;

		// Token: 0x04000006 RID: 6
		private bool isAutoRecord;
	}
}
