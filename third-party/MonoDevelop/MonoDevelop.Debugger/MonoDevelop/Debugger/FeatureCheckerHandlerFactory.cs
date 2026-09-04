using System;
using Mono.Debugging.Client;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger
{
	internal class FeatureCheckerHandlerFactory : IExecutionHandler
	{
		public DebuggerFeatures SupportedFeatures { get; set; }

		public bool CanExecute(ExecutionCommand command)
		{
			SupportedFeatures = DebuggingService.GetSupportedFeaturesForCommand(command);
			return SupportedFeatures != DebuggerFeatures.None;
		}

		public IProcessAsyncOperation Execute(ExecutionCommand cmd, IConsole console)
		{
			throw new NotImplementedException();
		}
	}
}
