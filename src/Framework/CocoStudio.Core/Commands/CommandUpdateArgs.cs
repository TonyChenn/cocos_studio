using System;
using CocoStudio.Core.ExtensionModel;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	// Token: 0x0200000E RID: 14
	public class CommandUpdateArgs : EventArgs
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000376C File Offset: 0x0000196C
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00003783 File Offset: 0x00001983
		public MenuInfo Info { get; private set; }

		// Token: 0x06000061 RID: 97 RVA: 0x0000378C File Offset: 0x0000198C
		internal CommandUpdateArgs(CommandInfo cmdInfo)
		{
			this.Info = new MenuInfo(cmdInfo);
		}
	}
}
