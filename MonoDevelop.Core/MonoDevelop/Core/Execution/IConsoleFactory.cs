using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200002B RID: 43
	public interface IConsoleFactory
	{
		// Token: 0x0600016D RID: 365
		IConsole CreateConsole(bool closeOnDispose);
	}
}
