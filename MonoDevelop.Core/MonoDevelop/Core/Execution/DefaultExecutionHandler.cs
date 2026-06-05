using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200002A RID: 42
	internal class DefaultExecutionHandler : IExecutionHandler
	{
		// Token: 0x0600016A RID: 362 RVA: 0x00006896 File Offset: 0x00004A96
		public bool CanExecute(ExecutionCommand command)
		{
			return Runtime.ProcessService.GetDefaultExecutionHandler(command) != null;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000068AC File Offset: 0x00004AAC
		public IProcessAsyncOperation Execute(ExecutionCommand command, IConsole console)
		{
			IExecutionHandler defaultExecutionHandler = Runtime.ProcessService.GetDefaultExecutionHandler(command);
			return defaultExecutionHandler.Execute(command, console);
		}
	}
}
