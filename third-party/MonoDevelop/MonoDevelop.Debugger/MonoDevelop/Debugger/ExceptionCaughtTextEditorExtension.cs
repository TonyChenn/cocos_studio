using Gdk;
using MonoDevelop.Ide.Gui.Content;

namespace MonoDevelop.Debugger
{
	internal class ExceptionCaughtTextEditorExtension : TextEditorExtension
	{
		public override bool KeyPress(Key key, char keyChar, ModifierType modifier)
		{
			if (key == Key.Escape && DebuggingService.ExceptionCaughtMessage != null && !DebuggingService.ExceptionCaughtMessage.IsMinimized && DebuggingService.ExceptionCaughtMessage.File.CanonicalPath == base.Document.FileName.CanonicalPath)
			{
				DebuggingService.ExceptionCaughtMessage.ShowMiniButton();
				return true;
			}
			return base.KeyPress(key, keyChar, modifier);
		}
	}
}
