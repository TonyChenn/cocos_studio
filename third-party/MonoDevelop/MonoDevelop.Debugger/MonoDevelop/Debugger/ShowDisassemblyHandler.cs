using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class ShowDisassemblyHandler : CommandHandler
	{
		protected override void Run()
		{
			DebuggingService.ShowDisassembly();
		}

		protected override void Update(CommandInfo info)
		{
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Disassembly);
		}
	}
}
