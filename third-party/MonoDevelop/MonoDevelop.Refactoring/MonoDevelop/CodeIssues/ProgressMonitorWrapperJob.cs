using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.CodeIssues
{
	public class ProgressMonitorWrapperJob : IAnalysisJob
	{
		private readonly IAnalysisJob wrappedJob;

		private IProgressMonitor monitor;

		private int reportingThinningFactor = 100;

		private int completedWork;

		public event EventHandler<CodeIssueEventArgs> CodeIssueAdded
		{
			add
			{
				wrappedJob.CodeIssueAdded += value;
			}
			remove
			{
				wrappedJob.CodeIssueAdded -= value;
			}
		}

		public event EventHandler<EventArgs> Completed
		{
			add
			{
				wrappedJob.Completed += value;
			}
			remove
			{
				wrappedJob.Completed -= value;
			}
		}

		public ProgressMonitorWrapperJob(IAnalysisJob wrappedJob, string message)
		{
			this.wrappedJob = wrappedJob;
			monitor = IdeApp.Workbench.ProgressMonitors.GetStatusProgressMonitor(message, null, showErrorDialogs: false);
			IEnumerable<ProjectFile> files = wrappedJob.GetFiles();
			Func<ProjectFile, int> selector = (ProjectFile f) => wrappedJob.GetIssueProviders(f).Count();
			int totalWork = files.Sum(selector);
			monitor.BeginTask(message, totalWork);
		}

		public IEnumerable<ProjectFile> GetFiles()
		{
			return wrappedJob.GetFiles();
		}

		public IEnumerable<BaseCodeIssueProvider> GetIssueProviders(ProjectFile file)
		{
			return wrappedJob.GetIssueProviders(file);
		}

		public void AddResult(ProjectFile file, BaseCodeIssueProvider provider, IEnumerable<CodeIssue> issues)
		{
			Step();
			wrappedJob.AddResult(file, provider, issues);
		}

		public void AddError(ProjectFile file, BaseCodeIssueProvider provider)
		{
			Step();
			wrappedJob.AddError(file, provider);
		}

		public void SetCompleted()
		{
			StopReporting();
			wrappedJob.SetCompleted();
		}

		private void Step()
		{
			completedWork++;
			if (monitor != null && completedWork % reportingThinningFactor == 0)
			{
				monitor.Step(reportingThinningFactor);
			}
		}

		private void StopReporting()
		{
			if (monitor != null)
			{
				monitor.Dispose();
				monitor = null;
			}
		}

		public void NotifyCancelled()
		{
			StopReporting();
		}
	}
}
