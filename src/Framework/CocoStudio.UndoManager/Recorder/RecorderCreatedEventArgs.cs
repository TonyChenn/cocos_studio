using System;

namespace CocoStudio.UndoManager.Recorder
{
	// Token: 0x0200001A RID: 26
	public class RecorderCreatedEventArgs : EventArgs
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004948 File Offset: 0x00002B48
		// (set) Token: 0x060000CB RID: 203 RVA: 0x0000495F File Offset: 0x00002B5F
		public INotifyStateChanged Item { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004968 File Offset: 0x00002B68
		// (set) Token: 0x060000CD RID: 205 RVA: 0x0000497F File Offset: 0x00002B7F
		public UndoTask Task { get; private set; }

		// Token: 0x060000CE RID: 206 RVA: 0x00004988 File Offset: 0x00002B88
		public RecorderCreatedEventArgs(INotifyStateChanged item, UndoTask task)
		{
			this.Item = item;
			this.Task = task;
		}
	}
}
