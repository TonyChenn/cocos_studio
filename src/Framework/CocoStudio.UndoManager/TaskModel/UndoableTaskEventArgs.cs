using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x0200001C RID: 28
	public class UndoableTaskEventArgs<TArgument> : TaskEventArgs<TArgument>
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00004A2C File Offset: 0x00002C2C
		public UndoableTaskEventArgs(TArgument argument) : base(argument)
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004A3F File Offset: 0x00002C3F
		internal UndoableTaskEventArgs(TArgument argument, TaskMode taskMode) : base(argument, taskMode)
		{
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00004A54 File Offset: 0x00002C54
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00004A6C File Offset: 0x00002C6C
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		// Token: 0x04000027 RID: 39
		private bool enabled = true;
	}
}
