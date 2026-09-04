using System;

namespace Gtk
{
	// Token: 0x02000092 RID: 146
	public class EndAnimationClickEventArgs : EventArgs
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000CD10 File Offset: 0x0000AF10
		// (set) Token: 0x0600031B RID: 795 RVA: 0x0000CD27 File Offset: 0x0000AF27
		public Window Window { get; private set; }

		// Token: 0x0600031C RID: 796 RVA: 0x0000CD30 File Offset: 0x0000AF30
		public EndAnimationClickEventArgs(Window window)
		{
			this.Window = window;
		}
	}
}
