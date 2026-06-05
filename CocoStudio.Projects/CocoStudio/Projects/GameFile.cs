using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x02000080 RID: 128
	public class GameFile : CocosFile
	{
		// Token: 0x060003E2 RID: 994 RVA: 0x0000D3E0 File Offset: 0x0000B5E0
		protected GameFile()
		{
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000D3E8 File Offset: 0x0000B5E8
		public GameFile(FilePath file) : base(file)
		{
			this.Type = "GameProject";
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000D3FC File Offset: 0x0000B5FC
		public GameFile(CocosItemCreateInfo info) : base(info)
		{
			this.Type = info.ContentType;
		}
	}
}
