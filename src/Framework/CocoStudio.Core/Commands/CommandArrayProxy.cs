using System;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	// Token: 0x02000012 RID: 18
	public class CommandArrayProxy : ActionCommand
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000076 RID: 118 RVA: 0x00003AB8 File Offset: 0x00001CB8
		// (remove) Token: 0x06000077 RID: 119 RVA: 0x00003AF4 File Offset: 0x00001CF4
		public event EventHandler<CommandRunArgs> Execute;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000078 RID: 120 RVA: 0x00003B30 File Offset: 0x00001D30
		// (remove) Token: 0x06000079 RID: 121 RVA: 0x00003B6C File Offset: 0x00001D6C
		public event EventHandler<CommandArrayUpdateArgs> Update;

		// Token: 0x0600007A RID: 122 RVA: 0x00003BA8 File Offset: 0x00001DA8
		internal CommandArrayProxy()
		{
			base.DefaultHandler = new CommandArrayProxy.DefaultCommandHanle(this);
			base.DefaultHandlerType = base.DefaultHandler.GetType();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003BD4 File Offset: 0x00001DD4
		private void RaiseExecute(object dataItem)
		{
			if (this.Execute != null)
			{
				this.Execute(this, new CommandRunArgs(dataItem));
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003C04 File Offset: 0x00001E04
		private void RaiseCanArrayExecute(CommandArrayInfo cmdArrayInfo)
		{
			if (this.Update != null)
			{
				CommandArrayUpdateArgs e = new CommandArrayUpdateArgs(cmdArrayInfo);
				this.Update(this, e);
			}
		}

		// Token: 0x02000013 RID: 19
		private class DefaultCommandHanle : CommandHandler
		{
			// Token: 0x0600007D RID: 125 RVA: 0x00003C36 File Offset: 0x00001E36
			public DefaultCommandHanle(CommandArrayProxy command)
			{
				this.command = command;
			}

			// Token: 0x0600007E RID: 126 RVA: 0x00003C48 File Offset: 0x00001E48
			protected override void Run(object dataItem)
			{
				this.command.RaiseExecute(dataItem);
			}

			// Token: 0x0600007F RID: 127 RVA: 0x00003C58 File Offset: 0x00001E58
			protected override void Update(CommandArrayInfo info)
			{
				this.command.RaiseCanArrayExecute(info);
			}

			// Token: 0x04000084 RID: 132
			private CommandArrayProxy command;
		}
	}
}
