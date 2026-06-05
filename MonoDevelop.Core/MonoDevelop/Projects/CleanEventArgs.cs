using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000229 RID: 553
	public class CleanEventArgs : EventArgs
	{
		// Token: 0x060014B9 RID: 5305 RVA: 0x00055764 File Offset: 0x00053964
		public CleanEventArgs(IProgressMonitor monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x00055773 File Offset: 0x00053973
		public IProgressMonitor Monitor
		{
			get
			{
				return this.monitor;
			}
		}

		// Token: 0x04000646 RID: 1606
		private IProgressMonitor monitor;
	}
}
