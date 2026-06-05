using System;
using System.IO;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x02000033 RID: 51
	public sealed class SynchronizedProgressMonitor : IProgressMonitor, IDisposable
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00006C61 File Offset: 0x00004E61
		public SynchronizedProgressMonitor(IProgressMonitor monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006C70 File Offset: 0x00004E70
		public void BeginTask(string name, int totalWork)
		{
			lock (this.monitor.SyncRoot)
			{
				this.monitor.BeginTask(name, totalWork);
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006CBC File Offset: 0x00004EBC
		public void BeginStepTask(string name, int totalWork, int stepSize)
		{
			lock (this.monitor.SyncRoot)
			{
				this.monitor.BeginStepTask(name, totalWork, stepSize);
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006D0C File Offset: 0x00004F0C
		public void EndTask()
		{
			lock (this.monitor.SyncRoot)
			{
				this.monitor.EndTask();
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006D58 File Offset: 0x00004F58
		public void Step(int work)
		{
			lock (this.monitor.SyncRoot)
			{
				this.monitor.Step(work);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00006DA4 File Offset: 0x00004FA4
		public TextWriter Log
		{
			get
			{
				return this.monitor.Log;
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006DB4 File Offset: 0x00004FB4
		public void ReportSuccess(string message)
		{
			lock (this.monitor.SyncRoot)
			{
				this.monitor.ReportSuccess(message);
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006E00 File Offset: 0x00005000
		public void ReportWarning(string message)
		{
			lock (this.monitor.SyncRoot)
			{
				this.monitor.ReportWarning(message);
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006E4C File Offset: 0x0000504C
		public void ReportError(string message, Exception ex)
		{
			lock (this.monitor.SyncRoot)
			{
				this.monitor.ReportError(message, ex);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00006E98 File Offset: 0x00005098
		public bool IsCancelRequested
		{
			get
			{
				bool isCancelRequested;
				lock (this.monitor.SyncRoot)
				{
					isCancelRequested = this.monitor.IsCancelRequested;
				}
				return isCancelRequested;
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006EE4 File Offset: 0x000050E4
		public void Dispose()
		{
			lock (this.monitor.SyncRoot)
			{
				this.monitor.Dispose();
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00006F30 File Offset: 0x00005130
		public IAsyncOperation AsyncOperation
		{
			get
			{
				IAsyncOperation asyncOperation;
				lock (this.monitor.SyncRoot)
				{
					asyncOperation = this.monitor.AsyncOperation;
				}
				return asyncOperation;
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000198 RID: 408 RVA: 0x00006F7C File Offset: 0x0000517C
		// (remove) Token: 0x06000199 RID: 409 RVA: 0x00006FC8 File Offset: 0x000051C8
		public event MonitorHandler CancelRequested
		{
			add
			{
				lock (this.monitor.SyncRoot)
				{
					this.monitor.CancelRequested += value;
				}
			}
			remove
			{
				lock (this.monitor.SyncRoot)
				{
					this.monitor.CancelRequested -= value;
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00007014 File Offset: 0x00005214
		public object SyncRoot
		{
			get
			{
				return this.monitor.SyncRoot;
			}
		}

		// Token: 0x04000097 RID: 151
		private IProgressMonitor monitor;
	}
}
