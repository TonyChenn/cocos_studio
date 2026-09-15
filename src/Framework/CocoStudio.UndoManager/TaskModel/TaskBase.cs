using System;

namespace CocoStudio.UndoManager.TaskModel
{
	public abstract class TaskBase<T> : IInternalTask, ITask, IDisposable
	{
		public bool Undoable { get; protected internal set; }

		private event EventHandler<TaskEventArgs<T>> execute;

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

		private void OnExecute(TaskEventArgs<T> e)
		{
			if (this.execute != null)
			{
				this.execute(this, e);
			}
		}

		internal T Argument { get; private set; }

		object IInternalTask.Argument
		{
			get
			{
				return this.Argument;
			}
		}

		TaskResult IInternalTask.PerformTask(object argument, TaskMode taskMode)
		{
			this.Argument = (T)((object)argument);
			TaskEventArgs<T> taskEventArgs = new TaskEventArgs<T>(this.Argument, taskMode);
			this.OnExecute(taskEventArgs);
			return taskEventArgs.TaskResult;
		}

		internal TaskResult PerformTask(object argument, TaskMode taskMode)
		{
			return ((IInternalTask)this).PerformTask(argument, taskMode);
		}

		internal TaskResult Repeat()
		{
			TaskEventArgs<T> taskEventArgs = new TaskEventArgs<T>(this.Argument, TaskMode.Repeat);
			this.OnExecute(taskEventArgs);
			return taskEventArgs.TaskResult;
		}

		public abstract string DescriptionForUser { get; }

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

		internal IInternalTaskService TaskService { get; set; }

		public virtual void Dispose()
		{
		}

		private bool repeatable;
	}
}
