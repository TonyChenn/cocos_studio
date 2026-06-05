using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000009 RID: 9
	public class BreakpointEventArgs : EventArgs
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000033CF File Offset: 0x000015CF
		public BreakpointEventArgs(IBreakpoint breakpoint)
		{
			this.breakpoint = breakpoint;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000033DE File Offset: 0x000015DE
		public IBreakpoint Breakpoint
		{
			get
			{
				return this.breakpoint;
			}
		}

		// Token: 0x04000031 RID: 49
		private IBreakpoint breakpoint;
	}
}
