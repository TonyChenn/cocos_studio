using System;
using CocoStudio.UndoManager.Recorder;
using CocoStudio.UndoManager.TaskModel;

namespace CocoStudio.UndoManager
{
	// Token: 0x0200000E RID: 14
	public class UndoTask : UndoableTaskBase<object>
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002AAC File Offset: 0x00000CAC
		public override string DescriptionForUser
		{
			get
			{
				return "UndoTask";
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002AC4 File Offset: 0x00000CC4
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002ADB File Offset: 0x00000CDB
		public string TaskGroupName { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002AE4 File Offset: 0x00000CE4
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002AFB File Offset: 0x00000CFB
		internal BaseRecorder Recorder { get; private set; }

		// Token: 0x06000055 RID: 85 RVA: 0x00002B04 File Offset: 0x00000D04
		public UndoTask(BaseRecorder recorder)
		{
			if (recorder != null && recorder.NeedRecordCallback)
			{
				this.Recorder = recorder;
			}
			this.TaskGroupName = recorder.TaskGroupName;
			this.Init();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002B4A File Offset: 0x00000D4A
		public UndoTask(BaseRecorder recorder, Action<object> execute, Action<object> unExecute = null, Predicate<object> canExecute = null) : this(recorder)
		{
			this.execute = execute;
			this.canExecute = canExecute;
			this.unExecute = unExecute;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002B6C File Offset: 0x00000D6C
		private void Init()
		{
			base.Repeatable = false;
			this.RegisterEventHandle();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002B7E File Offset: 0x00000D7E
		private void RegisterEventHandle()
		{
			base.Execute += this.OnExecute;
			base.Undo += this.OnUndo;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002BA8 File Offset: 0x00000DA8
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

		// Token: 0x0600005A RID: 90 RVA: 0x00002C24 File Offset: 0x00000E24
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

		// Token: 0x0600005B RID: 91 RVA: 0x00002C8C File Offset: 0x00000E8C
		~UndoTask()
		{
			this.Dispose();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public override void Dispose()
		{
			this.canExecute = null;
			this.execute = null;
			this.unExecute = null;
			this.Recorder = null;
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		// Token: 0x0400000F RID: 15
		protected Predicate<object> canExecute;

		// Token: 0x04000010 RID: 16
		protected Action<object> execute;

		// Token: 0x04000011 RID: 17
		protected Action<object> unExecute;
	}
}
