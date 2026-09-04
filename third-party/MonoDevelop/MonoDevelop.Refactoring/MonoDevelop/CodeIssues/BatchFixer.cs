using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Refactoring;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using Mono.TextEditor.Utils;
using MonoDevelop.CodeActions;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.Refactoring;

namespace MonoDevelop.CodeIssues
{
	public class BatchFixer
	{
		private readonly IActionMatcher matcher;

		private readonly IProgressMonitor monitor;

		public BatchFixer(IActionMatcher matcher, IProgressMonitor monitor)
		{
			this.matcher = matcher;
			this.monitor = monitor;
		}

		public IEnumerable<ActionSummary> TryFixIssues(IEnumerable<ActionSummary> actions)
		{
			if (actions == null)
			{
				throw new ArgumentNullException("actions");
			}
			IList<ActionSummary> actionSummaries = (actions as IList<ActionSummary>) ?? actions.ToList();
			List<IssueSummary> issueSummaries = actionSummaries.Select((ActionSummary action) => action.IssueSummary).ToList();
			List<ProjectFile> list = issueSummaries.Select((IssueSummary issue) => issue.File).Distinct().ToList();
			monitor.BeginTask("Applying fixes", list.Count);
			List<ActionSummary> appliedActions = new List<ActionSummary>(issueSummaries.Count);
			Parallel.ForEach(list, delegate(ProjectFile file)
			{
				monitor.Step(1);
				IEnumerable<IssueSummary> source = issueSummaries.Where((IssueSummary summary) => summary.File == file);
				HashSet<string> inspectorIds = new HashSet<string>(source.Select((IssueSummary summary) => summary.InspectorIdString));
				TextEditorData textEditorData = TextFileProvider.Instance.GetTextEditorData(file.FilePath, out var hadBom, out var encoding, out var isOpen);
				List<CodeAction> list2 = GetIssues(textEditorData, file, inspectorIds, out var refactoringContext).SelectMany((CodeIssue issue) => issue.Actions).ToList();
				if (list2.Count != 0 && refactoringContext != null)
				{
					List<ActionSummary> summaries = actionSummaries.Where((ActionSummary summary) => summary.IssueSummary.File == file).ToList();
					List<IssueMatch> source2 = matcher.Match(summaries, list2).ToList();
					IList<CodeAction> appliedFixes = RefactoringService.ApplyFixes(source2.Select((IssueMatch match) => match.Action), refactoringContext);
					appliedActions.AddRange(from match in source2
						where appliedFixes.Contains(match.Action)
						select match.Summary);
					if (!isOpen)
					{
						TextFileUtility.WriteText(file.Name, textEditorData.Text, encoding, hadBom);
					}
				}
			});
			return appliedActions;
		}

		private static IList<CodeIssue> GetIssues(TextEditorData data, ProjectFile file, ISet<string> inspectorIds, out IRefactoringContext refactoringContext)
		{
			List<CodeIssue> list = new List<CodeIssue>();
			ParsedDocument parsedDocument = TypeSystemService.ParseFile(file.Project, data);
			if (parsedDocument == null)
			{
				refactoringContext = null;
				return list;
			}
			IProjectContent projectContext = TypeSystemService.GetProjectContext(file.Project);
			ICompilation compilation = projectContext.AddOrUpdateFiles(parsedDocument.ParsedFile).CreateCompilation();
			CSharpAstResolver arg = new CSharpAstResolver(compilation, parsedDocument.GetAst<SyntaxTree>(), parsedDocument.ParsedFile as CSharpUnresolvedFile);
			refactoringContext = parsedDocument.CreateRefactoringContextWithEditor(data, arg, CancellationToken.None);
			IRefactoringContext refactoringContext2 = refactoringContext;
			foreach (CodeIssueProvider inspector in GetInspectors(data, inspectorIds))
			{
				if (inspector.GetSeverity() == Severity.None || !inspector.GetIsEnabled())
				{
					continue;
				}
				try
				{
					lock (list)
					{
						list.AddRange(inspector.GetIssues(refactoringContext2, CancellationToken.None));
					}
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Error while running code issue on: " + data.FileName, ex);
				}
			}
			return list;
		}

		private static IList<CodeIssueProvider> GetInspectors(TextEditorData editor, ICollection<string> inspectorIds)
		{
			List<CodeIssueProvider> source = RefactoringService.GetInspectors(editor.MimeType).ToList();
			return source.Where((CodeIssueProvider inspector) => inspectorIds.Contains(inspector.IdString)).ToList();
		}
	}
}
