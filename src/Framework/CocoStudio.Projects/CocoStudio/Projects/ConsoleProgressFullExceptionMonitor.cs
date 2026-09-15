using System;
using CocoStudio.Basic;
using MonoDevelop.Core.ProgressMonitoring;

namespace CocoStudio.Projects
{
	public class ConsoleProgressFullExceptionMonitor : NullProgressMonitor
	{
		public ConsoleProgressFullExceptionMonitor(bool isWriteLogFile = false, bool isShowDetail = true)
		{
			this.isWriteLogFile = isWriteLogFile;
			this.isShowDetail = isShowDetail;
		}

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

		public override void ReportWarning(string message)
		{
			if (this.isWriteLogFile)
			{
				LogConfig.Logger.Info(message, true);
			}
			base.ReportWarning(message);
		}

		private bool isWriteLogFile;

		private bool isShowDetail = true;
	}
}
