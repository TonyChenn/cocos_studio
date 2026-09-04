using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class StepIntoHandler : CommandHandler
	{
		protected override void Run()
		{
			DebuggingService.StepInto();
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = DebuggingService.IsPaused;
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Stepping);
		}
	}
}
