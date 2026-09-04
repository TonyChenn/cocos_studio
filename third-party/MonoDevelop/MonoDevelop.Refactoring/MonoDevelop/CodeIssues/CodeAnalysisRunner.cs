using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.NRefactory.Refactoring;
using Mono.TextEditor;
using MonoDevelop.AnalysisCore;
using MonoDevelop.AnalysisCore.Fixes;
using MonoDevelop.CodeActions;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Refactoring;
using MonoDevelop.SourceEditor.QuickTasks;

namespace MonoDevelop.CodeIssues
{
	public static class CodeAnalysisRunner
	{
		private static IEnumerable<BaseCodeIssueProvider> EnumerateProvider(CodeIssueProvider p)
		{
			if (p.HasSubIssues)
			{
				return p.SubIssues;
			}
			return new BaseCodeIssueProvider[1] { p };
		}

		public static IEnumerable<Result> Check(Document input, CancellationToken cancellationToken)
		{
			if (!QuickTaskStrip.EnableFancyFeatures || input.Project == null || !input.IsCompileableInProject)
			{
				return Enumerable.Empty<Result>();
			}
			TextEditorData editor = input.Editor;
			if (editor == null)
			{
				return Enumerable.Empty<Result>();
			}
			DocumentLocation loc = editor.Caret.Location;
			BlockingCollection<Result> result = new BlockingCollection<Result>();
			CodeIssueProvider[] source = RefactoringService.GetInspectors(editor.Document.MimeType).ToArray();
			IRefactoringContext context = ((input.ParsedDocument.CreateRefactoringContext != null) ? input.ParsedDocument.CreateRefactoringContext(input, cancellationToken) : null);
			Parallel.ForEach(source, delegate(CodeIssueProvider parentProvider)
			{
				try
				{
					foreach (BaseCodeIssueProvider item in EnumerateProvider(parentProvider))
					{
						Severity severity = item.GetSeverity();
						if (severity != Severity.None && item.GetIsEnabled())
						{
							foreach (CodeIssue r in item.GetIssues(context, cancellationToken))
							{
								List<GenericFix> list = ((r.Actions == null) ? new List<GenericFix>() : new List<GenericFix>(r.Actions.Where((CodeAction a) => a != null).Select(delegate(CodeAction a)
								{
									Action batchFix = null;
									if (a.SupportsBatchRunning)
									{
										batchFix = delegate
										{
											a.BatchRun(input, loc);
										};
									}
									return new GenericFix(a.Title, delegate
									{
										using (IDisposable script = context.CreateScript())
										{
											a.Run(context, script);
										}
									}, batchFix)
									{
										DocumentRegion = new DocumentRegion(r.Region.Begin, r.Region.End),
										IdString = a.IdString
									};
								})));
								result.Add(new InspectorResults(item, r.Region, r.Description, severity, r.IssueMarker, list.ToArray()));
							}
						}
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex2)
				{
					LoggingService.LogError(string.Concat("CodeAnalysis: Got exception in inspector '", parentProvider, "'"), ex2);
				}
			});
			return result;
		}
	}
}
