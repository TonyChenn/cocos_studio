using System;
using System.Collections.Generic;

namespace Modules.Communal.CocosCodeIDE
{
	// Token: 0x02000007 RID: 7
	public class ProcessInfo
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002CF8 File Offset: 0x00000EF8
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002D00 File Offset: 0x00000F00
		public int processID { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002D09 File Offset: 0x00000F09
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002D11 File Offset: 0x00000F11
		public int port { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002D1A File Offset: 0x00000F1A
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002D22 File Offset: 0x00000F22
		public List<string> projectDirs { get; set; }
	}
}
