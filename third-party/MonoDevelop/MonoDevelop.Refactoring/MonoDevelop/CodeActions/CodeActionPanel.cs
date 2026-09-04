using Gtk;
using MonoDevelop.Ide.Gui.Dialogs;

namespace MonoDevelop.CodeActions
{
	internal class CodeActionPanel : OptionsPanel
	{
		private ContextActionPanelWidget widget;

		public override Widget CreatePanelWidget()
		{
			return widget = new ContextActionPanelWidget("text/x-csharp");
		}

		public override void ApplyChanges()
		{
			widget.ApplyChanges();
		}
	}
}
