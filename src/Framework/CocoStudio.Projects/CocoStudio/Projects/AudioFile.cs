using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000063 RID: 99
	[DataItem(Name = "Audio")]
	public class AudioFile : ResourceFile
	{
		// Token: 0x060002DF RID: 735 RVA: 0x0000B0D7 File Offset: 0x000092D7
		public AudioFile(FilePath fileName) : base(fileName)
		{
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000B0E0 File Offset: 0x000092E0
		public AudioFile()
		{
		}
	}
}
