using System;

namespace MonoDevelop.Core
{
	// Token: 0x0200022F RID: 559
	internal class SystemInformationSection : ISystemInformationProvider
	{
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x00055F9C File Offset: 0x0005419C
		// (set) Token: 0x060014EA RID: 5354 RVA: 0x00055FA4 File Offset: 0x000541A4
		public string Title { get; set; }

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x00055FAD File Offset: 0x000541AD
		// (set) Token: 0x060014EC RID: 5356 RVA: 0x00055FB5 File Offset: 0x000541B5
		public string Description { get; set; }
	}
}
