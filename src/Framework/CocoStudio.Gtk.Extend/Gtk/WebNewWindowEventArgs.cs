using System;

namespace Gtk
{
	// Token: 0x02000073 RID: 115
	public class WebNewWindowEventArgs : EventArgs
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000A96C File Offset: 0x00008B6C
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x0000A983 File Offset: 0x00008B83
		public string Url { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000A98C File Offset: 0x00008B8C
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x0000A9A3 File Offset: 0x00008BA3
		public int X { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000A9AC File Offset: 0x00008BAC
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x0000A9C3 File Offset: 0x00008BC3
		public int Y { get; private set; }

		// Token: 0x060002A8 RID: 680 RVA: 0x0000A9CC File Offset: 0x00008BCC
		public WebNewWindowEventArgs(string url, int x, int y)
		{
			this.Url = url;
			this.X = x;
			this.Y = y;
		}
	}
}
