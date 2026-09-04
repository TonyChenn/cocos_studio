using System.Collections.Generic;
using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Debugger
{
	internal class ShowBreakpointPropertiesHandler : CommandHandler
	{
		protected override void Run()
		{
			BreakpointStore breakpoints = DebuggingService.Breakpoints;
			IList<Breakpoint> breakpointsAtFileLine;
			lock (breakpoints)
			{
				breakpointsAtFileLine = breakpoints.GetBreakpointsAtFileLine(IdeApp.Workbench.ActiveDocument.FileName, IdeApp.Workbench.ActiveDocument.Editor.Caret.Line);
			}
			if (breakpointsAtFileLine.Count > 0)
			{
				BreakEvent bp = breakpointsAtFileLine[0];
				DebuggingService.ShowBreakpointProperties(ref bp);
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
