using System;

namespace MonoDevelop.Core.LogReporting
{
	// Token: 0x02000236 RID: 566
	public interface ICrashMonitor
	{
		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06001500 RID: 5376
		// (remove) Token: 0x06001501 RID: 5377
		event EventHandler ApplicationExited;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06001502 RID: 5378
		// (remove) Token: 0x06001503 RID: 5379
		event EventHandler<CrashEventArgs> CrashDetected;

		// Token: 0x06001504 RID: 5380
		void Start();

		// Token: 0x06001505 RID: 5381
		void Stop();
	}
}
