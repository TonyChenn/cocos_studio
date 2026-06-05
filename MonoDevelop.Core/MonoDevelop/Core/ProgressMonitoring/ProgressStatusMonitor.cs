using System;
using Mono.Addins;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x02000222 RID: 546
	public class ProgressStatusMonitor : MarshalByRefObject, IProgressStatus, IDisposable
	{
		// Token: 0x06001473 RID: 5235 RVA: 0x000546A9 File Offset: 0x000528A9
		public ProgressStatusMonitor(IProgressMonitor monitor) : this(monitor, 1)
		{
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x000546B3 File Offset: 0x000528B3
		public ProgressStatusMonitor(IProgressMonitor monitor, int logLevel)
		{
			this.logLevel = logLevel;
			this.monitor = monitor;
			monitor.BeginTask("", 100);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x000546D6 File Offset: 0x000528D6
		public void SetMessage(string msg)
		{
			this.monitor.EndTask();
			this.monitor.BeginTask(msg, 100 - this.step);
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000546F8 File Offset: 0x000528F8
		public void SetProgress(double progress)
		{
			int num = (int)(progress * 100.0);
			this.monitor.Step(num - this.step);
			this.step = num;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0005472C File Offset: 0x0005292C
		public void Log(string msg)
		{
			this.monitor.Log.WriteLine(msg);
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0005473F File Offset: 0x0005293F
		public void ReportWarning(string message)
		{
			this.monitor.ReportWarning(message);
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0005474D File Offset: 0x0005294D
		public void ReportError(string message, Exception exception)
		{
			this.monitor.ReportError(message, exception);
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0005475C File Offset: 0x0005295C
		public bool IsCanceled
		{
			get
			{
				return this.monitor.IsCancelRequested;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x00054769 File Offset: 0x00052969
		// (set) Token: 0x0600147C RID: 5244 RVA: 0x00054771 File Offset: 0x00052971
		public int LogLevel
		{
			get
			{
				return this.logLevel;
			}
			set
			{
				this.logLevel = value;
			}
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0005477A File Offset: 0x0005297A
		public void Cancel()
		{
			this.monitor.AsyncOperation.Cancel();
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0005478C File Offset: 0x0005298C
		public void Dispose()
		{
			this.monitor.EndTask();
		}

		// Token: 0x04000621 RID: 1569
		private IProgressMonitor monitor;

		// Token: 0x04000622 RID: 1570
		private int step;

		// Token: 0x04000623 RID: 1571
		private int logLevel;
	}
}
