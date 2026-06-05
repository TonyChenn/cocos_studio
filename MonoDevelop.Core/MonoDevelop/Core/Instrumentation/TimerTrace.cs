using System;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000E2 RID: 226
	[Serializable]
	public class TimerTrace
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x000207B4 File Offset: 0x0001E9B4
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x000207BC File Offset: 0x0001E9BC
		public DateTime Timestamp { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x000207C5 File Offset: 0x0001E9C5
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x000207CD File Offset: 0x0001E9CD
		public string Message { get; set; }

		// Token: 0x0400028D RID: 653
		internal TimerTrace Next;
	}
}
