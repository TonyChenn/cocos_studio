using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class StepOutHandler : CommandHandler
	{
		protected override void Run()
		{
			DebuggingService.StepOut();
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = DebuggingService.IsPaused;
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Stepping);
		}
	}
}
