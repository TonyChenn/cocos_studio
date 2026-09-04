using Gtk;
using MonoDevelop.Core;

namespace MonoDevelop.AnalysisCore.Gui
{
	internal class AnalysisOptionsWidget : VBox
	{
		private CheckButton enabledCheck;

		private CheckButton enabledTest;

		public bool AnalysisEnabled
		{
			get
			{
				return enabledCheck.Active;
			}
			set
			{
				enabledCheck.Active = value;
			}
		}

		public bool UnitTestIntegrationEnabled
		{
			get
			{
				return enabledTest.Active;
			}
			set
			{
				enabledTest.Active = value;
			}
		}

		public AnalysisOptionsWidget()
		{
			enabledCheck = new CheckButton(GettextCatalog.GetString("Enable source analysis of open files"));
			PackStart(enabledCheck, expand: false, fill: false, 0u);
			enabledTest = new CheckButton(GettextCatalog.GetString("Enable text editor unit test integration"));
			PackStart(enabledTest, expand: false, fill: false, 0u);
			ShowAll();
		}
	}
}
