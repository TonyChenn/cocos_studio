using System;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.ExtensionModel
{
	// Token: 0x02000008 RID: 8
	internal class CcsCmdHandler : CommandHandler
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000031C7 File Offset: 0x000013C7
		public CcsCmdHandler(MenuHandler handler)
		{
			this.Handler = handler;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000031D9 File Offset: 0x000013D9
		protected override void Run()
		{
			this.Handler.InternalRun();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000031E8 File Offset: 0x000013E8
		protected override void Run(object dataItem)
		{
			this.Handler.InternalRun(dataItem);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000031F8 File Offset: 0x000013F8
		protected override void Update(CommandInfo info)
		{
			MenuInfo info2 = new MenuInfo(info);
			this.Handler.InternalUpdate(info2);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000321C File Offset: 0x0000141C
		protected override void Update(CommandArrayInfo info)
		{
			MenuArrayInfo info2 = new MenuArrayInfo(info);
			this.Handler.InternalUpdate(info2);
		}

		// Token: 0x04000030 RID: 48
		private MenuHandler Handler;
	}
}
