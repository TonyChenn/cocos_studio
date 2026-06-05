using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000130 RID: 304
	internal class ProjectFilePathChangedEventArgs : ProjectFileVirtualPathChangedEventArgs
	{
		// Token: 0x06000B7B RID: 2939 RVA: 0x0002B444 File Offset: 0x00029644
		public ProjectFilePathChangedEventArgs(ProjectFile projectFile, FilePath oldPath, FilePath newPath, FilePath oldVirtualPath, FilePath newVirtualPath) : base(projectFile, oldVirtualPath, newVirtualPath)
		{
			this.OldPath = oldPath;
			this.NewPath = newPath;
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0002B45F File Offset: 0x0002965F
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0002B467 File Offset: 0x00029667
		public FilePath OldPath { get; private set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0002B470 File Offset: 0x00029670
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0002B478 File Offset: 0x00029678
		public FilePath NewPath { get; private set; }
	}
}
