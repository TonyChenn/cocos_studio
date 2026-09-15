using System;
using CocoStudio.UndoManager.Recorder;
using CocoStudio.UndoManager.TaskModel;

namespace CocoStudio.UndoManager
{
	public class UndoTask : UndoableTaskBase<object>
	{
		public override string DescriptionForUser
		{
			get
			{
				return "UndoTask";
			}
		}

		public string TaskGroupName { get; private set; }

		internal BaseRecorder Recorder { get; private set; }

		public UndoTask(BaseRecorder recorder)
		{
			if (recorder != null && recorder.NeedRecordCallback)
			{
				this.Recorder = recorder;
			}
			this.TaskGroupName = recorder.TaskGroupName;
			this.Init();
		}

		public UndoTask(BaseRecorder recorder, Action<object> execute, Action<object> unExecute = null, Predicate<object> canExecute = null) : this(recorder)
		{
			this.execute = execute;
			this.canExecute = canExecute;
			this.unExecute = unExecute;
		}

		private void Init()
		{
			base.Repeatable = false;
			this.RegisterEventHandle();
		}

		private void RegisterEventHandle()
		{
			base.Execute += this.OnExecute;
			base.Undo += this.OnUndo;
		}

		private void OnExecute(object sender, TaskEventArgs<object> e)
		{
			if (e.TaskMode == TaskMode.Redo)
			{
				if (this.Recorder != null)
				{
					RecorderTaskEventArgs recorderTaskEventArgs = new RecorderTaskEventArgs(this);
					this.Recorder.Redoing(recorderTaskEventArgs);
					if (recorderTaskEventArgs.Enabled)
					{
						this.execute(null);
					}
					this.Recorder.Redone(recorderTaskEventArgs);
				}
				else
				{
					this.execute(null);
				}
			}
		}

		private void OnUndo(object sender, TaskEventArgs<object> e)
		{
			if (this.Recorder != null)
			{
				RecorderTaskEventArgs recorderTaskEventArgs = new RecorderTaskEventArgs(this);
				this.Recorder.Undoing(recorderTaskEventArgs);
				if (recorderTaskEventArgs.Enabled)
				{
					this.unExecute(null);
				}
				this.Recorder.Undone(recorderTaskEventArgs);
			}
			else
			{
				this.unExecute(null);
			}
		}

		~UndoTask()
		{
			this.Dispose();
		}

		public override void Dispose()
		{
			this.canExecute = null;
			this.execute = null;
			this.unExecute = null;
			this.Recorder = null;
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		protected Predicate<object> canExecute;

		protected Action<object> execute;

		protected Action<object> unExecute;
	}
}
