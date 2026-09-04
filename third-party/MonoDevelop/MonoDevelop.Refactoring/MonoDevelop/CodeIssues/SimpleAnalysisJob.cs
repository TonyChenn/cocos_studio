using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Refactoring;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MonoDevelop.Refactoring;

namespace MonoDevelop.CodeIssues
{
	public class SimpleAnalysisJob : IAnalysisJob
	{
		private object _lock = new object();

		private readonly IList<ProjectFile> files;

		public bool IsCompleted { get; private set; }

		private bool IsCancelled { get; set; }

		public event EventHandler<CodeIssueEventArgs> CodeIssueAdded;

		private event EventHandler<EventArgs> completed;

		public event EventHandler<EventArgs> Completed
		{
			add
			{
				completed += value;
				if (IsCompleted && !IsCancelled)
				{
					OnCompleted(new EventArgs());
				}
			}
			remove
			{
				completed += value;
			}
		}

		public SimpleAnalysisJob(IList<ProjectFile> files)
		{
			if (files == null)
			{
				throw new ArgumentNullException("files");
			}
			this.files = files;
		}

		protected virtual void OnCodeIssueAdded(CodeIssueEventArgs args)
		{
			CodeIssueAdded?.Invoke(this, args);
		}

		public IEnumerable<ProjectFile> GetFiles()
		{
			return files;
		}

		public IEnumerable<BaseCodeIssueProvider> GetIssueProviders(ProjectFile file)
		{
			return from provider in RefactoringService.GetInspectors(DesktopService.GetMimeTypeForUri(file.Name))
				where (provider.GetSeverity() != Severity.None && provider.GetIsEnabled()) ? true : false
				select provider;
		}

		public void AddResult(ProjectFile file, BaseCodeIssueProvider provider, IEnumerable<CodeIssue> issues)
		{
			OnCodeIssueAdded(new CodeIssueEventArgs(file, provider, issues));
		}

		public void AddError(ProjectFile file, BaseCodeIssueProvider provider)
		{
		}

		public void NotifyCancelled()
		{
			lock (_lock)
			{
				IsCancelled = true;
			}
		}

		protected virtual void OnCompleted(EventArgs e)
		{
			completed?.Invoke(this, e);
		}

		public void SetCompleted()
		{
			bool flag;
			lock (_lock)
			{
				IsCompleted = true;
				flag = !IsCancelled && !IsCompleted;
			}
			if (flag)
			{
				OnCompleted(new EventArgs());
			}
		}
	}
}
