using System;
using MonoDevelop.Core.Logging;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200000C RID: 12
	public interface IProcessHostController
	{
		// Token: 0x0600004A RID: 74
		ILogger GetLogger();

		// Token: 0x0600004B RID: 75
		void RegisterHost(IProcessHost processHost);

		// Token: 0x0600004C RID: 76
		void WaitForExit();
	}
}
