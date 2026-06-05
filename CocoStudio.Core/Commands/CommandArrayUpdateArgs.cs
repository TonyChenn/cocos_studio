using System;
using CocoStudio.Core.ExtensionModel;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	// Token: 0x0200000F RID: 15
	public class CommandArrayUpdateArgs : EventArgs
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000037A4 File Offset: 0x000019A4
		// (set) Token: 0x06000063 RID: 99 RVA: 0x000037BB File Offset: 0x000019BB
		public MenuArrayInfo Info { get; private set; }

		// Token: 0x06000064 RID: 100 RVA: 0x000037C4 File Offset: 0x000019C4
		internal CommandArrayUpdateArgs(CommandArrayInfo cmdArrayInfo)
		{
			this.Info = new MenuArrayInfo(cmdArrayInfo);
		}
	}
}
