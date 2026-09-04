using System.Collections.Generic;
using System.Linq;
using Gdk;
using Gtk;
using MonoDevelop.CodeActions;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.SourceEditor;

namespace MonoDevelop.AnalysisCore
{
	internal class ShowFixesHandler : CommandHandler
	{
		protected override void Update(CommandInfo info)
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			if (activeDocument == null || activeDocument.Editor == null)
			{
				info.Enabled = false;
				return;
			}
			CodeActionEditorExtension content = activeDocument.GetContent<CodeActionEditorExtension>();
			if (content == null)
			{
				info.Enabled = false;
				return;
			}
			List<CodeAction> currentFixes = content.GetCurrentFixes();
			info.Enabled = currentFixes.Any();
		}

		protected override void Run()
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			SourceEditorView content = activeDocument.GetContent<SourceEditorView>();
			if (content == null)
			{
				LoggingService.LogWarning("ShowFixesHandler could not find a SourceEditorView");
				return;
			}
			ExtensibleTextEditor widget = content.TextEditor;
			Point pt = content.DocumentToScreenLocation(activeDocument.Editor.Caret.Location);
			CommandEntrySet commandEntrySet = new CommandEntrySet();
			commandEntrySet.AddItem(AnalysisCommands.FixOperations);
			Menu menu = IdeApp.CommandService.CreateMenu(commandEntrySet);
			menu.Popup(null, null, delegate(Menu mn, out int x, out int y, out bool push_in)
			{
				x = pt.X;
				y = pt.Y;
				push_in = true;
				if (y + mn.Requisition.Height > widget.Screen.Height)
				{
					y -= mn.Requisition.Height + (int)widget.LineHeight;
				}
			}, 0u, Gtk.Global.CurrentEventTime);
		}
	}
}
