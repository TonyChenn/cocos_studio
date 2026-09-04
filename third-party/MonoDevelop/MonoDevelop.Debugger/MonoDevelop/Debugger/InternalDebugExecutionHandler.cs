using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger
{
	internal class InternalDebugExecutionHandler : IExecutionHandler
	{
		private readonly DebuggerEngine engine;

		public InternalDebugExecutionHandler(DebuggerEngine engine)
		{
			this.engine = engine;
		}

		public bool CanExecute(ExecutionCommand command)
		{
			return engine.CanDebugCommand(command);
		}

		public IProcessAsyncOperation Execute(ExecutionCommand command, IConsole console)
		{
			DebugExecutionHandler debugExecutionHandler = new DebugExecutionHandler(engine);
			return debugExecutionHandler.Execute(command, console);
		}
	}
}
