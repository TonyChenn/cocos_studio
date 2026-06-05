using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000169 RID: 361
	public class SolutionItemFileEventArgs : EventArgs
	{
		// Token: 0x06000E52 RID: 3666 RVA: 0x000351B9 File Offset: 0x000333B9
		public SolutionItemFileEventArgs(FilePath file)
		{
			this.file = file;
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x000351C8 File Offset: 0x000333C8
		public FilePath File
		{
			get
			{
				return this.file;
			}
		}

		// Token: 0x0400041F RID: 1055
		private FilePath file;
	}
}
