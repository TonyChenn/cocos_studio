using System;
using System.Diagnostics;
using System.Threading;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000012 RID: 18
	internal class InernalProcessHost : Process, IProcessAsyncOperation, IAsyncOperation, IDisposable
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00003E58 File Offset: 0x00002058
		public InernalProcessHost()
		{
			base.Exited += delegate(object param0, EventArgs param1)
			{
				lock (this.doneLock)
				{
					this.finished = true;
					Monitor.PulseAll(this.doneLock);
					if (this.completed != null)
					{
						this.completed(this);
					}
				}
			};
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003E8F File Offset: 0x0000208F
		public int ProcessId
		{
			get
			{
				return base.Id;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000079 RID: 121 RVA: 0x00003E98 File Offset: 0x00002098
		// (remove) Token: 0x0600007A RID: 122 RVA: 0x00003EF8 File Offset: 0x000020F8
		public event OperationHandler Completed
		{
			add
			{
				lock (this.doneLock)
				{
					this.completed = (OperationHandler)Delegate.Combine(this.completed, value);
					if (this.finished)
					{
						value(this);
					}
				}
			}
			remove
			{
				lock (this.doneLock)
				{
					this.completed = (OperationHandler)Delegate.Remove(this.completed, value);
				}
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003F4C File Offset: 0x0000214C
		public void Cancel()
		{
			base.Kill();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003F54 File Offset: 0x00002154
		public void WaitForCompleted()
		{
			lock (this.doneLock)
			{
				while (!this.finished)
				{
					Monitor.Wait(this.doneLock);
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003FA4 File Offset: 0x000021A4
		public bool IsCompleted
		{
			get
			{
				bool result;
				lock (this.doneLock)
				{
					result = this.finished;
				}
				return result;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003FE8 File Offset: 0x000021E8
		public bool Success
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003FEB File Offset: 0x000021EB
		public bool SuccessWithWarnings
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003FEE File Offset: 0x000021EE
		int IProcessAsyncOperation.get_ExitCode()
		{
			return base.ExitCode;
		}

		// Token: 0x04000042 RID: 66
		private object doneLock = new object();

		// Token: 0x04000043 RID: 67
		private bool finished;

		// Token: 0x04000044 RID: 68
		private OperationHandler completed;
	}
}
