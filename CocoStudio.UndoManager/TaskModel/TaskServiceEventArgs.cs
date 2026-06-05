using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x02000027 RID: 39
	public class TaskServiceEventArgs : EventArgs
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00006674 File Offset: 0x00004874
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000668B File Offset: 0x0000488B
		public ITask Task { get; private set; }

		// Token: 0x0600013C RID: 316 RVA: 0x00006694 File Offset: 0x00004894
		public TaskServiceEventArgs()
		{
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000669F File Offset: 0x0000489F
		public TaskServiceEventArgs(ITask task)
		{
			this.Task = task;
		}
	}
}
