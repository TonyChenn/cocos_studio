using System;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	public class CommandArrayProxy : ActionCommand
	{
		public event EventHandler<CommandRunArgs> Execute;

		public event EventHandler<CommandArrayUpdateArgs> Update;

		internal CommandArrayProxy()
		{
			base.DefaultHandler = new CommandArrayProxy.DefaultCommandHanle(this);
			base.DefaultHandlerType = base.DefaultHandler.GetType();
		}

		private void RaiseExecute(object dataItem)
		{
			if (this.Execute != null)
			{
				this.Execute(this, new CommandRunArgs(dataItem));
			}
		}

		private void RaiseCanArrayExecute(CommandArrayInfo cmdArrayInfo)
		{
			if (this.Update != null)
			{
				CommandArrayUpdateArgs e = new CommandArrayUpdateArgs(cmdArrayInfo);
				this.Update(this, e);
			}
		}

		private class DefaultCommandHanle : CommandHandler
		{
			public DefaultCommandHanle(CommandArrayProxy command)
			{
				this.command = command;
			}

			protected override void Run(object dataItem)
			{
				this.command.RaiseExecute(dataItem);
			}

			protected override void Update(CommandArrayInfo info)
			{
				this.command.RaiseCanArrayExecute(info);
			}

			private CommandArrayProxy command;
		}
	}
}
