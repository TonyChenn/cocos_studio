using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class StepOverHandler : CommandHandler
	{
		protected override void Run()
		{
			DebuggingService.StepOver();
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = DebuggingService.IsPaused;
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Stepping);
		}
	}
}
