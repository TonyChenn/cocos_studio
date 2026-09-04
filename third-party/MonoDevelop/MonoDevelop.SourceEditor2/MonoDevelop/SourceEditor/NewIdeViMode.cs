using Gdk;
using Mono.TextEditor.Vi;
using MonoDevelop.Ide;

namespace MonoDevelop.SourceEditor
{
	public class NewIdeViMode : NewViEditMode
	{
		public NewIdeViMode(ExtensibleTextEditor editor)
		{
			base.editor = editor;
		}

		protected override void HandleKeypress(Key key, uint unicodeKey, ModifierType modifier)
		{
			base.HandleKeypress(key, unicodeKey, modifier);
			IdeApp.Workbench.StatusBar.ShowMessage(base.ViEditor.Message);
		}
	}
}
