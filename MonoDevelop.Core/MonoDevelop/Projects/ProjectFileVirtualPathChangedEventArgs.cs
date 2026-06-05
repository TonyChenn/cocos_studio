using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x0200012F RID: 303
	internal class ProjectFileVirtualPathChangedEventArgs : EventArgs
	{
		// Token: 0x06000B74 RID: 2932 RVA: 0x0002B3F4 File Offset: 0x000295F4
		public ProjectFileVirtualPathChangedEventArgs(ProjectFile projectFile, FilePath oldPath, FilePath newPath)
		{
			this.ProjectFile = projectFile;
			this.OldVirtualPath = oldPath;
			this.NewVirtualPath = newPath;
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0002B411 File Offset: 0x00029611
		// (set) Token: 0x06000B76 RID: 2934 RVA: 0x0002B419 File Offset: 0x00029619
		public ProjectFile ProjectFile { get; private set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x0002B422 File Offset: 0x00029622
		// (set) Token: 0x06000B78 RID: 2936 RVA: 0x0002B42A File Offset: 0x0002962A
		public FilePath OldVirtualPath { get; private set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x0002B433 File Offset: 0x00029633
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x0002B43B File Offset: 0x0002963B
		public FilePath NewVirtualPath { get; private set; }
	}
}
