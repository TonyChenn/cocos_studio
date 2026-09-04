using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class ShowCurrentExecutionLineCommand : CommandHandler
	{
		protected override void Run()
		{
			DebuggingService.ShowCurrentExecutionLine();
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = DebuggingService.IsPaused;
			info.Visible = DebuggingService.IsDebuggingSupported;
		}
	}
}
