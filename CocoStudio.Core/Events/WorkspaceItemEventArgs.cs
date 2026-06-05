using System;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	// Token: 0x02000023 RID: 35
	public class WorkspaceItemEventArgs : EventArgs
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00006258 File Offset: 0x00004458
		public WorkspaceItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006270 File Offset: 0x00004470
		public WorkspaceItemEventArgs(WorkspaceItem item)
		{
			this.item = item;
		}

		// Token: 0x040000D5 RID: 213
		private WorkspaceItem item;
	}
}
