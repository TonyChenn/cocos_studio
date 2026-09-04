using System;
using System.Collections.Generic;

namespace MonoDevelop.CodeIssues
{
	public class JobStatus
	{
		private readonly object _lock = new object();

		private readonly IAnalysisJob job;

		private readonly ISet<JobSlice> slices = new HashSet<JobSlice>();

		public JobStatus(IAnalysisJob job)
		{
			if (job == null)
			{
				throw new ArgumentNullException("job");
			}
			this.job = job;
		}

		public void AddSlice(JobSlice slice)
		{
			lock (_lock)
			{
				slices.Add(slice);
			}
		}

		public void MarkAsComplete(JobSlice slice)
		{
			lock (_lock)
			{
				slices.Remove(slice);
				if (slices.Count == 0)
				{
					job.SetCompleted();
				}
			}
		}
	}
}
