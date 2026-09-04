using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class ClearAllBreakpointsHandler : CommandHandler
	{
		protected override void Run()
		{
			BreakpointStore breakpoints = DebuggingService.Breakpoints;
			lock (breakpoints)
			{
				breakpoints.Clear();
			}
		}

		protected override void Update(CommandInfo info)
		{
			BreakpointStore breakpoints = DebuggingService.Breakpoints;
			lock (breakpoints)
			{
				info.Enabled = !breakpoints.IsReadOnly && breakpoints.Count > 0;
			}
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Breakpoints);
		}
	}
}
