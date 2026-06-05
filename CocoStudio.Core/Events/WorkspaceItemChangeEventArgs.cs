using System;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	// Token: 0x02000058 RID: 88
	public class WorkspaceItemChangeEventArgs : WorkspaceItemEventArgs
	{
		// Token: 0x06000378 RID: 888 RVA: 0x000101B0 File Offset: 0x0000E3B0
		public WorkspaceItemChangeEventArgs(WorkspaceItem item, bool reloading) : base(item)
		{
			this.reloading = reloading;
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000379 RID: 889 RVA: 0x000101C4 File Offset: 0x0000E3C4
		public bool Reloading
		{
			get
			{
				return this.reloading;
			}
		}

		// Token: 0x04000181 RID: 385
		private bool reloading;
	}
}
