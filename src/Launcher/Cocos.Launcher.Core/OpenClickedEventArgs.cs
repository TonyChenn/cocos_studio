using System;
using Cocos.Launcher.Control;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000062 RID: 98
	public class OpenClickedEventArgs : EventArgs
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000378 RID: 888 RVA: 0x000106A5 File Offset: 0x0000E8A5
		// (set) Token: 0x06000379 RID: 889 RVA: 0x000106AD File Offset: 0x0000E8AD
		public object Tag { get; private set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600037A RID: 890 RVA: 0x000106B6 File Offset: 0x0000E8B6
		// (set) Token: 0x0600037B RID: 891 RVA: 0x000106BE File Offset: 0x0000E8BE
		public StartEnum StartItem { get; private set; }

		// Token: 0x0600037C RID: 892 RVA: 0x000106C7 File Offset: 0x0000E8C7
		public OpenClickedEventArgs(object tag, StartEnum startItem)
		{
			this.Tag = tag;
			this.StartItem = startItem;
		}
	}
}
