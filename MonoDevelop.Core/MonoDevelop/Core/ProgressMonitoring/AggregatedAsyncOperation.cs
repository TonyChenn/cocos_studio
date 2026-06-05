using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x020000E4 RID: 228
	public class AggregatedAsyncOperation : IAsyncOperation
	{
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060007F8 RID: 2040 RVA: 0x00020964 File Offset: 0x0001EB64
		// (remove) Token: 0x060007F9 RID: 2041 RVA: 0x0002099C File Offset: 0x0001EB9C
		public event OperationHandler Completed;

		// Token: 0x060007FA RID: 2042 RVA: 0x000209D1 File Offset: 0x0001EBD1
		public void Add(IAsyncOperation oper)
		{
			if (this.started)
			{
				throw new InvalidOperationException("Can't add more operations after calling StartMonitoring");
			}
			this.operations.Add(oper);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x000209F4 File Offset: 0x0001EBF4
		public void StartMonitoring()
		{
			this.started = true;
			foreach (IAsyncOperation asyncOperation in this.operations)
			{
				asyncOperation.Completed += this.OperCompleted;
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00020A5C File Offset: 0x0001EC5C
		private void OperCompleted(IAsyncOperation op)
		{
			bool flag2;
			lock (this.operations)
			{
				this.completed++;
				this.success = (this.success && op.Success);
				this.successWithWarnings = (this.success && op.SuccessWithWarnings);
				flag2 = (this.completed == this.operations.Count);
			}
			if (flag2 && this.Completed != null)
			{
				this.Completed(this);
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00020B00 File Offset: 0x0001ED00
		public void Cancel()
		{
			this.CheckStarted();
			lock (this.operations)
			{
				foreach (IAsyncOperation asyncOperation in this.operations)
				{
					asyncOperation.Cancel();
				}
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00020B84 File Offset: 0x0001ED84
		public void WaitForCompleted()
		{
			this.CheckStarted();
			foreach (IAsyncOperation asyncOperation in this.operations)
			{
				asyncOperation.WaitForCompleted();
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00020BDC File Offset: 0x0001EDDC
		public bool IsCompleted
		{
			get
			{
				this.CheckStarted();
				return this.completed == this.operations.Count;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00020BF7 File Offset: 0x0001EDF7
		public bool Success
		{
			get
			{
				this.CheckStarted();
				return this.success;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00020C05 File Offset: 0x0001EE05
		public bool SuccessWithWarnings
		{
			get
			{
				this.CheckStarted();
				return this.successWithWarnings;
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00020C13 File Offset: 0x0001EE13
		private void CheckStarted()
		{
			if (!this.started)
			{
				throw new InvalidOperationException("Operation not started");
			}
		}

		// Token: 0x04000291 RID: 657
		private List<IAsyncOperation> operations = new List<IAsyncOperation>();

		// Token: 0x04000292 RID: 658
		private bool started;

		// Token: 0x04000293 RID: 659
		private bool success = true;

		// Token: 0x04000294 RID: 660
		private bool successWithWarnings = true;

		// Token: 0x04000295 RID: 661
		private int completed;
	}
}
