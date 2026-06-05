using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000019 RID: 25
	[DesignerCategory("Code")]
	public class ProcessWrapper : Process, IProcessAsyncOperation, IAsyncOperation, IDisposable
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004DC0 File Offset: 0x00002FC0
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00004DC8 File Offset: 0x00002FC8
		public bool CancelRequested { get; private set; }

		// Token: 0x060000B8 RID: 184 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public new void Start()
		{
			this.CheckDisposed();
			base.Start();
			this.captureOutputThread = new Thread(new ThreadStart(this.CaptureOutput));
			this.captureOutputThread.Name = "Process output reader";
			this.captureOutputThread.IsBackground = true;
			this.captureOutputThread.Start();
			if (this.ErrorStreamChanged != null)
			{
				this.captureErrorThread = new Thread(new ThreadStart(this.CaptureError));
				this.captureErrorThread.Name = "Process error reader";
				this.captureErrorThread.IsBackground = true;
				this.captureErrorThread.Start();
				return;
			}
			this.endEventErr.Set();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004E7F File Offset: 0x0000307F
		public void WaitForOutput(int milliseconds)
		{
			this.CheckDisposed();
			base.WaitForExit(milliseconds);
			this.endEventOut.WaitOne();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004E9B File Offset: 0x0000309B
		public void WaitForOutput()
		{
			this.WaitForOutput(-1);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004EA4 File Offset: 0x000030A4
		private void CaptureOutput()
		{
			try
			{
				if (this.OutputStreamChanged != null)
				{
					char[] array = new char[1024];
					int length;
					while ((length = base.StandardOutput.Read(array, 0, array.Length)) > 0)
					{
						if (this.OutputStreamChanged != null)
						{
							this.OutputStreamChanged(this, new string(array, 0, length));
						}
					}
				}
			}
			catch (ThreadAbortException)
			{
				Thread.ResetAbort();
			}
			finally
			{
				if (this.endEventErr != null)
				{
					this.endEventErr.WaitOne();
				}
				this.OnExited(this, EventArgs.Empty);
				lock (this.lockObj)
				{
					if (this.endEventOut != null)
					{
						this.endEventOut.Set();
					}
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004F80 File Offset: 0x00003180
		private void CaptureError()
		{
			try
			{
				char[] array = new char[1024];
				int length;
				while ((length = base.StandardError.Read(array, 0, array.Length)) > 0)
				{
					if (this.ErrorStreamChanged != null)
					{
						this.ErrorStreamChanged(this, new string(array, 0, length));
					}
				}
			}
			finally
			{
				lock (this.lockObj)
				{
					if (this.endEventErr != null)
					{
						this.endEventErr.Set();
					}
				}
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000501C File Offset: 0x0000321C
		protected override void Dispose(bool disposing)
		{
			lock (this.lockObj)
			{
				if (this.endEventOut == null)
				{
					return;
				}
				if (!this.done)
				{
					((IAsyncOperation)this).Cancel();
				}
				this.captureOutputThread = (this.captureErrorThread = null);
				this.endEventOut.Close();
				this.endEventErr.Close();
				this.endEventOut = (this.endEventErr = null);
			}
			try
			{
				base.Dispose(disposing);
			}
			catch
			{
				if (disposing)
				{
					throw;
				}
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000050C4 File Offset: 0x000032C4
		private void CheckDisposed()
		{
			if (this.endEventOut == null)
			{
				throw new ObjectDisposedException("ProcessWrapper");
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000050D9 File Offset: 0x000032D9
		int IProcessAsyncOperation.ExitCode
		{
			get
			{
				return base.ExitCode;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000050E1 File Offset: 0x000032E1
		int IProcessAsyncOperation.ProcessId
		{
			get
			{
				return base.Id;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000050EC File Offset: 0x000032EC
		void IAsyncOperation.Cancel()
		{
			try
			{
				if (!this.done)
				{
					try
					{
						this.CancelRequested = true;
						this.KillProcessTree();
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError(ex.ToString());
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005140 File Offset: 0x00003340
		void IAsyncOperation.WaitForCompleted()
		{
			this.WaitForOutput();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005148 File Offset: 0x00003348
		private void OnExited(object sender, EventArgs args)
		{
			try
			{
				if (!base.HasExited)
				{
					base.WaitForExit();
				}
			}
			catch
			{
			}
			finally
			{
				lock (this.lockObj)
				{
					this.done = true;
					try
					{
						if (this.completedEvent != null)
						{
							this.completedEvent(this);
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000C4 RID: 196 RVA: 0x000051DC File Offset: 0x000033DC
		// (remove) Token: 0x060000C5 RID: 197 RVA: 0x00005238 File Offset: 0x00003438
		event OperationHandler IAsyncOperation.Completed
		{
			add
			{
				bool flag = false;
				lock (this.lockObj)
				{
					if (this.done)
					{
						flag = true;
					}
					else
					{
						this.completedEvent += value;
					}
				}
				if (flag)
				{
					value(this);
				}
			}
			remove
			{
				lock (this.lockObj)
				{
					this.completedEvent -= value;
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000527C File Offset: 0x0000347C
		bool IAsyncOperation.Success
		{
			get
			{
				return this.done && base.ExitCode == 0;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005291 File Offset: 0x00003491
		bool IAsyncOperation.SuccessWithWarnings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00005294 File Offset: 0x00003494
		bool IAsyncOperation.IsCompleted
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000C9 RID: 201 RVA: 0x0000529C File Offset: 0x0000349C
		// (remove) Token: 0x060000CA RID: 202 RVA: 0x000052D4 File Offset: 0x000034D4
		private event OperationHandler completedEvent;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000CB RID: 203 RVA: 0x0000530C File Offset: 0x0000350C
		// (remove) Token: 0x060000CC RID: 204 RVA: 0x00005344 File Offset: 0x00003544
		public event ProcessEventHandler OutputStreamChanged;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060000CD RID: 205 RVA: 0x0000537C File Offset: 0x0000357C
		// (remove) Token: 0x060000CE RID: 206 RVA: 0x000053B4 File Offset: 0x000035B4
		public event ProcessEventHandler ErrorStreamChanged;

		// Token: 0x04000053 RID: 83
		private Thread captureOutputThread;

		// Token: 0x04000054 RID: 84
		private Thread captureErrorThread;

		// Token: 0x04000055 RID: 85
		private ManualResetEvent endEventOut = new ManualResetEvent(false);

		// Token: 0x04000056 RID: 86
		private ManualResetEvent endEventErr = new ManualResetEvent(false);

		// Token: 0x04000057 RID: 87
		private bool done;

		// Token: 0x04000058 RID: 88
		private object lockObj = new object();
	}
}
