using Gdk;
using Gtk;
using Mono.TextEditor;

namespace MonoDevelop.SourceEditor
{
	public class CompileErrorTooltipProvider : TooltipProvider
	{
		public override TooltipItem GetItem(TextEditor editor, int offset)
		{
			if (!(editor is ExtensibleTextEditor extensibleTextEditor))
			{
				return null;
			}
			string errorInformationAt = extensibleTextEditor.GetErrorInformationAt(offset);
			if (string.IsNullOrEmpty(errorInformationAt))
			{
				return null;
			}
			return new TooltipItem(errorInformationAt, editor.Document.GetLineByOffset(offset));
		}

		protected override Gtk.Window CreateTooltipWindow(TextEditor editor, int offset, ModifierType modifierState, TooltipItem item)
		{
			LanguageItemWindow languageItemWindow = new LanguageItemWindow((ExtensibleTextEditor)editor, modifierState, null, (string)item.Item, null);
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
