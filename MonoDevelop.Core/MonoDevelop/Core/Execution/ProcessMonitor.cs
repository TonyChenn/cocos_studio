using System;
using Mono.Unix.Native;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000015 RID: 21
	internal class ProcessMonitor
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00004C2C File Offset: 0x00002E2C
		public ProcessMonitor(IConsole console, IProcessAsyncOperation operation, EventHandler exited)
		{
			this.exited = exited;
			this.operation = operation;
			this.console = console;
			operation.Completed += this.OnOperationCompleted;
			console.CancelRequested += this.OnCancelRequest;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004C78 File Offset: 0x00002E78
		public void OnOperationCompleted(IAsyncOperation op)
		{
			try
			{
				if (this.exited != null)
				{
					this.exited(op, null);
				}
				if (!Platform.IsWindows && Syscall.WIFSIGNALED(this.operation.ExitCode))
				{
					this.console.Log.WriteLine(GettextCatalog.GetString("The application was terminated by a signal: {0}"), Syscall.WTERMSIG(this.operation.ExitCode));
				}
				else if (this.operation.ExitCode != 0)
				{
					this.console.Log.WriteLine(GettextCatalog.GetString("The application exited with code: {0}"), this.operation.ExitCode);
				}
			}
			finally
			{
				this.console.Dispose();
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004D3C File Offset: 0x00002F3C
		private void OnCancelRequest(object sender, EventArgs args)
		{
			this.operation.Cancel();
			this.console.CancelRequested -= this.OnCancelRequest;
		}

		// Token: 0x0400004F RID: 79
		public IConsole console;

		// Token: 0x04000050 RID: 80
		private EventHandler exited;

		// Token: 0x04000051 RID: 81
		private IProcessAsyncOperation operation;
	}
}
