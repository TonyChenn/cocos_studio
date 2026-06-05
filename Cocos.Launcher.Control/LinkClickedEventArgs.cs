using System;

namespace Cocos.Launcher.Control
{
	// Token: 0x0200000F RID: 15
	public class LinkClickedEventArgs : EventArgs
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000348A File Offset: 0x0000168A
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003492 File Offset: 0x00001692
		public object Tag { get; private set; }

		// Token: 0x060000A3 RID: 163 RVA: 0x0000349B File Offset: 0x0000169B
		public LinkClickedEventArgs(object tag)
		{
			this.Tag = tag;
		}
	}
}
