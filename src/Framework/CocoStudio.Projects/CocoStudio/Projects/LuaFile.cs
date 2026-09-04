using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000044 RID: 68
	[DataItem(Name = "Lua")]
	public class LuaFile : CodeFile
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x00007537 File Offset: 0x00005737
		protected LuaFile()
		{
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000753F File Offset: 0x0000573F
		public LuaFile(FilePath file) : base(file)
		{
		}

		// Token: 0x0400007B RID: 123
		public const string FileSuffix = ".lua";
	}
}
