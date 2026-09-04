using System;
using CocoStudio.Basic;
using MonoDevelop.Core.ProgressMonitoring;

namespace CocoStudio.Projects
{
	// Token: 0x02000003 RID: 3
	public class ConsoleProgressFullExceptionMonitor : NullProgressMonitor
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002091 File Offset: 0x00000291
		public ConsoleProgressFullExceptionMonitor(bool isWriteLogFile = false, bool isShowDetail = true)
		{
			this.isWriteLogFile = isWriteLogFile;
			this.isShowDetail = isShowDetail;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		public override void ReportError(string message, Exception ex)
		{
			string value = message;
			if (ex != null)
			{
				value = ex.ToString();
			}
			if (this.isShowDetail)
			{
				Console.WriteLine(value);
			}
			if (this.isWriteLogFile)
			{
				LogConfig.Logger.Error(message, ex);
			}
			base.ReportError(message, ex);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020F3 File Offset: 0x000002F3
		public override void ReportWarning(string message)
		{
			if (this.isWriteLogFile)
			{
				LogConfig.Logger.Info(message, true);
			}
			base.ReportWarning(message);
		}

		// Token: 0x04000002 RID: 2
		private bool isWriteLogFile;

		// Token: 0x04000003 RID: 3
		private bool isShowDetail = true;
	}
}
