using System;
using MonoDevelop.Core.ProgressMonitoring;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200002F RID: 47
	public class NullProcessAsyncOperation : NullAsyncOperation, IProcessAsyncOperation, IAsyncOperation, IDisposable
	{
		// Token: 0x0600017B RID: 379 RVA: 0x0000692B File Offset: 0x00004B2B
		public NullProcessAsyncOperation(bool success) : base(success, false)
		{
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00006935 File Offset: 0x00004B35
		public int ExitCode
		{
			get
			{
				if (!((IAsyncOperation)this).Success)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006942 File Offset: 0x00004B42
		public int ProcessId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006945 File Offset: 0x00004B45
		void IDisposable.Dispose()
		{
		}

		// Token: 0x04000091 RID: 145
		public new static NullProcessAsyncOperation Success = new NullProcessAsyncOperation(true);

		// Token: 0x04000092 RID: 146
		public new static NullProcessAsyncOperation Failure = new NullProcessAsyncOperation(false);
	}
}
