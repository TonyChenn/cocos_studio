using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	public class LocalLogger : Logger
	{
		private IEventSource eventSource;

		private readonly List<MSBuildTargetResult> results = new List<MSBuildTargetResult>();

		private readonly string projectFile;

		public List<MSBuildTargetResult> BuildResult => results;

		public LocalLogger(string projectFile)
		{
			this.projectFile = projectFile;
		}

		public override void Initialize(IEventSource eventSource)
		{
			this.eventSource = eventSource;
			eventSource.WarningRaised += EventSourceWarningRaised;
			eventSource.ErrorRaised += EventSourceErrorRaised;
		}

		public override void Shutdown()
		{
			eventSource.ErrorRaised -= EventSourceErrorRaised;
			eventSource.WarningRaised -= EventSourceWarningRaised;
		}

		private void EventSourceWarningRaised(object sender, BuildWarningEventArgs e)
		{
			results.Add(new MSBuildTargetResult(projectFile, isWarning: true, e.Subcategory, e.Code, e.File, e.LineNumber, e.ColumnNumber, e.ColumnNumber, e.EndLineNumber, e.Message, e.HelpKeyword));
		}

		private void EventSourceErrorRaised(object sender, BuildErrorEventArgs e)
		{
			results.Add(new MSBuildTargetResult(projectFile, isWarning: false, e.Subcategory, e.Code, e.File, e.LineNumber, e.ColumnNumber, e.ColumnNumber, e.EndLineNumber, e.Message, e.HelpKeyword));
		}
	}
}
