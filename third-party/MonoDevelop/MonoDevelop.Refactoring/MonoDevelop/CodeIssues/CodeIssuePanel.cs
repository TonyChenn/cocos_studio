using Gtk;
using MonoDevelop.Ide.Gui.Dialogs;

namespace MonoDevelop.CodeIssues
{
	internal class CodeIssuePanel : OptionsPanel
	{
		private CodeIssuePanelWidget widget;

		public CodeIssuePanelWidget Widget
		{
			get
			{
				EnsureWidget();
				return widget;
			}
		}

		private void EnsureWidget()
		{
			if (widget == null)
			{
				widget = new CodeIssuePanelWidget("text/x-csharp");
			}
		}

		public override Widget CreatePanelWidget()
		{
			EnsureWidget();
			return widget;
		}

		public override void ApplyChanges()
		{
			widget.ApplyChanges();
		}
	}
}
