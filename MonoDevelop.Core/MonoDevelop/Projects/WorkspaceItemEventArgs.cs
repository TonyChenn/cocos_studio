using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000161 RID: 353
	public class WorkspaceItemEventArgs : EventArgs
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00030D9E File Offset: 0x0002EF9E
		public WorkspaceItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00030DA6 File Offset: 0x0002EFA6
		public WorkspaceItemEventArgs(WorkspaceItem item)
		{
			this.item = item;
		}

		// Token: 0x040003EA RID: 1002
		private WorkspaceItem item;
	}
}
