using System;

namespace CocoStudio.UndoManager.TaskModel
{
	// Token: 0x02000028 RID: 40
	public class CancellableTaskServiceEventArgs : TaskServiceEventArgs
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000066B4 File Offset: 0x000048B4
		// (set) Token: 0x0600013F RID: 319 RVA: 0x000066CC File Offset: 0x000048CC
		public bool Cancel
		{
			get
			{
				return this.cancelled;
			}
			set
			{
				if (!this.cancelled)
				{
					this.cancelled = value;
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000066F4 File Offset: 0x000048F4
		// (set) Token: 0x06000141 RID: 321 RVA: 0x0000670B File Offset: 0x0000490B
		internal object OwnerKey { get; set; }

		// Token: 0x06000142 RID: 322 RVA: 0x00006714 File Offset: 0x00004914
		public CancellableTaskServiceEventArgs()
		{
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000671F File Offset: 0x0000491F
		public CancellableTaskServiceEventArgs(ITask task) : base(task)
		{
		}

		// Token: 0x04000055 RID: 85
		private bool cancelled;
	}
}
