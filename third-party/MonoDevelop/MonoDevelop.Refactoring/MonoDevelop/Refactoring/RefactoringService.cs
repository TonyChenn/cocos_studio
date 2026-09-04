using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ICSharpCode.NRefactory;
using Mono.Addins;
using Mono.TextEditor;
using MonoDevelop.AnalysisCore;
using MonoDevelop.AnalysisCore.Gui;
using MonoDevelop.CodeActions;
using MonoDevelop.CodeIssues;
using MonoDevelop.Core;
using MonoDevelop.Core.Instrumentation;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects.Text;

namespace MonoDevelop.Refactoring
{
	public static class RefactoringService
	{
		private class RenameHandler
		{
			private readonly IEnumerable<Change> changes;

			public RenameHandler(IEnumerable<Change> changes)
			{
				this.changes = changes;
			}

			public void FileRename(object sender, FileCopyEventArgs e)
			{
				foreach (FileCopyEventInfo item in e)
				{
					foreach (Change change in changes)
					{
						if (change is TextReplaceChange textReplaceChange && item.SourceFile == (FilePath)textReplaceChange.FileName)
						{
							textReplaceChange.FileName = item.TargetFile;
						}
					}
				}
			}
		}

		private static readonly List<RefactoringOperation> refactorings;

		private static readonly List<CodeActionProvider> contextActions;

		private static readonly List<CodeIssueProvider> inspectors;

		private static Stopwatch validActionsWatch;

		private static Stopwatch actionWatch;

		private static readonly CodeAnalysisBatchRunner runner;

		public static IEnumerable<CodeActionProvider> ContextAddinNodes => contextActions;

		public static List<CodeIssueProvider> Inspectors => inspectors;

		public static IEnumerable<RefactoringOperation> Refactorings => refactorings;

		public static void AddProvider(CodeActionProvider provider)
		{
			contextActions.Add(provider);
		}

		public static void AddProvider(CodeIssueProvider provider)
		{
			inspectors.Add(provider);
		}

		static RefactoringService()
		{
			refactorings = new List<RefactoringOperation>();
			contextActions = new List<CodeActionProvider>();
			inspectors = new List<CodeIssueProvider>();
			validActionsWatch = new Stopwatch();
			actionWatch = new Stopwatch();
			runner = new CodeAnalysisBatchRunner();
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/Refactoring/Refactorings", delegate(object sender, ExtensionNodeEventArgs args)
			{
				switch (args.Change)
				{
				case ExtensionChange.Add:
					refactorings.Add((RefactoringOperation)args.ExtensionObject);
					break;
				case ExtensionChange.Remove:
					refactorings.Remove((RefactoringOperation)args.ExtensionObject);
					break;
				}
			});
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/Refactoring/CodeActions", delegate(object sender, ExtensionNodeEventArgs args)
			{
				switch (args.Change)
				{
				case ExtensionChange.Add:
					contextActions.Add(((CodeActionAddinNode)args.ExtensionNode).Action);
					break;
				case ExtensionChange.Remove:
					contextActions.Remove(((CodeActionAddinNode)args.ExtensionNode).Action);
					break;
				}
			});
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/Refactoring/CodeActionSource", delegate(object sender, ExtensionNodeEventArgs args)
			{
				if (args.Change == ExtensionChange.Add)
				{
					contextActions.AddRange(((ICodeActionProviderSource)args.ExtensionObject).GetProviders());
				}
			});
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/Refactoring/CodeIssues", delegate(object sender, ExtensionNodeEventArgs args)
			{
				switch (args.Change)
				{
				case ExtensionChange.Add:
					inspectors.Add(((CodeIssueAddinNode)args.ExtensionNode).Inspector);
					break;
				case ExtensionChange.Remove:
					inspectors.Remove(((CodeIssueAddinNode)args.ExtensionNode).Inspector);
					break;
				}
			});
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/Refactoring/CodeIssueSource", delegate(object sender, ExtensionNodeEventArgs args)
			{
				if (args.Change == ExtensionChange.Add)
				{
					ICodeIssueProviderSource codeIssueProviderSource = (ICodeIssueProviderSource)args.ExtensionObject;
					IEnumerable<CodeIssueProvider> providers = codeIssueProviderSource.GetProviders();
					inspectors.AddRange(providers);
				}
			});
		}

		public static void AcceptChanges(IProgressMonitor monitor, List<Change> changes)
		{
			AcceptChanges(monitor, changes, TextFileProvider.Instance);
		}

		public static void AcceptChanges(IProgressMonitor monitor, List<Change> changes, ITextFileProvider fileProvider)
		{
			RefactoringOptions rctx = new RefactoringOptions(null);
			RenameHandler renameHandler = new RenameHandler(changes);
			FileService.FileRenamed += renameHandler.FileRename;
			HashSet<FilePath> hashSet = new HashSet<FilePath>();
			for (int i = 0; i < changes.Count; i++)
			{
				changes[i].PerformChange(monitor, rctx);
				if (!(changes[i] is TextReplaceChange textReplaceChange))
				{
					continue;
				}
				for (int j = i + 1; j < changes.Count; j++)
				{
					if (!(changes[j] is TextReplaceChange textReplaceChange2))
					{
						continue;
					}
					hashSet.Add(textReplaceChange2.FileName);
					if (textReplaceChange.Offset < 0 || textReplaceChange2.Offset < 0 || !(textReplaceChange.FileName == textReplaceChange2.FileName))
					{
						continue;
					}
					if (textReplaceChange.Offset < textReplaceChange2.Offset)
					{
						textReplaceChange2.Offset -= textReplaceChange.RemovedChars;
						if (!string.IsNullOrEmpty(textReplaceChange.InsertedText))
						{
							textReplaceChange2.Offset += textReplaceChange.InsertedText.Length;
						}
					}
					else if (textReplaceChange.Offset < textReplaceChange2.Offset + textReplaceChange2.RemovedChars)
					{
						textReplaceChange2.RemovedChars = Math.Max(0, textReplaceChange2.RemovedChars - textReplaceChange.RemovedChars);
						textReplaceChange2.Offset = textReplaceChange.Offset + ((!string.IsNullOrEmpty(textReplaceChange.InsertedText)) ? textReplaceChange.InsertedText.Length : 0);
					}
				}
			}
			FileService.NotifyFilesChanged(hashSet);
			FileService.FileRenamed -= renameHandler.FileRename;
			TextReplaceChange.FinishRefactoringOperation();
		}

		public static IEnumerable<CodeIssueProvider> GetInspectors(string mimeType)
		{
			return inspectors.Where((CodeIssueProvider i) => i.MimeType == mimeType);
		}

		public static IEnumerable<CodeAction> GetValidActions(Document doc, TextLocation loc, CancellationToken cancellationToken = default(CancellationToken))
		{
			TextEditorData editor = doc.Editor;
			string disabledNodes = ((editor != null) ? (PropertyService.Get("ContextActions." + editor.MimeType, "") ?? "") : "");
			List<CodeAction> list = new List<CodeAction>();
			TimerCounter timerCounter = InstrumentationService.CreateTimerCounter("Source analysis background task", "Source analysis");
			timerCounter.BeginTiming();
			validActionsWatch.Restart();
			Dictionary<CodeActionProvider, long> dictionary = new Dictionary<CodeActionProvider, long>();
			try
			{
				ParsedDocument parsedDocument = doc.ParsedDocument;
				if (editor != null && parsedDocument != null && parsedDocument.CreateRefactoringContext != null)
				{
					IRefactoringContext refactoringContext = parsedDocument.CreateRefactoringContext(doc, cancellationToken);
					if (refactoringContext != null)
					{
						foreach (CodeActionProvider item in contextActions.Where((CodeActionProvider fix) => fix.MimeType == editor.MimeType && disabledNodes.IndexOf(fix.IdString, StringComparison.Ordinal) < 0))
						{
							try
							{
								actionWatch.Restart();
								list.AddRange(item.GetActions(doc, refactoringContext, loc, cancellationToken));
								actionWatch.Stop();
								dictionary[item] = actionWatch.ElapsedMilliseconds;
							}
							catch (Exception ex)
							{
								LoggingService.LogError("Error in context action provider " + item.Title, ex);
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				LoggingService.LogError("Error in analysis service", ex2);
			}
			finally
			{
				timerCounter.EndTiming();
				validActionsWatch.Stop();
				if (validActionsWatch.ElapsedMilliseconds > 1000)
				{
					LoggingService.LogWarning("Warning slow edit action update.");
					foreach (KeyValuePair<CodeActionProvider, long> item2 in dictionary)
					{
						if (item2.Value > 50)
						{
							LoggingService.LogInfo("ACTION '" + item2.Key.Title + "' took " + item2.Value + "ms");
						}
					}
				}
			}
			return list;
		}

		public static void QueueQuickFixAnalysis(Document doc, TextLocation loc, CancellationToken token, Action<List<CodeAction>> callback)
		{
			ResultsEditorExtension content = doc.GetContent<ResultsEditorExtension>();
			List<Result> issues = ((content != null) ? (from r in content.GetResultsAtOffset(doc.Editor.LocationToOffset(loc), token)
				orderby r.Level
				select r).ToList() : new List<Result>());
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					List<CodeAction> list = new List<CodeAction>();
					foreach (Result item in issues)
					{
						if (token.IsCancellationRequested)
						{
							return;
						}
						if (item is FixableResult result)
						{
							foreach (IAnalysisFixAction action in FixOperationsHandler.GetActions(doc, result))
							{
								list.Add(new AnalysisContextActionProvider.AnalysisCodeAction(action, item)
								{
									DocumentRegion = action.DocumentRegion
								});
							}
						}
					}
					list.AddRange(GetValidActions(doc, loc));
					callback(list);
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Error in analysis service", ex);
				}
			});
		}

		public static IList<CodeAction> ApplyFixes(IEnumerable<CodeAction> fixes, IRefactoringContext refactoringContext)
		{
			if (fixes == null)
			{
				throw new ArgumentNullException("fixes");
			}
			if (refactoringContext == null)
			{
				throw new ArgumentNullException("refactoringContext");
			}
			IList<CodeAction> list = (fixes as IList<CodeAction>) ?? ((IList<CodeAction>)fixes.ToArray());
			if (list.Count == 0)
			{
				return new List<CodeAction>();
			}
			if (refactoringContext == null)
			{
				return RunAll(list, refactoringContext, null);
			}
			using (IDisposable script = refactoringContext.CreateScript())
			{
				return RunAll(list, refactoringContext, script);
			}
		}

		public static void ApplyFix(CodeAction action, IRefactoringContext context)
		{
			using (IDisposable script = context.CreateScript())
			{
				action.Run(context, script);
			}
		}

		private static List<CodeAction> RunAll(IEnumerable<CodeAction> allFixes, IRefactoringContext refactoringContext, object script)
		{
			List<CodeAction> list = new List<CodeAction>();
			foreach (CodeAction allFix in allFixes)
			{
				allFix.Run(refactoringContext, script);
				list.Add(allFix);
			}
			return list;
		}

		public static DocumentLocation GetCorrectResolveLocation(Document doc, DocumentLocation location)
		{
			if (doc == null)
			{
				return location;
			}
			TextEditorData editor = doc.Editor;
			if (editor == null || location.Column == 1)
			{
				return location;
			}
			if (editor.IsSomethingSelected)
			{
				return editor.MainSelection.Start;
			}
			DocumentLine line = editor.GetLine(location.Line);
			if (line == null || location.Column > line.LengthIncludingDelimiter)
			{
				return location;
			}
			int num = editor.LocationToOffset(location);
			if (num > 0 && !char.IsLetterOrDigit(doc.Editor.GetCharAt(num)) && char.IsLetterOrDigit(doc.Editor.GetCharAt(num - 1)))
			{
				return new DocumentLocation(location.Line, location.Column - 1);
			}
			return location;
		}

		public static IJobContext QueueCodeIssueAnalysis(IAnalysisJob job, string progressMessage = null)
		{
			if (progressMessage != null)
			{
				job = new ProgressMonitorWrapperJob(job, progressMessage);
			}
			return runner.QueueJob(job);
		}
	}
}
