using System;
using System.Collections.Generic;

namespace MonoDevelop.Core
{
	// Token: 0x0200003D RID: 61
	public class FileCopyEventArgs : EventArgsChain<FileCopyEventInfo>
	{
		// Token: 0x06000205 RID: 517 RVA: 0x000089D9 File Offset: 0x00006BD9
		public FileCopyEventArgs(IEnumerable<FileCopyEventInfo> args) : base(args)
		{
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000089E2 File Offset: 0x00006BE2
		public FileCopyEventArgs(FilePath sourceFile, FilePath targetFile, bool isDirectory)
		{
			base.Add(new FileCopyEventInfo(sourceFile, targetFile, isDirectory));
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000089F8 File Offset: 0x00006BF8
		public FileCopyEventArgs()
		{
		}
	}
}
