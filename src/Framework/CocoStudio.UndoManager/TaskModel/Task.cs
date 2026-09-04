using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x02000021 RID: 33
	public sealed class Task<T> : TaskBase<T>
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00004FE4 File Offset: 0x000031E4
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00005048 File Offset: 0x00003248
		public override string DescriptionForUser
		{
			get
			{
				return this.descriptionForUser;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005060 File Offset: 0x00003260
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00005078 File Offset: 0x00003278
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

		// Token: 0x04000031 RID: 49
		private readonly string descriptionForUser;
	}
}
