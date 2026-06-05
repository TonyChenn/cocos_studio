using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000B5 RID: 181
	internal class MsNetExecutionHandler : NativePlatformExecutionHandler
	{
		// Token: 0x0600062F RID: 1583 RVA: 0x000172A9 File Offset: 0x000154A9
		public override bool CanExecute(ExecutionCommand command)
		{
			return command is DotNetExecutionCommand;
		}
	}
}
