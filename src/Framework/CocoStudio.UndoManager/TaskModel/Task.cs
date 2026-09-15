using System;

namespace CocoStudio.UndoManager.TaskModel
{
	public sealed class Task<T> : TaskBase<T>
	{
		public Task(Action<TaskEventArgs<T>> execute, string descriptionForUser)
		{
			ArgumentValidator.AssertNotNull<Action<TaskEventArgs<T>>>(execute, "execute");
			ArgumentValidator.AssertNotNullOrEmpty(descriptionForUser, "descriptionForUser");
			this.descriptionForUser = descriptionForUser;
			base.Execute += delegate(object o, TaskEventArgs<T> args)
			{
				execute(args);
			};
		}

		public override string DescriptionForUser
		{
			get
			{
				return this.descriptionForUser;
			}
		}

		public new bool Repeatable
		{
			get
			{
				return base.Repeatable;
			}
			set
			{
				base.Repeatable = value;
			}
		}

		private readonly string descriptionForUser;
	}
}
