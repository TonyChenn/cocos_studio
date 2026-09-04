using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class StopEvaluationHandler : CommandHandler
	{
		protected override void Run()
		{
			DebuggingService.DebuggerSession.CancelAsyncEvaluations();
		}

		protected override void Update(CommandInfo info)
		{
			info.Visible = DebuggingService.IsDebugging && DebuggingService.IsPaused && DebuggingService.DebuggerSession.CanCancelAsyncEvaluations;
		}
	}
}
