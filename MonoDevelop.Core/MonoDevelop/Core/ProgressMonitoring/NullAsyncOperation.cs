using System;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x02000026 RID: 38
	public class NullAsyncOperation : IAsyncOperation
	{
		// Token: 0x06000150 RID: 336 RVA: 0x0000658D File Offset: 0x0000478D
		protected NullAsyncOperation(bool success, bool warnings)
		{
			this.success = success;
			this.warnings = warnings;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000065A3 File Offset: 0x000047A3
		public void Cancel()
		{
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000065A5 File Offset: 0x000047A5
		public void WaitForCompleted()
		{
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000065A7 File Offset: 0x000047A7
		public bool IsCompleted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000065AA File Offset: 0x000047AA
		bool IAsyncOperation.Success
		{
			get
			{
				return this.success;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000065B2 File Offset: 0x000047B2
		bool IAsyncOperation.SuccessWithWarnings
		{
			get
			{
				return this.success && this.warnings;
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000156 RID: 342 RVA: 0x000065C4 File Offset: 0x000047C4
		// (remove) Token: 0x06000157 RID: 343 RVA: 0x000065CD File Offset: 0x000047CD
		public event OperationHandler Completed
		{
			add
			{
				value(this);
			}
			remove
			{
			}
		}

		// Token: 0x04000084 RID: 132
		public static NullAsyncOperation Success = new NullAsyncOperation(true, false);

		// Token: 0x04000085 RID: 133
		public static NullAsyncOperation Failure = new NullAsyncOperation(false, false);

		// Token: 0x04000086 RID: 134
		private bool success;

		// Token: 0x04000087 RID: 135
		private bool warnings;
	}
}
