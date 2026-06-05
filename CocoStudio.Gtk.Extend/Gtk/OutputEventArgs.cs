using System;

namespace Gtk
{
	// Token: 0x02000069 RID: 105
	public class OutputEventArgs : EventArgs
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000A1B0 File Offset: 0x000083B0
		// (set) Token: 0x06000251 RID: 593 RVA: 0x0000A1C7 File Offset: 0x000083C7
		public string OutputInfo { get; private set; }

		// Token: 0x06000252 RID: 594 RVA: 0x0000A1D0 File Offset: 0x000083D0
		public OutputEventArgs(string info)
		{
			this.OutputInfo = info;
		}
	}
}
