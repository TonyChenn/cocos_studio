using ICSharpCode.NRefactory.Refactoring;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.CodeIssues;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Dialogs;

namespace MonoDevelop.AnalysisCore.Fixes
{
	public class InspectorResults : GenericResults
	{
		public BaseCodeIssueProvider Inspector { get; private set; }

		public override bool HasOptionsDialog => true;

		public override string OptionsTitle => GetTitle(Inspector);

		public InspectorResults(BaseCodeIssueProvider inspector, DomRegion region, string message, Severity level, IssueMarker mark, params GenericFix[] fixes)
			: base(region, message, level, mark, fixes)
		{
			Inspector = inspector;
		}

		public override void ShowResultOptionsDialog()
		{
			IdeApp.Workbench.ShowGlobalPreferencesDialog(null, "C#", delegate(OptionsDialog dialog)
			{
				dialog.GetPanel<CodeIssuePanel>("C#")?.Widget.SelectCodeIssue(Inspector.IdString);
			});
		}

		public static string GetTitle(BaseCodeIssueProvider inspector)
		{
			if (inspector.Parent == null)
			{
				return inspector.Title;
			}
			return inspector.Parent.Title + " -> " + inspector.Title;
		}
	}
}
