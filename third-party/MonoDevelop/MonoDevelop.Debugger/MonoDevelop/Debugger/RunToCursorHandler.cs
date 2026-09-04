using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace MonoDevelop.Debugger
{
	internal class RunToCursorHandler : CommandHandler
	{
		protected override void Run()
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (DebuggingService.IsPaused)
			{
				DebuggingService.RunToCursor(activeDocument.FileName, activeDocument.Editor.Caret.Line, activeDocument.Editor.Caret.Column);
				return;
			}
			RunToCursorBreakpoint bp = new RunToCursorBreakpoint(activeDocument.FileName, activeDocument.Editor.Caret.Line, activeDocument.Editor.Caret.Column);
			DebuggingService.Breakpoints.Add(bp);
			DebugHandler.BuildAndDebug();
		}

		protected override void Update(CommandInfo info)
		{
			info.Visible = true;
			if (!DebuggingService.IsDebuggingSupported || !DebuggingService.IsFeatureSupported(DebuggerFeatures.Breakpoints) || DebuggingService.Breakpoints.IsReadOnly)
			{
				info.Enabled = false;
				return;
			}
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument != null && activeDocument.Editor != null && activeDocument.FileName != FilePath.Null)
			{
				if (IdeApp.Workspace.IsOpen)
				{
					IBuildTarget runTarget = DebugHandler.GetRunTarget();
					info.Enabled = runTarget != null && IdeApp.ProjectOperations.CanDebug(runTarget);
				}
				else
				{
					info.Enabled = activeDocument.IsBuildTarget && activeDocument.CanDebug();
				}
			}
			else
			{
				info.Enabled = false;
			}
		}
	}
}
