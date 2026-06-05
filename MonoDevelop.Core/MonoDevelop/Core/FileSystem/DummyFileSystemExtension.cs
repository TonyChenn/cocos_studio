using System;

namespace MonoDevelop.Core.FileSystem
{
	// Token: 0x02000040 RID: 64
	internal class DummyFileSystemExtension : FileSystemExtension
	{
		// Token: 0x0600021F RID: 543 RVA: 0x00008C54 File Offset: 0x00006E54
		public override bool CanHandlePath(FilePath path, bool isDirectory)
		{
			return false;
		}
	}
}
