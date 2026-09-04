using Gtk;
using MonoDevelop.Ide.Gui.Dialogs;

namespace MonoDevelop.Debugger
{
	public class DebuggerOptionsPanel : OptionsPanel
	{
		private DebuggerOptionsPanelWidget w;

		public override Widget CreatePanelWidget()
		{
			return w = new DebuggerOptionsPanelWidget();
		}

		public override void ApplyChanges()
		{
			w.Store();
		}
	}
}
