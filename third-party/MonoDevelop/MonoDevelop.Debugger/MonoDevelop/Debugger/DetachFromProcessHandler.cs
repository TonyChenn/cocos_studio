using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Debugger
{
	internal class DetachFromProcessHandler : CommandHandler
	{
		protected override void Run()
		{
			if (MessageService.Confirm(GettextCatalog.GetString("Do you want to detach from the process being debugged?"), new AlertButton(GettextCatalog.GetString("Detach")), confirmIsDefault: true))
			{
				DebuggingService.DebuggerSession.Detach();
			}
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = DebuggingService.IsDebugging && DebuggingService.DebuggerSession.AttachedToProcess;
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Attaching);
		}
	}
}
