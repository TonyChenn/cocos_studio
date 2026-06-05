using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x0200001B RID: 27
	public class TaskEventArgs<TArgument> : EventArgs
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000049A4 File Offset: 0x00002BA4
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x000049BB File Offset: 0x00002BBB
		public TArgument Argument { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000049C4 File Offset: 0x00002BC4
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x000049DB File Offset: 0x00002BDB
		public TaskResult TaskResult { get; set; }

		// Token: 0x060000D3 RID: 211 RVA: 0x000049E4 File Offset: 0x00002BE4
		public TaskEventArgs(TArgument argument)
		{
			this.Argument = argument;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000049F7 File Offset: 0x00002BF7
		internal TaskEventArgs(TArgument argument, TaskMode taskMode) : this(argument)
		{
			this.TaskMode = taskMode;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00004A0C File Offset: 0x00002C0C
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00004A23 File Offset: 0x00002C23
		public TaskMode TaskMode { get; private set; }
	}
}
