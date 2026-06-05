using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000017 RID: 23
	// (Invoke) Token: 0x060000AE RID: 174
	public delegate IProcessAsyncOperation ExternalConsoleHandler(string command, string arguments, string workingDirectory, IDictionary<string, string> environmentVariables, string title, bool pauseWhenFinished);
}
