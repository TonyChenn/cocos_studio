using System;
using System.Collections.ObjectModel;

namespace CocoStudio.Projects
{
	// Token: 0x02000089 RID: 137
	public class Workspace : WorkspaceItem
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0000DCD0 File Offset: 0x0000BED0
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x0000DCD8 File Offset: 0x0000BED8
		public ObservableCollection<WorkspaceItem> Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
			}
		}

		// Token: 0x04000121 RID: 289
		private ObservableCollection<WorkspaceItem> items;
	}
}
