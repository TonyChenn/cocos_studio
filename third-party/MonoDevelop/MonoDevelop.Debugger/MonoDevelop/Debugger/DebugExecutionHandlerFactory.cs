using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger
{
	internal class DebugExecutionHandlerFactory : IExecutionHandler
	{
		public bool CanExecute(ExecutionCommand command)
		{
			return DebuggingService.CanDebugCommand(command);
		}

		public IProcessAsyncOperation Execute(ExecutionCommand command, IConsole console)
		{
			if (!CanExecute(command))
			{
				return null;
			}
			DebugExecutionHandler debugExecutionHandler = new DebugExecutionHandler(null);
			return debugExecutionHandler.Execute(command, console);
		}
	}
}
