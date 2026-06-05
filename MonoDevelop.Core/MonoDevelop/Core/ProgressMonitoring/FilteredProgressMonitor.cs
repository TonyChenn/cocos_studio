using System;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x02000046 RID: 70
	public class FilteredProgressMonitor : AggregatedProgressMonitor
	{
		// Token: 0x0600023D RID: 573 RVA: 0x00008FB2 File Offset: 0x000071B2
		public FilteredProgressMonitor(IProgressMonitor targetMonitor) : this(targetMonitor, MonitorAction.WriteLog | MonitorAction.ReportError | MonitorAction.ReportWarning | MonitorAction.ReportSuccess | MonitorAction.Cancel | MonitorAction.SlaveCancel)
		{
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008FC0 File Offset: 0x000071C0
		public FilteredProgressMonitor(IProgressMonitor targetMonitor, MonitorAction actionMask)
		{
			base.AddSlaveMonitor(targetMonitor, actionMask);
		}
	}
}
