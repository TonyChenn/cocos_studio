using System;

namespace CocoStudio.UndoManager.TaskModel
{
	public sealed class UndoableTask<T> : UndoableTaskBase<T>
	{
		public UndoableTask(Action<TaskEventArgs<T>> execute, Action<TaskEventArgs<T>> undo, string descriptionForUser)
		{
			ArgumentValidator.AssertNotNullOrEmpty(descriptionForUser, "descriptionForUser");
			ArgumentValidator.AssertNotNull<Action<TaskEventArgs<T>>>(execute, "execute");
			ArgumentValidator.AssertNotNull<Action<TaskEventArgs<T>>>(undo, "undo");
			this.descriptionForUser = descriptionForUser;
			base.Execute += delegate(object o, TaskEventArgs<T> args)
			{
				execute(args);
			};
			base.Undo += delegate(object o, TaskEventArgs<T> args)
			{
				undo(args);
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
