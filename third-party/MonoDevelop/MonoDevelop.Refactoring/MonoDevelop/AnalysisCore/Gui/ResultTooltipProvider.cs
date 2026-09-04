using System.Collections.Generic;
using System.Text;
using Gdk;
using Gtk;
using Mono.TextEditor;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.SourceEditor;

namespace MonoDevelop.AnalysisCore.Gui
{
	internal class ResultTooltipProvider : TooltipProvider
	{
		public override TooltipItem GetItem(TextEditor editor, int offset)
		{
			if (!(editor is ExtensibleTextEditor extensibleTextEditor))
			{
				return null;
			}
			ITextEditorExtension textEditorExtension = extensibleTextEditor.Extension;
			while (textEditorExtension != null && !(textEditorExtension is ResultsEditorExtension))
			{
				textEditorExtension = textEditorExtension.Next;
			}
			if (textEditorExtension == null)
			{
				return null;
			}
			ResultsEditorExtension resultsEditorExtension = (ResultsEditorExtension)textEditorExtension;
			IList<Result> resultsAtOffset = resultsEditorExtension.GetResultsAtOffset(offset);
			if (resultsAtOffset == null || resultsAtOffset.Count == 0)
			{
				return null;
			}
			return new TooltipItem(resultsAtOffset, editor.Document.GetLineByOffset(offset));
		}

		protected override Gtk.Window CreateTooltipWindow(TextEditor editor, int offset, ModifierType modifierState, TooltipItem item)
		{
			IList<Result> list = (IList<Result>)item.Item;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (Result item2 in list)
			{
				if (!flag)
				{
					flag = true;
				}
				else
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(item2.Level.ToString());
				stringBuilder.Append(": ");
				stringBuilder.Append(AmbienceService.EscapeText(item2.Message));
			}
			LanguageItemWindow languageItemWindow = new LanguageItemWindow((ExtensibleTextEditor)editor, modifierState, null, stringBuilder.ToString(), null);
			if (languageItemWindow.IsEmpty)
			{
				return null;
			}
			return languageItemWindow;
		}

		protected override void GetRequiredPosition(TextEditor editor, Gtk.Window tipWindow, out int requiredWidth, out double xalign)
		{
			LanguageItemWindow languageItemWindow = (LanguageItemWindow)tipWindow;
			requiredWidth = languageItemWindow.SetMaxWidth(languageItemWindow.Screen.Width);
			xalign = 0.5;
		}
	}
}
