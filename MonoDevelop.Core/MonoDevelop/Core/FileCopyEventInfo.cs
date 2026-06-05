using System;

namespace MonoDevelop.Core
{
	// Token: 0x0200003E RID: 62
	public class FileCopyEventInfo : EventArgs
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00008A00 File Offset: 0x00006C00
		public FilePath SourceFile
		{
			get
			{
				return this.sourceFile;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00008A08 File Offset: 0x00006C08
		public FilePath TargetFile
		{
			get
			{
				return this.targetFile;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00008A10 File Offset: 0x00006C10
		public bool IsDirectory
		{
			get
			{
				return this.isDirectory;
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008A18 File Offset: 0x00006C18
		public FileCopyEventInfo(FilePath sourceFile, FilePath targetFile, bool isDirectory)
		{
			this.sourceFile = sourceFile;
			this.targetFile = targetFile;
			this.isDirectory = isDirectory;
		}

		// Token: 0x040000BF RID: 191
		private FilePath sourceFile;

		// Token: 0x040000C0 RID: 192
		private FilePath targetFile;

		// Token: 0x040000C1 RID: 193
		private bool isDirectory;
	}
}
