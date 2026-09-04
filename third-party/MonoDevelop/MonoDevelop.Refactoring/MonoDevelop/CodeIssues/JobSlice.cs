using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MonoDevelop.Projects;

namespace MonoDevelop.CodeIssues
{
	public class JobSlice : IComparable<JobSlice>, IDisposable
	{
		private bool disposed;

		private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

		private readonly IList<IAnalysisJob> jobs = new List<IAnalysisJob>();

		private readonly IList<JobStatus> statuses = new List<JobStatus>();

		public ProjectFile File { get; private set; }

		public CancellationToken CancellationToken => tokenSource.Token;

		public JobSlice(ProjectFile file)
		{
			File = file;
		}

		~JobSlice()
		{
			Dispose(disposing: false);
		}

		public void AddJob(IAnalysisJob job, JobStatus status)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			statuses.Add(status);
			jobs.Add(job);
		}

		public IEnumerable<IAnalysisJob> GetJobs()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return new List<IAnalysisJob>(jobs);
		}

		public void RemoveJob(IAnalysisJob job)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			jobs.Remove(job);
			if (!jobs.Any())
			{
				tokenSource.Cancel();
			}
		}

		private void MarkAsComplete()
		{
			foreach (JobStatus status in statuses)
			{
				status.MarkAsComplete(this);
			}
		}

		public int CompareTo(JobSlice other)
		{
			return jobs.Count.CompareTo(other.jobs.Count);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				MarkAsComplete();
				disposed = true;
			}
		}
	}
}
