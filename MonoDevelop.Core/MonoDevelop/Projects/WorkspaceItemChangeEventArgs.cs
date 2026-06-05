using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200016C RID: 364
	public class WorkspaceItemChangeEventArgs : WorkspaceItemEventArgs
	{
		// Token: 0x06000E5E RID: 3678 RVA: 0x00035395 File Offset: 0x00033595
		public WorkspaceItemChangeEventArgs(WorkspaceItem item, bool reloading) : base(item)
		{
			this.reloading = reloading;
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x000353A5 File Offset: 0x000335A5
		public bool Reloading
		{
			get
			{
				return this.reloading;
			}
		}

		// Token: 0x04000421 RID: 1057
		private bool reloading;
	}
}
