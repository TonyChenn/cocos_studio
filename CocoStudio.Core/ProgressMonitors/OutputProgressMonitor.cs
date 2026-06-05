using System;
using CocoStudio.Basic;
using MonoDevelop.Core.ProgressMonitoring;

namespace CocoStudio.Core.ProgressMonitors
{
	// Token: 0x02000028 RID: 40
	internal class OutputProgressMonitor : NullProgressMonitor
	{
		// Token: 0x06000171 RID: 369 RVA: 0x0000677F File Offset: 0x0000497F
		public override void ReportError(string message, Exception ex)
		{
			LogConfig.Output.Error(message, ex);
			base.ReportError(message, ex);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006798 File Offset: 0x00004998
		public override void ReportWarning(string message)
		{
			LogConfig.Output.Info(message, false);
			base.ReportWarning(message);
		}
	}
}
