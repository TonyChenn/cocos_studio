namespace MonoDevelop.CodeIssues
{
	public interface IIssueSummarySink
	{
		void AddIssue(IssueSummary issueSummary);
	}
}
