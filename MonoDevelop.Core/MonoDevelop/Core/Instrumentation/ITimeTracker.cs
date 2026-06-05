using System;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000DE RID: 222
	public interface ITimeTracker : IDisposable
	{
		// Token: 0x060007E3 RID: 2019
		void Trace(string message);

		// Token: 0x060007E4 RID: 2020
		void End();
	}
}
