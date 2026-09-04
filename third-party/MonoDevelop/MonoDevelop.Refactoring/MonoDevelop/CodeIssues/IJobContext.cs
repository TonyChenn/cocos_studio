namespace MonoDevelop.CodeIssues
{
	public interface IJobContext
	{
		IAnalysisJob Job { get; }

		void CancelJob();
	}
}
