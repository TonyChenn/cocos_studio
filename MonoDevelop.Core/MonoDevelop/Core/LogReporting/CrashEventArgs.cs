using System;

namespace MonoDevelop.Core.LogReporting
{
	// Token: 0x02000235 RID: 565
	public class CrashEventArgs : EventArgs
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060014FD RID: 5373 RVA: 0x00056290 File Offset: 0x00054490
		// (set) Token: 0x060014FE RID: 5374 RVA: 0x00056298 File Offset: 0x00054498
		public string CrashLogPath { get; private set; }

		// Token: 0x060014FF RID: 5375 RVA: 0x000562A1 File Offset: 0x000544A1
		public CrashEventArgs(string crashLogPath)
		{
			this.CrashLogPath = crashLogPath;
		}
	}
}
