using System;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x020000ED RID: 237
	public class ProgressError
	{
		// Token: 0x0600084E RID: 2126 RVA: 0x000215E0 File Offset: 0x0001F7E0
		public ProgressError(string message, Exception ex)
		{
			this.ex = ex;
			this.message = message;
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x000215F6 File Offset: 0x0001F7F6
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x000215FE File Offset: 0x0001F7FE
		public Exception Exception
		{
			get
			{
				return this.ex;
			}
		}

		// Token: 0x040002A6 RID: 678
		private Exception ex;

		// Token: 0x040002A7 RID: 679
		private string message;
	}
}
