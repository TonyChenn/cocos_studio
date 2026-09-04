using Gtk;
using MonoDevelop.Ide.Gui.Dialogs;

namespace MonoDevelop.AnalysisCore.Gui
{
	public class AnalysisOptionsPanel : OptionsPanel
	{
		private AnalysisOptionsWidget widget;

		public override Widget CreatePanelWidget()
		{
			return widget = new AnalysisOptionsWidget
			{
				AnalysisEnabled = AnalysisOptions.AnalysisEnabled,
				UnitTestIntegrationEnabled = AnalysisOptions.EnableUnitTestEditorIntegration
			};
		}

		public override void ApplyChanges()
		{
			AnalysisOptions.AnalysisEnabled.Set(widget.AnalysisEnabled);
			AnalysisOptions.EnableUnitTestEditorIntegration.Set(widget.UnitTestIntegrationEnabled);
		}
	}
}
