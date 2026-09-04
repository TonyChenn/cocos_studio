using System;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	// Token: 0x02000010 RID: 16
	public class CommandProxy : ActionCommand
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000037DC File Offset: 0x000019DC
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000037F3 File Offset: 0x000019F3
		public CmdGroupEnum GroupType { get; internal set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000037FC File Offset: 0x000019FC
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00003813 File Offset: 0x00001A13
		public bool IsLocal { get; internal set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000381C File Offset: 0x00001A1C
		public bool IsEnable
		{
			get
			{
				CommandInfo updateInfo = this.GetUpdateInfo();
				return updateInfo.Enabled;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000383B File Offset: 0x00001A3B
		internal CommandProxy()
		{
			base.DefaultHandler = new CommandProxy.DefaultCommandHanle(this);
			base.DefaultHandlerType = base.DefaultHandler.GetType();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600006B RID: 107 RVA: 0x00003868 File Offset: 0x00001A68
		// (remove) Token: 0x0600006C RID: 108 RVA: 0x000038A4 File Offset: 0x00001AA4
		public event EventHandler<CommandRunArgs> Execute;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600006D RID: 109 RVA: 0x000038E0 File Offset: 0x00001AE0
		// (remove) Token: 0x0600006E RID: 110 RVA: 0x0000391C File Offset: 0x00001B1C
		public event EventHandler<CommandUpdateArgs> Update;

		// Token: 0x0600006F RID: 111 RVA: 0x00003958 File Offset: 0x00001B58
		public void RaiseExecute(object dataItem = null)
		{
			if (this.Execute != null)
			{
				this.Execute(this, new CommandRunArgs(dataItem));
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003988 File Offset: 0x00001B88
		public string GetTooltipText(bool hasHotkey = true)
		{
			CommandInfo updateInfo = this.GetUpdateInfo();
			string text = (updateInfo != null) ? updateInfo.Text : base.Text;
			string result;
			if (hasHotkey)
			{
				string text2 = KeyBindingManager.BindingToDisplayLabel(base.KeyBinding, true);
				if (string.IsNullOrWhiteSpace(text2))
				{
					result = text;
				}
				else
				{
					result = text + string.Format(" ({0})", text2);
				}
			}
			else
			{
				result = text;
			}
			return result;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000039F8 File Offset: 0x00001BF8
		private CommandInfo GetUpdateInfo()
		{
			CommandInfo commandInfo = Services.CommandService.GetCommandInfo(base.Id);
			if (commandInfo != null)
			{
				this.RaiseCanExecute(commandInfo);
			}
			return commandInfo;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003A2C File Offset: 0x00001C2C
		private void RaiseCanExecute(CommandInfo cmdInfo)
		{
			if (this.Execute == null)
			{
				cmdInfo.Enabled = false;
			}
			else if (this.Update != null)
			{
				CommandUpdateArgs e = new CommandUpdateArgs(cmdInfo);
				this.Update(this, e);
			}
			else
			{
				cmdInfo.Enabled = true;
			}
		}

		// Token: 0x02000011 RID: 17
		private class DefaultCommandHanle : CommandHandler
		{
			// Token: 0x06000073 RID: 115 RVA: 0x00003A85 File Offset: 0x00001C85
			public DefaultCommandHanle(CommandProxy command)
			{
				this.command = command;
			}

			// Token: 0x06000074 RID: 116 RVA: 0x00003A97 File Offset: 0x00001C97
			protected override void Run()
			{
				this.command.RaiseExecute(null);
			}

			// Token: 0x06000075 RID: 117 RVA: 0x00003AA7 File Offset: 0x00001CA7
			protected override void Update(CommandInfo info)
			{
				this.command.RaiseCanExecute(info);
			}

			// Token: 0x04000081 RID: 129
			private CommandProxy command;
		}
	}
}
