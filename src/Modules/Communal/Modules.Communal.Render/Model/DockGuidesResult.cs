using System;
using CocoStudio.Model.ViewModel;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000026 RID: 38
	internal class DockGuidesResult
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000854C File Offset: 0x0000674C
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00008563 File Offset: 0x00006763
		public float Offset { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000856C File Offset: 0x0000676C
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00008583 File Offset: 0x00006783
		public float Distance { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000858C File Offset: 0x0000678C
		// (set) Token: 0x0600014A RID: 330 RVA: 0x000085A3 File Offset: 0x000067A3
		public GuidesObject Guides { get; set; }
	}
}
