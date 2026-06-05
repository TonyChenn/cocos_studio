using System;

namespace MonoDevelop.Core
{
	// Token: 0x0200003C RID: 60
	public class FileEventInfo
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000089A4 File Offset: 0x00006BA4
		public FilePath FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000202 RID: 514 RVA: 0x000089AC File Offset: 0x00006BAC
		public bool IsDirectory
		{
			get
			{
				return this.isDirectory;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000203 RID: 515 RVA: 0x000089B4 File Offset: 0x00006BB4
		public bool AutoReload
		{
			get
			{
				return this.autoReload;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000089BC File Offset: 0x00006BBC
		public FileEventInfo(FilePath fileName, bool isDirectory, bool autoReload)
		{
			this.fileName = fileName;
			this.isDirectory = isDirectory;
			this.autoReload = autoReload;
		}

		// Token: 0x040000BC RID: 188
		private FilePath fileName;

		// Token: 0x040000BD RID: 189
		private bool isDirectory;

		// Token: 0x040000BE RID: 190
		private bool autoReload;
	}
}
