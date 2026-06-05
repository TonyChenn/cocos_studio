using System;
using CocoStudio.Core.ProgressMonitors;
using CocoStudio.Projects;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;

namespace CocoStudio.Core
{
	// Token: 0x02000045 RID: 69
	public class ProgressMonitorManager
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000A548 File Offset: 0x00008748
		public IProgressMonitor Default
		{
			get
			{
				return this.GetConsoleProgressMonitor(false, true);
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000A562 File Offset: 0x00008762
		internal void Initialize()
		{
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000A568 File Offset: 0x00008768
		public IProgressMonitor GetProgressMonitor()
		{
			return this.GetConsoleProgressMonitor(false, true);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000A584 File Offset: 0x00008784
		internal IProgressMonitor GetStatusProgressMonitor()
		{
			return this.GetConsoleProgressMonitor(false, true);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000A5A0 File Offset: 0x000087A0
		public IProgressMonitor GetSimpleProgressMonitor()
		{
			return new SimpleProgressMonitor();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000A5B8 File Offset: 0x000087B8
		public IProgressMonitor GetConsoleProgressMonitor(bool isWriteLog = false, bool isShowDetail = true)
		{
			return new ConsoleProgressFullExceptionMonitor(isWriteLog, isShowDetail);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000A5D4 File Offset: 0x000087D4
		public MessageDialogProgressMonitor GetMessageDialogProgreeMonitor()
		{
			return new MessageDialogProgressMonitor(true, true, true, false);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000A5F0 File Offset: 0x000087F0
		public IProgressMonitor GetOutputProgressMonitor()
		{
			return new OutputProgressMonitor();
		}
	}
}
