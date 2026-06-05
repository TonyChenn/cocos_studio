using System;
using MonoDevelop.Core;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x02000167 RID: 359
	internal class DummySolutionFolderHandler : ISolutionItemHandler, IDisposable
	{
		// Token: 0x06000E45 RID: 3653 RVA: 0x00035071 File Offset: 0x00033271
		public DummySolutionFolderHandler(SolutionFolder folder)
		{
			this.folder = folder;
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00035080 File Offset: 0x00033280
		public string ItemId
		{
			get
			{
				return this.folder.Name;
			}
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0003508D File Offset: 0x0003328D
		public BuildResult RunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00035094 File Offset: 0x00033294
		public void Save(IProgressMonitor monitor)
		{
			throw new NotImplementedException();
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0003509B File Offset: 0x0003329B
		public bool SyncFileName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0003509E File Offset: 0x0003329E
		public void OnModified(string hint)
		{
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x000350A0 File Offset: 0x000332A0
		public void Dispose()
		{
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x000350A2 File Offset: 0x000332A2
		public object GetService(Type t)
		{
			return null;
		}

		// Token: 0x0400041D RID: 1053
		private SolutionFolder folder;
	}
}
