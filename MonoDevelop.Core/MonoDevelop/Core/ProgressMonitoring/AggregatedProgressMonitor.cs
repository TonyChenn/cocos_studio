using System;
using System.Collections.Generic;
using System.IO;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x0200001D RID: 29
	public class AggregatedProgressMonitor : IProgressMonitor, IDisposable, IAsyncOperation
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000053FF File Offset: 0x000035FF
		public IProgressMonitor MasterMonitor
		{
			get
			{
				return this.masterMonitor;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005407 File Offset: 0x00003607
		public AggregatedProgressMonitor() : this(new NullProgressMonitor(), new IProgressMonitor[0])
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000541C File Offset: 0x0000361C
		public AggregatedProgressMonitor(IProgressMonitor masterMonitor, params IProgressMonitor[] slaveMonitors)
		{
			this.masterMonitor = masterMonitor;
			this.AddSlaveMonitor(masterMonitor, MonitorAction.All);
			this.logger = new LogTextWriter();
			this.logger.TextWritten += this.OnWriteLog;
			foreach (IProgressMonitor slaveMonitor in slaveMonitors)
			{
				this.AddSlaveMonitor(slaveMonitor);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000548A File Offset: 0x0000368A
		public void AddSlaveMonitor(IProgressMonitor slaveMonitor)
		{
			this.AddSlaveMonitor(slaveMonitor, MonitorAction.All);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005498 File Offset: 0x00003698
		public void AddSlaveMonitor(IProgressMonitor slaveMonitor, MonitorAction actionMask)
		{
			AggregatedProgressMonitor.MonitorInfo monitorInfo = new AggregatedProgressMonitor.MonitorInfo();
			monitorInfo.ActionMask = actionMask;
			monitorInfo.Monitor = slaveMonitor;
			this.monitors.Add(monitorInfo);
			if ((actionMask & MonitorAction.SlaveCancel) != MonitorAction.None)
			{
				slaveMonitor.CancelRequested += this.OnSlaveCancelRequested;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000054E0 File Offset: 0x000036E0
		public void BeginTask(string name, int totalWork)
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.Tasks) != MonitorAction.None)
				{
					monitorInfo.Monitor.BeginTask(name, totalWork);
				}
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005544 File Offset: 0x00003744
		public void BeginStepTask(string name, int totalWork, int stepSize)
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.Tasks) != MonitorAction.None)
				{
					monitorInfo.Monitor.BeginStepTask(name, totalWork, stepSize);
				}
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000055AC File Offset: 0x000037AC
		public void EndTask()
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.Tasks) != MonitorAction.None)
				{
					monitorInfo.Monitor.EndTask();
				}
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005610 File Offset: 0x00003810
		public void Step(int work)
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.Tasks) != MonitorAction.None)
				{
					monitorInfo.Monitor.Step(work);
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005674 File Offset: 0x00003874
		public TextWriter Log
		{
			get
			{
				return this.logger;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000567C File Offset: 0x0000387C
		private void OnWriteLog(string text)
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.WriteLog) != MonitorAction.None)
				{
					monitorInfo.Monitor.Log.Write(text);
				}
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000056E4 File Offset: 0x000038E4
		public void ReportSuccess(string message)
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.ReportSuccess) != MonitorAction.None)
				{
					monitorInfo.Monitor.ReportSuccess(message);
				}
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005748 File Offset: 0x00003948
		public void ReportWarning(string message)
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.ReportWarning) != MonitorAction.None)
				{
					monitorInfo.Monitor.ReportWarning(message);
				}
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000057AC File Offset: 0x000039AC
		public void ReportError(string message, Exception ex)
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.ReportError) != MonitorAction.None)
				{
					monitorInfo.Monitor.ReportError(message, ex);
				}
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005810 File Offset: 0x00003A10
		public void Dispose()
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.Dispose) != MonitorAction.None)
				{
					monitorInfo.Monitor.Dispose();
				}
				if ((monitorInfo.ActionMask & MonitorAction.SlaveCancel) != MonitorAction.None)
				{
					monitorInfo.Monitor.CancelRequested -= this.OnSlaveCancelRequested;
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00005898 File Offset: 0x00003A98
		public bool IsCancelRequested
		{
			get
			{
				foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
				{
					if ((monitorInfo.ActionMask & MonitorAction.SlaveCancel) != MonitorAction.None && monitorInfo.Monitor.IsCancelRequested)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005908 File Offset: 0x00003B08
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000590B File Offset: 0x00003B0B
		private void OnSlaveCancelRequested(IProgressMonitor sender)
		{
			this.AsyncOperation.Cancel();
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00005918 File Offset: 0x00003B18
		public IAsyncOperation AsyncOperation
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000591C File Offset: 0x00003B1C
		void IAsyncOperation.Cancel()
		{
			foreach (AggregatedProgressMonitor.MonitorInfo monitorInfo in this.monitors)
			{
				if ((monitorInfo.ActionMask & MonitorAction.Cancel) != MonitorAction.None && !monitorInfo.Monitor.IsCancelRequested)
				{
					monitorInfo.Monitor.AsyncOperation.Cancel();
				}
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005990 File Offset: 0x00003B90
		void IAsyncOperation.WaitForCompleted()
		{
			this.masterMonitor.AsyncOperation.WaitForCompleted();
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x000059A2 File Offset: 0x00003BA2
		public bool IsCompleted
		{
			get
			{
				return this.masterMonitor.AsyncOperation.IsCompleted;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000059B4 File Offset: 0x00003BB4
		bool IAsyncOperation.Success
		{
			get
			{
				return this.masterMonitor.AsyncOperation.Success;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000059C6 File Offset: 0x00003BC6
		bool IAsyncOperation.SuccessWithWarnings
		{
			get
			{
				return this.masterMonitor.AsyncOperation.SuccessWithWarnings;
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060000F8 RID: 248 RVA: 0x000059D8 File Offset: 0x00003BD8
		// (remove) Token: 0x060000F9 RID: 249 RVA: 0x000059E6 File Offset: 0x00003BE6
		public event MonitorHandler CancelRequested
		{
			add
			{
				this.masterMonitor.CancelRequested += value;
			}
			remove
			{
				this.masterMonitor.CancelRequested -= value;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060000FA RID: 250 RVA: 0x000059F4 File Offset: 0x00003BF4
		// (remove) Token: 0x060000FB RID: 251 RVA: 0x00005A07 File Offset: 0x00003C07
		public event OperationHandler Completed
		{
			add
			{
				this.masterMonitor.AsyncOperation.Completed += value;
			}
			remove
			{
				this.masterMonitor.AsyncOperation.Completed -= value;
			}
		}

		// Token: 0x04000068 RID: 104
		private IProgressMonitor masterMonitor;

		// Token: 0x04000069 RID: 105
		private List<AggregatedProgressMonitor.MonitorInfo> monitors = new List<AggregatedProgressMonitor.MonitorInfo>();

		// Token: 0x0400006A RID: 106
		private LogTextWriter logger;

		// Token: 0x0200001E RID: 30
		private class MonitorInfo
		{
			// Token: 0x0400006B RID: 107
			public MonitorAction ActionMask;

			// Token: 0x0400006C RID: 108
			public IProgressMonitor Monitor;
		}
	}
}
