using System;
using System.Threading;

namespace MonoDevelop.Core
{
	// Token: 0x02000094 RID: 148
	public class AsyncOperation : IAsyncOperation
	{
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060004DC RID: 1244 RVA: 0x00010D7C File Offset: 0x0000EF7C
		// (remove) Token: 0x060004DD RID: 1245 RVA: 0x00010DE8 File Offset: 0x0000EFE8
		public event OperationHandler Completed
		{
			add
			{
				bool flag = false;
				lock (this.lck)
				{
					if (this.completed)
					{
						flag = true;
					}
					else
					{
						this.completedEvent = (OperationHandler)Delegate.Combine(this.completedEvent, value);
					}
				}
				if (flag)
				{
					value(this);
				}
			}
			remove
			{
				lock (this.lck)
				{
					this.completedEvent = (OperationHandler)Delegate.Remove(this.completedEvent, value);
				}
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060004DE RID: 1246 RVA: 0x00010E3C File Offset: 0x0000F03C
		// (remove) Token: 0x060004DF RID: 1247 RVA: 0x00010E74 File Offset: 0x0000F074
		public event OperationHandler CancelRequested;

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00010EA9 File Offset: 0x0000F0A9
		public bool Canceled
		{
			get
			{
				return this.canceled;
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00010EB1 File Offset: 0x0000F0B1
		public void Cancel()
		{
			this.canceled = true;
			if (this.trackedOperation != null)
			{
				this.trackedOperation.Cancel();
			}
			if (this.CancelRequested != null)
			{
				this.CancelRequested(this);
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00010EE4 File Offset: 0x0000F0E4
		void IAsyncOperation.WaitForCompleted()
		{
			lock (this.lck)
			{
				if (!this.completed)
				{
					Monitor.Wait(this.lck);
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00010F34 File Offset: 0x0000F134
		public bool IsCompleted
		{
			get
			{
				return this.completed;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00010F3C File Offset: 0x0000F13C
		public bool Success
		{
			get
			{
				return this.success;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00010F44 File Offset: 0x0000F144
		public bool SuccessWithWarnings
		{
			get
			{
				return this.successWithWarnings;
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00010FA4 File Offset: 0x0000F1A4
		public void TrackOperation(IAsyncOperation oper, bool isFinal)
		{
			if (this.trackedOperation != null)
			{
				throw new InvalidOperationException("An operation is already being tracked.");
			}
			this.trackedOperation = oper;
			oper.Completed += delegate(IAsyncOperation param0)
			{
				if (!oper.Success || isFinal)
				{
					this.SetCompleted(oper.Success, oper.SuccessWithWarnings);
				}
				this.trackedOperation = null;
			};
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00011002 File Offset: 0x0000F202
		public void SetCompleted(bool success)
		{
			this.SetCompleted(success, false);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001100C File Offset: 0x0000F20C
		public void SetCompleted(bool success, bool hasWarnings)
		{
			lock (this.lck)
			{
				this.completed = true;
				this.success = success;
				if (success && hasWarnings)
				{
					this.successWithWarnings = true;
				}
				Monitor.PulseAll(this.lck);
				if (this.completedEvent != null)
				{
					this.completedEvent(this);
				}
			}
		}

		// Token: 0x04000192 RID: 402
		private bool canceled;

		// Token: 0x04000193 RID: 403
		private bool completed;

		// Token: 0x04000194 RID: 404
		private bool success;

		// Token: 0x04000195 RID: 405
		private bool successWithWarnings;

		// Token: 0x04000196 RID: 406
		private IAsyncOperation trackedOperation;

		// Token: 0x04000197 RID: 407
		private object lck = new object();

		// Token: 0x04000198 RID: 408
		private OperationHandler completedEvent;
	}
}
