using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000269 RID: 617
	public interface IDebugConsole : IConsole, IDisposable
	{
		// Token: 0x06001658 RID: 5720
		void Debug(int level, string category, string message);
	}
}
