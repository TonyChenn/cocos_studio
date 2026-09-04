using System;

namespace CocoStudio.Model.Event
{
	// Token: 0x020000A7 RID: 167
	public class OutputInfoEventArgs
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x000184AC File Offset: 0x000166AC
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x000184C3 File Offset: 0x000166C3
		public string Info { get; private set; }

		// Token: 0x0600057F RID: 1407 RVA: 0x000184CC File Offset: 0x000166CC
		public OutputInfoEventArgs(string info)
		{
			this.Info = info;
		}
	}
}
