using System;

namespace Gtk
{
	// Token: 0x02000072 RID: 114
	public class WebJavaScriptCallEventArgs : EventArgs
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000A8C0 File Offset: 0x00008AC0
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000A8D7 File Offset: 0x00008AD7
		public string Type { get; private set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000A8E0 File Offset: 0x00008AE0
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000A8F7 File Offset: 0x00008AF7
		public string Info { get; private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000A900 File Offset: 0x00008B00
		// (set) Token: 0x0600029E RID: 670 RVA: 0x0000A917 File Offset: 0x00008B17
		public int X { get; private set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000A920 File Offset: 0x00008B20
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0000A937 File Offset: 0x00008B37
		public int Y { get; private set; }

		// Token: 0x060002A1 RID: 673 RVA: 0x0000A940 File Offset: 0x00008B40
		public WebJavaScriptCallEventArgs(string type, string info, int x, int y)
		{
			this.Type = type;
			this.Info = info;
			this.X = x;
			this.Y = y;
		}
	}
}
