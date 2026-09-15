using System;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager.Recorder
{
	public abstract class BaseRecorder : IDisposable
	{
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

		public static bool IsUndoing
		{
			get
			{
				return TaskServiceSingleton.Instance.IsUndoing;
			}
		}

		[Obsolete]
		internal string TaskGroupName { get; private set; }

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

		internal bool NeedRecordCallback
		{
			get
			{
				return this.objectItem is IRecordableCallback;
			}
		}

		public event EventHandler<RecorderCreatedEventArgs> RecorderCreatedEvent;

		public BaseRecorder(INotifyStateChanged objectItem, string taskGroupName = null)
		{
			this.IsAutoRecord = true;
			this.objectItem = objectItem;
			this.TaskGroupName = taskGroupName;
		}

		internal void Redoing(RecorderTaskEventArgs args)
		{
			IRecordableCallback recordableCallback = this.objectItem as IRecordableCallback;
			if (recordableCallback != null)
			{
				recordableCallback.Redoing(args);
			}
		}

		internal void Redone(RecorderTaskEventArgs args)
		{
			IRecordableCallback recordableCallback = this.objectItem as IRecordableCallback;
			if (recordableCallback != null)
			{
				recordableCallback.Redone(args);
			}
		}

		internal void Undoing(RecorderTaskEventArgs args)
		{
			IRecordableCallback recordableCallback = this.objectItem as IRecordableCallback;
			if (recordableCallback != null)
			{
				recordableCallback.Undoing(args);
			}
		}

		internal void Undone(RecorderTaskEventArgs args)
		{
			IRecordableCallback recordableCallback = this.objectItem as IRecordableCallback;
			if (recordableCallback != null)
			{
				recordableCallback.Undone(args);
			}
		}

		protected virtual void OnStart(bool isCreateRecorder)
		{
		}

		protected virtual void OnStop(bool isUpdateOldValues = false)
		{
		}

		public void AddRecord(UndoTask undoTask)
		{
			CompositeTaskManager.Instance.AddRecord(undoTask);
			if (this.RecorderCreatedEvent != null)
			{
				this.RecorderCreatedEvent(this, new RecorderCreatedEventArgs(this.objectItem, undoTask));
			}
		}

		public void Start(bool isCreateRecorder = true, bool isSoleRecoder = false)
		{
			if (this.IsAutoRecord)
			{
				throw new InvalidOperationException("This recorder is already opened.");
			}
			this.IsAutoRecord = true;
			this.OnStart(isCreateRecorder);
		}

		public void Stop(bool isUpdateOldValues = false)
		{
			if (!this.IsAutoRecord)
			{
				throw new InvalidOperationException("This recorder is not opened.");
			}
			this.IsAutoRecord = false;
			this.OnStop(isUpdateOldValues);
		}

		~BaseRecorder()
		{
			this.Dispose();
		}

		public virtual void Dispose()
		{
			this.objectItem = null;
			GC.SuppressFinalize(this);
		}

		private static bool isCreateDefaultRecorder = true;

		protected INotifyStateChanged objectItem;

		private bool isAutoRecord;
	}
}
