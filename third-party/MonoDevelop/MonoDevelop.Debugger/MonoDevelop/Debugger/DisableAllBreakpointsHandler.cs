using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class DisableAllBreakpointsHandler : CommandHandler
	{
		protected override void Run()
		{
			BreakpointStore breakpoints = DebuggingService.Breakpoints;
			bool enabled = false;
			lock (breakpoints)
			{
				foreach (BreakEvent item in breakpoints)
				{
					if (!item.Enabled)
					{
						enabled = true;
						break;
					}
				}
				foreach (BreakEvent item2 in breakpoints)
				{
					item2.Enabled = enabled;
				}
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
