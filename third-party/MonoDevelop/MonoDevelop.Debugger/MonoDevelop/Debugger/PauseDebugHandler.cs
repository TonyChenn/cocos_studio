using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class PauseDebugHandler : CommandHandler
	{
		protected override void Run()
		{
			DebuggingService.Pause();
		}

		protected override void Update(CommandInfo info)
		{
			info.Visible = DebuggingService.IsRunning;
			info.Enabled = DebuggingService.IsFeatureSupported(DebuggerFeatures.Pause) && DebuggingService.IsConnected;
		}
	}
}
