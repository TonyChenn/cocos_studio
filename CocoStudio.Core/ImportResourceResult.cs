using System;
using System.Collections.Generic;
using System.Threading;
using CocoStudio.ControlLib;
using CocoStudio.Projects;

namespace CocoStudio.Core
{
	// Token: 0x0200002B RID: 43
	public class ImportResourceResult
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00008298 File Offset: 0x00006498
		// (set) Token: 0x0600019A RID: 410 RVA: 0x000082AF File Offset: 0x000064AF
		public List<ResourceItem> ImportResources { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000082B8 File Offset: 0x000064B8
		// (set) Token: 0x0600019C RID: 412 RVA: 0x000082CF File Offset: 0x000064CF
		public List<ResourceItem> AddResourcePanelItems { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000082D8 File Offset: 0x000064D8
		// (set) Token: 0x0600019E RID: 414 RVA: 0x000082EF File Offset: 0x000064EF
		internal DialogResult DialogResult { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000082F8 File Offset: 0x000064F8
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x0000830F File Offset: 0x0000650F
		public IEnumerable<string> FileTypeSuffix { get; set; }

		// Token: 0x060001A1 RID: 417 RVA: 0x00008318 File Offset: 0x00006518
		public ImportResourceResult()
		{
			this.ImportResources = new List<ResourceItem>();
			this.AddResourcePanelItems = new List<ResourceItem>();
			this.DialogResult = new DialogResult();
			this.Token = CancellationToken.None;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00008354 File Offset: 0x00006554
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000836B File Offset: 0x0000656B
		internal CancellationToken Token { get; set; }
	}
}
