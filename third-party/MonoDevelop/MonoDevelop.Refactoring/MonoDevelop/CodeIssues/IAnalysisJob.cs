using System;
using System.Collections.Generic;
using MonoDevelop.Projects;

namespace MonoDevelop.CodeIssues
{
	public interface IAnalysisJob
	{
		event EventHandler<CodeIssueEventArgs> CodeIssueAdded;

		event EventHandler<EventArgs> Completed;

		IEnumerable<ProjectFile> GetFiles();

		IEnumerable<BaseCodeIssueProvider> GetIssueProviders(ProjectFile file);

		void AddResult(ProjectFile file, BaseCodeIssueProvider provider, IEnumerable<CodeIssue> issues);

		void AddError(ProjectFile file, BaseCodeIssueProvider provider);

		void NotifyCancelled();

		void SetCompleted();
	}
}
