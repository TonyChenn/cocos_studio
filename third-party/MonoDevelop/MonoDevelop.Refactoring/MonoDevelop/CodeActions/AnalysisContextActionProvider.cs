using System;
using System.Collections.Generic;
using System.Threading;
using ICSharpCode.NRefactory;
using MonoDevelop.AnalysisCore;
using MonoDevelop.AnalysisCore.Fixes;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.CodeActions
{
	internal class AnalysisContextActionProvider : CodeActionProvider
	{
		internal class AnalysisCodeAction : CodeAction
		{
			public IAnalysisFixAction Action { get; private set; }

			public Result Result { get; private set; }

			public override bool SupportsBatchRunning => Action.SupportsBatchFix;

			public AnalysisCodeAction(IAnalysisFixAction action, Result result)
			{
				Action = action;
				base.Title = action.Label;
				Result = result;
				base.IdString = action.IdString;
			}

			public override void Run(IRefactoringContext context, object script)
			{
				Action.Fix();
			}

			public override void BatchRun(Document document, TextLocation loc)
			{
				Action.BatchFix();
			}

			public void ShowOptions(object sender, EventArgs e)
			{
				if (Result is InspectorResults inspectorResults)
				{
					inspectorResults.ShowResultOptionsDialog();
				}
			}

			public void HideCodeIssue(object sender, EventArgs e)
			{
				if (Result is InspectorResults inspectorResults)
				{
					inspectorResults.Inspector.SetIsEnabled(isEnabled: false);
				}
			}
		}

		public Result Result { get; private set; }

		public IAnalysisFixAction Action { get; private set; }

		public AnalysisContextActionProvider(Result result, IAnalysisFixAction action)
		{
			Result = result;
			Action = action;
			base.Description = result.Message;
		}

		public override IEnumerable<CodeAction> GetActions(Document document, object refactoringContext, TextLocation loc, CancellationToken cancellationToken)
		{
			yield return new AnalysisCodeAction(Action, Result)
			{
				DocumentRegion = Action.DocumentRegion
			};
		}
	}
}
