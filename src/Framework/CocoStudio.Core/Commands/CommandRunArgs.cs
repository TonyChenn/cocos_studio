using System;

namespace CocoStudio.Core.Commands
{
	// Token: 0x0200000D RID: 13
	public class CommandRunArgs : EventArgs
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003738 File Offset: 0x00001938
		// (set) Token: 0x0600005D RID: 93 RVA: 0x0000374F File Offset: 0x0000194F
		public object DataItem { get; private set; }

		// Token: 0x0600005E RID: 94 RVA: 0x00003758 File Offset: 0x00001958
		internal CommandRunArgs(object data)
		{
			this.DataItem = data;
		}
	}
}
