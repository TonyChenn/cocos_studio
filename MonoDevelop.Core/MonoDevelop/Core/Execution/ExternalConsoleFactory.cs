using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200002C RID: 44
	public sealed class ExternalConsoleFactory : IConsoleFactory
	{
		// Token: 0x0600016E RID: 366 RVA: 0x000068D5 File Offset: 0x00004AD5
		public IConsole CreateConsole(bool closeOnDispose)
		{
			return new ExternalConsole(closeOnDispose);
		}

		// Token: 0x0400008F RID: 143
		public static ExternalConsoleFactory Instance = new ExternalConsoleFactory();
	}
}
