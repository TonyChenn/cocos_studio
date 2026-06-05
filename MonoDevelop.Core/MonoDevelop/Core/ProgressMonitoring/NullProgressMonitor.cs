using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x0200001F RID: 31
	public class NullProgressMonitor : IProgressMonitor, IDisposable, IAsyncOperation
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00005A22 File Offset: 0x00003C22
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00005A25 File Offset: 0x00003C25
		public string[] Messages
		{
			get
			{
				if (this.messages != null)
				{
					return this.messages.ToArray();
				}
				return new string[0];
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00005A41 File Offset: 0x00003C41
		public string[] Warnings
		{
			get
			{
				if (this.warnings != null)
				{
					return this.warnings.ToArray();
				}
				return new string[0];
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005A5D File Offset: 0x00003C5D
		public ProgressError[] Errors
		{
			get
			{
				if (this.errors != null)
				{
					return this.errors.ToArray();
				}
				return new ProgressError[0];
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005A79 File Offset: 0x00003C79
		public virtual void BeginTask(string name, int totalWork)
		{
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005A7B File Offset: 0x00003C7B
		public virtual void EndTask()
		{
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005A7D File Offset: 0x00003C7D
		public virtual void BeginStepTask(string name, int totalWork, int stepSize)
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005A7F File Offset: 0x00003C7F
		public virtual void Step(int work)
		{
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00005A81 File Offset: 0x00003C81
		public virtual TextWriter Log
		{
			get
			{
				return TextWriter.Null;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005A88 File Offset: 0x00003C88
		public virtual void ReportSuccess(string message)
		{
			if (this.messages == null)
			{
				this.messages = new List<string>();
			}
			this.messages.Add(message);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005AA9 File Offset: 0x00003CA9
		public virtual void ReportWarning(string message)
		{
			if (this.warnings == null)
			{
				this.warnings = new List<string>();
			}
			this.warnings.Add(message);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005ACC File Offset: 0x00003CCC
		public virtual void ReportError(string message, Exception ex)
		{
			if (this.errors == null)
			{
				this.errors = new List<ProgressError>();
			}
			if (message == null && ex != null)
			{
				message = ex.Message;
			}
			else if (message != null && ex != null)
			{
				if (!message.EndsWith("."))
				{
					message += ".";
				}
				message = message + " " + ex.Message;
			}
			this.errors.Add(new ProgressError(message, ex));
			this.error = true;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005B48 File Offset: 0x00003D48
		public bool IsCancelRequested
		{
			get
			{
				return this.canceled;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005B50 File Offset: 0x00003D50
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.done)
				{
					return;
				}
				this.done = true;
				if (this.waitEvent != null)
				{
					this.waitEvent.Set();
				}
			}
			this.OnCompleted();
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public IAsyncOperation AsyncOperation
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005BB3 File Offset: 0x00003DB3
		void IAsyncOperation.Cancel()
		{
			this.OnCancelRequested();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005BBC File Offset: 0x00003DBC
		void IAsyncOperation.WaitForCompleted()
		{
			lock (this)
			{
				if (this.done)
				{
					return;
				}
				if (this.waitEvent == null)
				{
					this.waitEvent = new ManualResetEvent(false);
				}
			}
			this.waitEvent.WaitOne();
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005C1C File Offset: 0x00003E1C
		bool IAsyncOperation.IsCompleted
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00005C24 File Offset: 0x00003E24
		bool IAsyncOperation.Success
		{
			get
			{
				return !this.error && !this.canceled;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005C39 File Offset: 0x00003E39
		bool IAsyncOperation.SuccessWithWarnings
		{
			get
			{
				return !this.error && this.warnings != null;
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000111 RID: 273 RVA: 0x00005C54 File Offset: 0x00003E54
		// (remove) Token: 0x06000112 RID: 274 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public event OperationHandler Completed
		{
			add
			{
				bool flag = false;
				lock (this)
				{
					this.completedEvent += value;
					flag = this.done;
				}
				if (flag)
				{
					value(this);
				}
			}
			remove
			{
				lock (this)
				{
					this.completedEvent -= value;
				}
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000113 RID: 275 RVA: 0x00005CE0 File Offset: 0x00003EE0
		// (remove) Token: 0x06000114 RID: 276 RVA: 0x00005D30 File Offset: 0x00003F30
		public event MonitorHandler CancelRequested
		{
			add
			{
				bool flag = false;
				lock (this)
				{
					this.cancelRequestedEvent += value;
					flag = this.canceled;
				}
				if (flag)
				{
					value(this);
				}
			}
			remove
			{
				lock (this)
				{
					this.cancelRequestedEvent -= value;
				}
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005D6C File Offset: 0x00003F6C
		protected virtual void OnCancelRequested()
		{
			lock (this)
			{
				if (this.canceled)
				{
					return;
				}
				this.canceled = true;
			}
			if (this.cancelRequestedEvent != null)
			{
				this.cancelRequestedEvent(this);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005DC8 File Offset: 0x00003FC8
		protected virtual void OnCompleted()
		{
			if (this.completedEvent != null)
			{
				this.completedEvent(this.AsyncOperation);
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000117 RID: 279 RVA: 0x00005DE4 File Offset: 0x00003FE4
		// (remove) Token: 0x06000118 RID: 280 RVA: 0x00005E1C File Offset: 0x0000401C
		private event MonitorHandler cancelRequestedEvent;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000119 RID: 281 RVA: 0x00005E54 File Offset: 0x00004054
		// (remove) Token: 0x0600011A RID: 282 RVA: 0x00005E8C File Offset: 0x0000408C
		private event OperationHandler completedEvent;

		// Token: 0x0400006D RID: 109
		private bool done;

		// Token: 0x0400006E RID: 110
		private bool canceled;

		// Token: 0x0400006F RID: 111
		private bool error;

		// Token: 0x04000070 RID: 112
		private ManualResetEvent waitEvent;

		// Token: 0x04000071 RID: 113
		private List<ProgressError> errors;

		// Token: 0x04000072 RID: 114
		private List<string> warnings;

		// Token: 0x04000073 RID: 115
		private List<string> messages;
	}
}
