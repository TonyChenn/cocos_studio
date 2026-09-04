using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Debugger
{
	internal class ToggleBreakpointHandler : CommandHandler
	{
		protected override void Run()
		{
			BreakpointStore breakpoints = DebuggingService.Breakpoints;
			Breakpoint breakpoint;
			lock (breakpoints)
			{
				breakpoint = breakpoints.Toggle(IdeApp.Workbench.ActiveDocument.FileName, IdeApp.Workbench.ActiveDocument.Editor.Caret.Line, IdeApp.Workbench.ActiveDocument.Editor.Caret.Column);
			}
			if (breakpoint != null && breakpoint.Line != IdeApp.Workbench.ActiveDocument.Editor.Caret.Line)
			{
				IdeApp.Workbench.ActiveDocument.Editor.Caret.Line = breakpoint.Line;
			}
		}

		protected override void Update(CommandInfo info)
		{
			info.Visible = DebuggingService.IsFeatureSupported(DebuggerFeatures.Breakpoints);
			info.Enabled = IdeApp.Workbench.ActiveDocument != null && IdeApp.Workbench.ActiveDocument.Editor != null && IdeApp.Workbench.ActiveDocument.FileName != FilePath.Null && !DebuggingService.Breakpoints.IsReadOnly;
		}
	}
}
