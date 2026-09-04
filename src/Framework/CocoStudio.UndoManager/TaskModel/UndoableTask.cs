using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x0200002B RID: 43
	public sealed class UndoableTask<T> : UndoableTaskBase<T>
	{
		// Token: 0x0600015E RID: 350 RVA: 0x000072A8 File Offset: 0x000054A8
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00007340 File Offset: 0x00005540
		public override string DescriptionForUser
		{
			get
			{
				return this.descriptionForUser;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007358 File Offset: 0x00005558
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00007370 File Offset: 0x00005570
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

		// Token: 0x04000060 RID: 96
		private readonly string descriptionForUser;
	}
}
