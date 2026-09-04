using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.Refactoring;

namespace MonoDevelop.CodeIssues
{
	public class CodeAnalysisBatchRunner
	{
		private readonly object _lock = new object();

		private int workerCount;

		private readonly AnalysisJobQueue jobQueue = new AnalysisJobQueue();

		public IJobContext QueueJob(IAnalysisJob job)
		{
			jobQueue.Add(job);
			EnsureRunning();
			return new JobContext(job, jobQueue, this);
		}

		private void EnsureRunning()
		{
			while (Interlocked.Add(ref workerCount, 1) < Environment.ProcessorCount)
			{
				new Thread((ThreadStart)delegate
				{
					try
					{
						ProcessQueue();
					}
					finally
					{
						Interlocked.Add(ref workerCount, -1);
					}
				}).Start();
			}
		}

		private void ProcessQueue()
		{
			while (true)
			{
				try
				{
					while (true)
					{
						JobSlice slice = GetSlice();
						try
						{
							if (slice == null)
							{
								break;
							}
							AnalyzeFile(slice, slice.GetJobs().SelectMany((IAnalysisJob job) => job.GetIssueProviders(slice.File)));
						}
						finally
						{
							if (slice != null)
							{
								((IDisposable)slice).Dispose();
							}
						}
					}
					break;
				}
				catch (Exception ex)
				{
					LoggingService.LogInternalError(ex);
				}
			}
		}

		private JobSlice GetSlice()
		{
			lock (_lock)
			{
				return jobQueue.Dequeue(1).FirstOrDefault();
			}
		}

		private void AnalyzeFile(JobSlice item, IEnumerable<BaseCodeIssueProvider> codeIssueProviders)
		{
			ProjectFile file = item.File;
			if (file.BuildAction != "Compile" || !(file.Project is DotNetProject))
			{
				return;
			}
			TextEditorData readOnlyTextEditorData;
			try
			{
				readOnlyTextEditorData = TextFileProvider.Instance.GetReadOnlyTextEditorData(file.FilePath);
			}
			catch (FileNotFoundException)
			{
				return;
			}
			ParsedDocument parsedDocument = TypeSystemService.ParseFile(file.Project, readOnlyTextEditorData);
			if (parsedDocument == null)
			{
				return;
			}
			IProjectContent projectContext = TypeSystemService.GetProjectContext(file.Project);
			ICompilation compilation = projectContext.AddOrUpdateFiles(parsedDocument.ParsedFile).CreateCompilation();
			CSharpAstResolver cSharpAstResolver;
			using (ExtensionMethods.ResolveCounter.BeginTiming())
			{
				cSharpAstResolver = new CSharpAstResolver(compilation, parsedDocument.GetAst<SyntaxTree>(), parsedDocument.ParsedFile as CSharpUnresolvedFile);
				try
				{
					cSharpAstResolver.ApplyNavigator(new ExtensionMethods.ConstantModeResolveVisitorNavigator(ResolveVisitorNavigationMode.Resolve, null));
				}
				catch (Exception ex2)
				{
					LoggingService.LogError("Error while applying navigator", ex2);
				}
			}
			IRefactoringContext refactoringContext = parsedDocument.CreateRefactoringContextWithEditor(readOnlyTextEditorData, cSharpAstResolver, CancellationToken.None);
			foreach (BaseCodeIssueProvider provider in codeIssueProviders)
			{
				if (item.CancellationToken.IsCancellationRequested)
				{
					break;
				}
				IList<IAnalysisJob> source;
				lock (_lock)
				{
					source = item.GetJobs().ToList();
				}
				List<IAnalysisJob> list = source.Where((IAnalysisJob j) => j.GetIssueProviders(file).Contains(provider)).ToList();
				try
				{
					List<CodeIssue> issues = provider.GetIssues(refactoringContext, CancellationToken.None).ToList();
					foreach (IAnalysisJob item2 in list)
					{
						item2.AddResult(file, provider, issues);
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception)
				{
					foreach (IAnalysisJob item3 in list)
					{
						item3.AddError(file, provider);
					}
				}
			}
		}
	}
}
