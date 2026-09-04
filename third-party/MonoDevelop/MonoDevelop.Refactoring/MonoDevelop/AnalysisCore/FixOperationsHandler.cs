using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ICSharpCode.NRefactory.Refactoring;
using MonoDevelop.AnalysisCore.Fixes;
using MonoDevelop.AnalysisCore.Gui;
using MonoDevelop.CodeActions;
using MonoDevelop.CodeIssues;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.AnalysisCore
{
	internal class FixOperationsHandler : CommandHandler
	{
		protected override void Update(CommandArrayInfo info)
		{
			if (GetFixes(out var document, out var results))
			{
				PopulateInfos(info, document, results);
			}
		}

		protected override void Run(object dataItem)
		{
			if (dataItem is Result)
			{
				((Result)dataItem).ShowResultOptionsDialog();
			}
			else if (dataItem is Action)
			{
				((Action)dataItem)();
			}
			else if (dataItem is IAnalysisFixAction analysisFixAction)
			{
				analysisFixAction.Fix();
			}
			else if (dataItem is CodeAction codeAction)
			{
				Document activeDocument = IdeApp.Workbench.ActiveDocument;
				IRefactoringContext refactoringContext = ((activeDocument.ParsedDocument.CreateRefactoringContext != null) ? activeDocument.ParsedDocument.CreateRefactoringContext(activeDocument, default(CancellationToken)) : null);
				using (IDisposable script = refactoringContext.CreateScript())
				{
					codeAction.Run(refactoringContext, script);
				}
			}
		}

		public static bool GetFixes(out Document document, out IList<FixableResult> results)
		{
			results = null;
			document = IdeApp.Workbench.ActiveDocument;
			if (document == null)
			{
				return false;
			}
			ResultsEditorExtension content = document.GetContent<ResultsEditorExtension>();
			if (content == null)
			{
				return false;
			}
			List<FixableResult> list = content.GetResultsAtOffset(document.Editor.Caret.Offset).OfType<FixableResult>().ToList();
			list.Sort(ResultCompareImportanceDesc);
			results = list;
			if (results.Count > 0)
			{
				return true;
			}
			CodeActionEditorExtension content2 = document.GetContent<CodeActionEditorExtension>();
			if (content2 != null)
			{
				List<CodeAction> currentFixes = content2.GetCurrentFixes();
				if (currentFixes != null)
				{
					return currentFixes.Any(CodeActionEditorExtension.IsAnalysisOrErrorFix);
				}
			}
			return false;
		}

		private static int ResultCompareImportanceDesc(Result r1, Result r2)
		{
			int num = ((int)r1.Level).CompareTo((int)r2.Level);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(r1.Message, r2.Message, StringComparison.Ordinal);
		}

		public static void PopulateInfos(CommandArrayInfo infos, Document doc, IEnumerable<FixableResult> results)
		{
			int num = 1;
			CodeActionEditorExtension content = doc.GetContent<CodeActionEditorExtension>();
			List<CodeAction> currentFixes = content.GetCurrentFixes();
			if (currentFixes != null)
			{
				foreach (CodeAction item in currentFixes.Where(CodeActionEditorExtension.IsAnalysisOrErrorFix))
				{
					CodeAction codeAction = item;
					if (!(codeAction is AnalysisContextActionProvider.AnalysisCodeAction))
					{
						string text = codeAction.Title.Replace("_", "__");
						string text2 = ((num <= 10) ? ("_" + num++ % 10 + " " + text) : ("  " + text));
						infos.Add(text2, codeAction);
					}
				}
			}
			foreach (FixableResult result in results)
			{
				bool flag = true;
				foreach (IAnalysisFixAction action in GetActions(doc, result))
				{
					if (flag)
					{
						infos.Add(new CommandInfo(result.Message.Replace("_", "__"), enabled: false, checkd: false)
						{
							Icon = GetIcon(result.Level)
						}, null);
						flag = false;
					}
					string text3 = action.Label.Replace("_", "__");
					string text4 = ((num <= 10) ? ("_" + num++ % 10 + " " + text3) : ("  " + text3));
					infos.Add(text4, action);
				}
				if (!result.HasOptionsDialog)
				{
					continue;
				}
				CommandInfoSet commandInfoSet = new CommandInfoSet();
				commandInfoSet.Text = GettextCatalog.GetString("_Options for \"{0}\"", result.OptionsTitle);
				bool flag2 = false;
				foreach (IAnalysisFixAction item2 in from f in result.Fixes.OfType<IAnalysisFixAction>()
					where f.SupportsBatchFix
					select f)
				{
					flag2 = true;
					string text5 = string.Format(GettextCatalog.GetString("Apply in file: {0}"), item2.Label);
					commandInfoSet.CommandInfos.Add(text5, new Action(item2.BatchFix));
				}
				if (flag2)
				{
					commandInfoSet.CommandInfos.AddSeparator();
				}
				InspectorResults ir = result as InspectorResults;
				if (ir != null)
				{
					BaseCodeIssueProvider inspector = ir.Inspector;
					if (inspector.CanSuppressWithAttribute)
					{
						commandInfoSet.CommandInfos.Add(GettextCatalog.GetString("_Suppress with attribute"), (Action)delegate
						{
							inspector.SuppressWithAttribute(doc, ir.Region);
						});
					}
					if (inspector.CanDisableWithPragma)
					{
						commandInfoSet.CommandInfos.Add(GettextCatalog.GetString("_Suppress with #pragma"), (Action)delegate
						{
							inspector.DisableWithPragma(doc, ir.Region);
						});
					}
					if (inspector.CanDisableOnce)
					{
						commandInfoSet.CommandInfos.Add(GettextCatalog.GetString("_Disable Once"), (Action)delegate
						{
							inspector.DisableOnce(doc, ir.Region);
						});
					}
					if (inspector.CanDisableAndRestore)
					{
						commandInfoSet.CommandInfos.Add(GettextCatalog.GetString("Disable _and Restore"), (Action)delegate
						{
							inspector.DisableAndRestore(doc, ir.Region);
						});
					}
				}
				commandInfoSet.CommandInfos.Add(GettextCatalog.GetString("_Configure Rule"), result);
				infos.Add(commandInfoSet);
			}
		}

		public static IEnumerable<IAnalysisFixAction> GetActions(Document doc, FixableResult result)
		{
			try
			{
				IAnalysisFix[] fixes = result.Fixes;
				foreach (IAnalysisFix fix in fixes)
				{
					foreach (IFixHandler handler in AnalysisExtensions.GetFixHandlers(fix.FixType))
					{
						foreach (IAnalysisFixAction fix2 in handler.GetFixes(doc, fix))
						{
							yield return fix2;
						}
					}
				}
			}
			finally
			{
			}
		}

		private static string GetIcon(Severity severity)
		{
			switch (severity)
			{
			case Severity.Error:
				return MonoDevelop.Ide.Gui.Stock.Error;
			case Severity.Warning:
				return MonoDevelop.Ide.Gui.Stock.Warning;
			case Severity.Hint:
				return MonoDevelop.Ide.Gui.Stock.Information;
			default:
				return null;
			}
		}
	}
}
