using System;

namespace Gtk
{
	// Token: 0x02000074 RID: 116
	public class WebNavigatingEventArgs : EventArgs
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000A9F0 File Offset: 0x00008BF0
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000AA07 File Offset: 0x00008C07
		public string Url { get; private set; }

		// Token: 0x060002AB RID: 683 RVA: 0x0000AA10 File Offset: 0x00008C10
		public WebNavigatingEventArgs(string url)
		{
			this.Url = url;
		}
	}
}
