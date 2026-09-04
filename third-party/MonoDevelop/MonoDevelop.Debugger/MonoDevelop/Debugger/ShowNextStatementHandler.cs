using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class ShowNextStatementHandler : CommandHandler
	{
		protected override void Update(CommandInfo info)
		{
			info.Enabled = DebuggingService.IsPaused && DebuggingService.DebuggerSession.CanSetNextStatement;
			info.Visible = DebuggingService.IsPaused;
		}

		protected override void Run()
		{
			DebuggingService.ShowNextStatement();
		}
	}
}
