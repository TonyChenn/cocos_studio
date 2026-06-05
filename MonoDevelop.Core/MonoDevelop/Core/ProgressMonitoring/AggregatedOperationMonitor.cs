using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x02000032 RID: 50
	public class AggregatedOperationMonitor : IDisposable
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00006AE8 File Offset: 0x00004CE8
		public AggregatedOperationMonitor(IProgressMonitor monitor, params IAsyncOperation[] operations)
		{
			this.monitor = monitor;
			if (operations != null)
			{
				lock (this.list)
				{
					foreach (IAsyncOperation operation in operations)
					{
						this.AddOperation(operation);
					}
				}
			}
			monitor.CancelRequested += this.OnCancel;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006B70 File Offset: 0x00004D70
		public void AddOperation(IAsyncOperation operation)
		{
			lock (this.list)
			{
				if (this.monitor.IsCancelRequested)
				{
					operation.Cancel();
				}
				else
				{
					this.list.Add(operation);
				}
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006BCC File Offset: 0x00004DCC
		private void OnCancel(IProgressMonitor m)
		{
			lock (this.list)
			{
				foreach (IAsyncOperation asyncOperation in this.list)
				{
					asyncOperation.Cancel();
				}
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006C48 File Offset: 0x00004E48
		public void Dispose()
		{
			this.monitor.CancelRequested -= this.OnCancel;
		}

		// Token: 0x04000095 RID: 149
		private List<IAsyncOperation> list = new List<IAsyncOperation>();

		// Token: 0x04000096 RID: 150
		private IProgressMonitor monitor;
	}
}
