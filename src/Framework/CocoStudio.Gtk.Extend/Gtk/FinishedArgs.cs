using System;

namespace Gtk
{
	// Token: 0x0200006A RID: 106
	public class FinishedArgs : EventArgs
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000A1E4 File Offset: 0x000083E4
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000A1FB File Offset: 0x000083FB
		public bool IsSuccess { get; private set; }

		// Token: 0x06000255 RID: 597 RVA: 0x0000A204 File Offset: 0x00008404
		public FinishedArgs(bool isSuccess)
		{
			this.IsSuccess = isSuccess;
		}
	}
}
