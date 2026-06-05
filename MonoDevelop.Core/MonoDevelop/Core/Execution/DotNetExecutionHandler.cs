using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000B2 RID: 178
	public class DotNetExecutionHandler : IExecutionHandler
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x000168AE File Offset: 0x00014AAE
		public bool CanExecute(ExecutionCommand command)
		{
			return command is DotNetExecutionCommand;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000168BC File Offset: 0x00014ABC
		public IProcessAsyncOperation Execute(ExecutionCommand command, IConsole console)
		{
			DotNetExecutionCommand dotNetExecutionCommand = (DotNetExecutionCommand)command;
			if (dotNetExecutionCommand.TargetRuntime == null)
			{
				dotNetExecutionCommand.TargetRuntime = Runtime.SystemAssemblyService.DefaultRuntime;
			}
			return dotNetExecutionCommand.TargetRuntime.GetExecutionHandler().Execute(dotNetExecutionCommand, console);
		}
	}
}
