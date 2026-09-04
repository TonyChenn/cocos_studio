using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000053 RID: 83
	public class Advert
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000B0B0 File Offset: 0x000092B0
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x0000B0B8 File Offset: 0x000092B8
		public string ImageUrl { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000B0C1 File Offset: 0x000092C1
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x0000B0C9 File Offset: 0x000092C9
		public string Url { get; set; }

		// Token: 0x060002C4 RID: 708 RVA: 0x0000B0D4 File Offset: 0x000092D4
		public Advert()
		{
			this.ImageUrl = (this.Url = string.Empty);
		}
	}
}
