using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Debugger
{
	internal class SetNextStatementHandler : CommandHandler
	{
		protected override void Update(CommandInfo info)
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument != null && activeDocument.FileName != FilePath.Null && activeDocument.Editor != null && DebuggingService.IsDebuggingSupported)
			{
				info.Enabled = DebuggingService.IsPaused && DebuggingService.DebuggerSession.CanSetNextStatement;
				info.Visible = DebuggingService.IsPaused;
			}
			else
			{
				info.Visible = false;
				info.Enabled = false;
			}
		}

		protected override void Run()
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			try
			{
				DebuggingService.SetNextStatement(activeDocument.FileName, activeDocument.Editor.Caret.Line, activeDocument.Editor.Caret.Column);
			}
			catch (Exception ex)
			{
				if (ex is NotSupportedException || ex.InnerException is NotSupportedException)
				{
					MessageService.ShowError("Unable to set the next statement to this location.");
					return;
				}
				throw;
			}
		}
	}
}
