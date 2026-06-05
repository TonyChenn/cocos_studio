using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000173 RID: 371
	public class UnknownWorkspaceItem : WorkspaceItem
	{
		// Token: 0x06000E87 RID: 3719 RVA: 0x00035B17 File Offset: 0x00033D17
		public UnknownWorkspaceItem()
		{
			this.NeedsReload = false;
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x00035B31 File Offset: 0x00033D31
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x00035B39 File Offset: 0x00033D39
		public string LoadError
		{
			get
			{
				return this.loadError;
			}
			set
			{
				this.loadError = value;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x00035B42 File Offset: 0x00033D42
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x00035B4A File Offset: 0x00033D4A
		public bool UnloadedEntry
		{
			get
			{
				return this.unloaded;
			}
			set
			{
				this.unloaded = value;
			}
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00035B53 File Offset: 0x00033D53
		protected internal override void OnSave(IProgressMonitor monitor)
		{
			Services.ProjectService.InternalWriteWorkspaceItem(monitor, this.FileName, this);
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00035B68 File Offset: 0x00033D68
		// (set) Token: 0x06000E8E RID: 3726 RVA: 0x00035B9E File Offset: 0x00033D9E
		public override string Name
		{
			get
			{
				if (!this.FileName.IsNullOrEmpty)
				{
					return this.FileName.FileNameWithoutExtension;
				}
				return GettextCatalog.GetString("Unknown entry");
			}
			set
			{
			}
		}

		// Token: 0x0400042A RID: 1066
		private string loadError = string.Empty;

		// Token: 0x0400042B RID: 1067
		private bool unloaded;
	}
}
