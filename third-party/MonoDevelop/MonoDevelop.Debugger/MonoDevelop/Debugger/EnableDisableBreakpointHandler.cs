using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Debugger
{
	internal class EnableDisableBreakpointHandler : CommandHandler
	{
		protected override void Run()
		{
			BreakpointStore breakpoints = DebuggingService.Breakpoints;
			lock (breakpoints)
			{
				foreach (Breakpoint item in breakpoints.GetBreakpointsAtFileLine(IdeApp.Workbench.ActiveDocument.FileName, IdeApp.Workbench.ActiveDocument.Editor.Caret.Line))
				{
					item.Enabled = !item.Enabled;
				}
			}
		}

		protected override void Update(CommandInfo info)
		{
			BreakpointStore breakpoints = DebuggingService.Breakpoints;
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Breakpoints);
			if (IdeApp.Workbench.ActiveDocument != null && IdeApp.Workbench.ActiveDocument.Editor != null && IdeApp.Workbench.ActiveDocument.FileName != FilePath.Null && !breakpoints.IsReadOnly)
			{
				lock (breakpoints)
				{
					info.Enabled = breakpoints.GetBreakpointsAtFileLine(IdeApp.Workbench.ActiveDocument.FileName, IdeApp.Workbench.ActiveDocument.Editor.Caret.Line).Count > 0;
					return;
				}
			}
			info.Enabled = false;
		}
	}
}
