using System;

namespace MonoDevelop.CodeIssues
{
	public class JobContext : IJobContext
	{
		private readonly IAnalysisJob job;

		private readonly AnalysisJobQueue queue;

		private readonly CodeAnalysisBatchRunner runner;

		public IAnalysisJob Job => job;

		public JobContext(IAnalysisJob job, AnalysisJobQueue queue, CodeAnalysisBatchRunner runner)
		{
			if (job == null)
			{
				throw new ArgumentNullException("job");
			}
			if (queue == null)
			{
				throw new ArgumentNullException("queue");
			}
			if (runner == null)
			{
				throw new ArgumentNullException("runner");
			}
			this.job = job;
			this.queue = queue;
			this.runner = runner;
		}

		public void CancelJob()
		{
			job.NotifyCancelled();
			queue.Remove(job);
		}
	}
}
