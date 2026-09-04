using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x0200000B RID: 11
	public abstract class TaskBase<T> : IInternalTask, ITask, IDisposable
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000027A4 File Offset: 0x000009A4
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000027BB File Offset: 0x000009BB
		public bool Undoable { get; protected internal set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000038 RID: 56 RVA: 0x000027C4 File Offset: 0x000009C4
		// (remove) Token: 0x06000039 RID: 57 RVA: 0x00002800 File Offset: 0x00000A00
		private event EventHandler<TaskEventArgs<T>> execute;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600003A RID: 58 RVA: 0x0000283C File Offset: 0x00000A3C
		// (remove) Token: 0x0600003B RID: 59 RVA: 0x00002847 File Offset: 0x00000A47
		protected event EventHandler<TaskEventArgs<T>> Execute
		{
			add
			{
				this.execute += value;
			}
			remove
			{
				this.execute -= value;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002854 File Offset: 0x00000A54
		private void OnExecute(TaskEventArgs<T> e)
		{
			if (this.execute != null)
			{
				this.execute(this, e);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002880 File Offset: 0x00000A80
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002897 File Offset: 0x00000A97
		internal T Argument { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000028A0 File Offset: 0x00000AA0
		object IInternalTask.Argument
		{
			get
			{
				return this.Argument;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000028C0 File Offset: 0x00000AC0
		TaskResult IInternalTask.PerformTask(object argument, TaskMode taskMode)
		{
			this.Argument = (T)((object)argument);
			TaskEventArgs<T> taskEventArgs = new TaskEventArgs<T>(this.Argument, taskMode);
			this.OnExecute(taskEventArgs);
			return taskEventArgs.TaskResult;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000028FC File Offset: 0x00000AFC
		internal TaskResult PerformTask(object argument, TaskMode taskMode)
		{
			return ((IInternalTask)this).PerformTask(argument, taskMode);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002918 File Offset: 0x00000B18
		internal TaskResult Repeat()
		{
			TaskEventArgs<T> taskEventArgs = new TaskEventArgs<T>(this.Argument, TaskMode.Repeat);
			this.OnExecute(taskEventArgs);
			return taskEventArgs.TaskResult;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000043 RID: 67
		public abstract string DescriptionForUser { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002948 File Offset: 0x00000B48
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002960 File Offset: 0x00000B60
		public bool Repeatable
		{
			get
			{
				return this.repeatable;
			}
			protected set
			{
				if (this.repeatable != value)
				{
					this.repeatable = value;
					if (this.TaskService != null)
					{
						this.TaskService.NotifyTaskRepeatableChanged(this);
					}
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000029A0 File Offset: 0x00000BA0
		// (set) Token: 0x06000047 RID: 71 RVA: 0x000029B7 File Offset: 0x00000BB7
		internal IInternalTaskService TaskService { get; set; }

		// Token: 0x06000048 RID: 72 RVA: 0x000029C0 File Offset: 0x00000BC0
		public virtual void Dispose()
		{
		}

		// Token: 0x0400000A RID: 10
		private bool repeatable;
	}
}
