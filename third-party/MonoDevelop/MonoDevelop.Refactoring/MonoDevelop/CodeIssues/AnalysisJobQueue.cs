using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Projects;

namespace MonoDevelop.CodeIssues
{
	public class AnalysisJobQueue
	{
		private readonly object _lock = new object();

		private readonly List<JobSlice> slices = new List<JobSlice>();

		private bool sorted;

		public void Add(IAnalysisJob job)
		{
			lock (_lock)
			{
				JobStatus jobStatus = new JobStatus(job);
				foreach (ProjectFile file in job.GetFiles())
				{
					List<JobSlice> source = slices;
					Func<JobSlice, bool> predicate = (JobSlice j) => j.File == file;
					JobSlice jobSlice = source.FirstOrDefault(predicate);
					if (jobSlice == null)
					{
						jobSlice = new JobSlice(file);
						slices.Add(jobSlice);
					}
					jobStatus.AddSlice(jobSlice);
					jobSlice.AddJob(job, jobStatus);
				}
				InvalidateSort();
			}
		}

		public void Remove(IAnalysisJob job)
		{
			lock (_lock)
			{
				foreach (ProjectFile file in job.GetFiles())
				{
					List<JobSlice> source = slices;
					Func<JobSlice, bool> predicate = (JobSlice j) => j.File == file;
					JobSlice jobSlice = source.FirstOrDefault(predicate);
					if (jobSlice != null)
					{
						jobSlice.RemoveJob(job);
						if (!jobSlice.GetJobs().Any())
						{
							slices.Remove(jobSlice);
						}
					}
				}
				InvalidateSort();
			}
		}

		public IEnumerable<JobSlice> Dequeue(int maxNumber)
		{
			lock (_lock)
			{
				EnsureSorted();
				List<JobSlice> list = slices.Take(maxNumber).ToList();
				foreach (JobSlice item in list)
				{
					slices.Remove(item);
				}
				return list;
			}
		}

		private void InvalidateSort()
		{
			sorted = false;
		}

		private void EnsureSorted()
		{
			if (!sorted)
			{
				slices.Sort();
				sorted = true;
			}
		}
	}
}
