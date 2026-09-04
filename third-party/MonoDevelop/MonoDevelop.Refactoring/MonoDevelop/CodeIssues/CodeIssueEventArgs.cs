using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Projects;

namespace MonoDevelop.CodeIssues
{
	public class CodeIssueEventArgs : EventArgs
	{
		public ProjectFile File { get; private set; }

		public BaseCodeIssueProvider Provider { get; private set; }

		public IList<CodeIssue> CodeIssues { get; private set; }

		public CodeIssueEventArgs(ProjectFile file, BaseCodeIssueProvider provider, IEnumerable<CodeIssue> codeIssues)
		{
			File = file;
			Provider = provider;
			CodeIssues = (codeIssues as IList<CodeIssue>) ?? codeIssues.ToList();
		}
	}
}
