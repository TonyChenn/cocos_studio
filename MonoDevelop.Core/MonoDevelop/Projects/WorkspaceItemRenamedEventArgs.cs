using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000162 RID: 354
	public class WorkspaceItemRenamedEventArgs : WorkspaceItemEventArgs
	{
		// Token: 0x06000D5A RID: 3418 RVA: 0x00030DB5 File Offset: 0x0002EFB5
		public WorkspaceItemRenamedEventArgs(WorkspaceItem item, string oldName, string newName) : base(item)
		{
			this.oldName = oldName;
			this.newName = newName;
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x00030DCC File Offset: 0x0002EFCC
		public string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00030DD4 File Offset: 0x0002EFD4
		public string NewName
		{
			get
			{
				return this.newName;
			}
		}

		// Token: 0x040003EB RID: 1003
		private string oldName;

		// Token: 0x040003EC RID: 1004
		private string newName;
	}
}
