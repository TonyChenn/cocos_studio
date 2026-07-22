using System;
using System.Collections.Generic;

namespace Modules.Communal.CocosCodeIDE
{
	// Token: 0x02000006 RID: 6
	public class ConfigJson
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002CCE File Offset: 0x00000ECE
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002CD6 File Offset: 0x00000ED6
		public string host { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002CDF File Offset: 0x00000EDF
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002CE7 File Offset: 0x00000EE7
		public List<ProcessInfo> processes { get; set; }
	}
}
