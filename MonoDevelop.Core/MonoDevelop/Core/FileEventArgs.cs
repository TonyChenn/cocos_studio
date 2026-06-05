using System;
using System.Collections.Generic;

namespace MonoDevelop.Core
{
	// Token: 0x0200003B RID: 59
	public class FileEventArgs : EventArgsChain<FileEventInfo>
	{
		// Token: 0x060001FC RID: 508 RVA: 0x000088BD File Offset: 0x00006ABD
		public FileEventArgs()
		{
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000088C5 File Offset: 0x00006AC5
		public FileEventArgs(FilePath fileName, bool isDirectory)
		{
			base.Add(new FileEventInfo(fileName, isDirectory, false));
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000088DB File Offset: 0x00006ADB
		public FileEventArgs(FilePath fileName, bool isDirectory, bool autoReload)
		{
			base.Add(new FileEventInfo(fileName, isDirectory, autoReload));
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000088F4 File Offset: 0x00006AF4
		public FileEventArgs(IEnumerable<FilePath> files, bool isDirectory)
		{
			foreach (FilePath fileName in files)
			{
				base.Add(new FileEventInfo(fileName, isDirectory, false));
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000894C File Offset: 0x00006B4C
		public FileEventArgs(IEnumerable<FilePath> files, bool isDirectory, bool autoReload)
		{
			foreach (FilePath fileName in files)
			{
				base.Add(new FileEventInfo(fileName, isDirectory, autoReload));
			}
		}
	}
}
