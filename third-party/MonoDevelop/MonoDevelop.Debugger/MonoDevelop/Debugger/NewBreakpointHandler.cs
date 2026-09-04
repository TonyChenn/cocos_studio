using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;

namespace MonoDevelop.Debugger
{
	internal class NewBreakpointHandler : CommandHandler
	{
		protected override void Run()
		{
			BreakEvent bp = null;
			if (DebuggingService.ShowBreakpointProperties(ref bp))
			{
				BreakpointStore breakpoints = DebuggingService.Breakpoints;
				lock (breakpoints)
				{
					breakpoints.Add(bp);
				}
			}
		}

		protected override void Update(CommandInfo info)
		{
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Breakpoints);
			info.Enabled = !DebuggingService.Breakpoints.IsReadOnly;
		}
	}
}
