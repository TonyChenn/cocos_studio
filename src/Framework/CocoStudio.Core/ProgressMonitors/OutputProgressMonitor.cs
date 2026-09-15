using System;
using CocoStudio.Basic;
using MonoDevelop.Core.ProgressMonitoring;

namespace CocoStudio.Core.ProgressMonitors
{
	internal class OutputProgressMonitor : NullProgressMonitor
	{
		public override void ReportError(string message, Exception ex)
		{
			LogConfig.Output.Error(message, ex);
			base.ReportError(message, ex);
		}

		public override void ReportWarning(string message)
		{
			LogConfig.Output.Info(message, false);
			base.ReportWarning(message);
		}
	}
}
