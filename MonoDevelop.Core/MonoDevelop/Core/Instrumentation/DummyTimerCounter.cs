using System;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000DF RID: 223
	internal class DummyTimerCounter : ITimeTracker, IDisposable
	{
		// Token: 0x060007E5 RID: 2021 RVA: 0x00020588 File Offset: 0x0001E788
		public void Trace(string message)
		{
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002058A File Offset: 0x0001E78A
		public void End()
		{
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002058C File Offset: 0x0001E78C
		public void Dispose()
		{
		}
	}
}
