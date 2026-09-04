using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class ContinueDebugHandler : CommandHandler
	{
		protected override void Run()
		{
			DebuggingService.Resume();
		}

		protected override void Update(CommandInfo info)
		{
			info.Visible = !DebuggingService.IsRunning;
			info.Enabled = DebuggingService.IsConnected && DebuggingService.IsPaused;
		}
	}
}
