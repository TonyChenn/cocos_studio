using System.Collections.Generic;

namespace MonoDevelop.CodeIssues
{
	public interface ICodeIssueProviderSource
	{
		IEnumerable<CodeIssueProvider> GetProviders();
	}
}
