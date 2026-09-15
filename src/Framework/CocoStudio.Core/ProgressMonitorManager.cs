using System;
using CocoStudio.Core.ProgressMonitors;
using CocoStudio.Projects;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;

namespace CocoStudio.Core
{
	public class ProgressMonitorManager
	{
		public IProgressMonitor Default
		{
			get
			{
				return this.GetConsoleProgressMonitor(false, true);
			}
		}

		internal void Initialize()
		{
		}

		public IProgressMonitor GetProgressMonitor()
		{
			return this.GetConsoleProgressMonitor(false, true);
		}

		internal IProgressMonitor GetStatusProgressMonitor()
		{
			return this.GetConsoleProgressMonitor(false, true);
		}

		public IProgressMonitor GetSimpleProgressMonitor()
		{
			return new SimpleProgressMonitor();
		}

		public IProgressMonitor GetConsoleProgressMonitor(bool isWriteLog = false, bool isShowDetail = true)
		{
			return new ConsoleProgressFullExceptionMonitor(isWriteLog, isShowDetail);
		}

		public MessageDialogProgressMonitor GetMessageDialogProgreeMonitor()
		{
			return new MessageDialogProgressMonitor(true, true, true, false);
		}

		public IProgressMonitor GetOutputProgressMonitor()
		{
			return new OutputProgressMonitor();
		}
	}
}
