using System;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	public class CommandProxy : ActionCommand
	{
		public CmdGroupEnum GroupType { get; internal set; }

		public bool IsLocal { get; internal set; }

		public bool IsEnable
		{
			get
			{
				CommandInfo updateInfo = this.GetUpdateInfo();
				return updateInfo.Enabled;
			}
		}

		internal CommandProxy()
		{
			base.DefaultHandler = new CommandProxy.DefaultCommandHanle(this);
			base.DefaultHandlerType = base.DefaultHandler.GetType();
		}

		public event EventHandler<CommandRunArgs> Execute;

		public event EventHandler<CommandUpdateArgs> Update;

		public void RaiseExecute(object dataItem = null)
		{
			if (this.Execute != null)
			{
				this.Execute(this, new CommandRunArgs(dataItem));
			}
		}

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

		private CommandInfo GetUpdateInfo()
		{
			CommandInfo commandInfo = Services.CommandService.GetCommandInfo(base.Id);
			if (commandInfo != null)
			{
				this.RaiseCanExecute(commandInfo);
			}
			return commandInfo;
		}

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

		private class DefaultCommandHanle : CommandHandler
		{
			public DefaultCommandHanle(CommandProxy command)
			{
				this.command = command;
			}

			protected override void Run()
			{
				this.command.RaiseExecute(null);
			}

			protected override void Update(CommandInfo info)
			{
				this.command.RaiseCanExecute(info);
			}

			private CommandProxy command;
		}
	}
}
