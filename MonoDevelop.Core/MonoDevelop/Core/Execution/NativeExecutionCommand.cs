using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000B1 RID: 177
	public class NativeExecutionCommand : ProcessExecutionCommand
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x0001687B File Offset: 0x00014A7B
		public NativeExecutionCommand()
		{
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00016883 File Offset: 0x00014A83
		public NativeExecutionCommand(string command) : base(command)
		{
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001688C File Offset: 0x00014A8C
		public NativeExecutionCommand(string command, string arguments) : base(command, arguments)
		{
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00016896 File Offset: 0x00014A96
		public NativeExecutionCommand(string command, string arguments, string workingDirectory) : base(command, arguments, workingDirectory)
		{
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000168A1 File Offset: 0x00014AA1
		public NativeExecutionCommand(string command, string arguments, string workingDirectory, IDictionary<string, string> environmentVariables) : base(command, arguments, workingDirectory, environmentVariables)
		{
		}
	}
}
