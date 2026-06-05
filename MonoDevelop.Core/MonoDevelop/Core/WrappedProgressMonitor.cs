using System;
using System.IO;

namespace MonoDevelop.Core
{
	// Token: 0x0200022B RID: 555
	public class WrappedProgressMonitor : IProgressMonitor, IDisposable
	{
		// Token: 0x1400006D RID: 109
		// (add) Token: 0x060014BE RID: 5310 RVA: 0x0005577B File Offset: 0x0005397B
		// (remove) Token: 0x060014BF RID: 5311 RVA: 0x00055789 File Offset: 0x00053989
		public event MonitorHandler CancelRequested
		{
			add
			{
				this.WrappedMonitor.CancelRequested += value;
			}
			remove
			{
				this.WrappedMonitor.CancelRequested -= value;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x00055797 File Offset: 0x00053997
		public IAsyncOperation AsyncOperation
		{
			get
			{
				return this.WrappedMonitor.AsyncOperation;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x000557A4 File Offset: 0x000539A4
		public bool IsCancelRequested
		{
			get
			{
				return this.WrappedMonitor.IsCancelRequested;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x000557B1 File Offset: 0x000539B1
		public TextWriter Log
		{
			get
			{
				return this.WrappedMonitor.Log;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x000557BE File Offset: 0x000539BE
		public object SyncRoot
		{
			get
			{
				return this.WrappedMonitor.SyncRoot;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x000557CB File Offset: 0x000539CB
		// (set) Token: 0x060014C5 RID: 5317 RVA: 0x000557D3 File Offset: 0x000539D3
		private IProgressMonitor WrappedMonitor { get; set; }

		// Token: 0x060014C6 RID: 5318 RVA: 0x000557DC File Offset: 0x000539DC
		public WrappedProgressMonitor(IProgressMonitor monitor)
		{
			this.WrappedMonitor = monitor;
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x000557EB File Offset: 0x000539EB
		public void BeginStepTask(string name, int totalWork, int stepSize)
		{
			this.WrappedMonitor.BeginStepTask(name, totalWork, stepSize);
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x000557FB File Offset: 0x000539FB
		public void BeginTask(string name, int totalWork)
		{
			this.WrappedMonitor.BeginTask(name, totalWork);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0005580A File Offset: 0x00053A0A
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			if (this.WrappedMonitor != null)
			{
				this.WrappedMonitor.Dispose();
				this.WrappedMonitor = null;
			}
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0005582A File Offset: 0x00053A2A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00055839 File Offset: 0x00053A39
		public void EndTask()
		{
			this.WrappedMonitor.EndTask();
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00055846 File Offset: 0x00053A46
		public void ReportError(string message, Exception exception)
		{
			this.WrappedMonitor.ReportError(message, exception);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00055855 File Offset: 0x00053A55
		public void ReportSuccess(string message)
		{
			this.WrappedMonitor.ReportSuccess(message);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00055863 File Offset: 0x00053A63
		public void ReportWarning(string message)
		{
			this.WrappedMonitor.ReportWarning(message);
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00055871 File Offset: 0x00053A71
		public void Step(int work)
		{
			this.WrappedMonitor.Step(work);
		}
	}
}
