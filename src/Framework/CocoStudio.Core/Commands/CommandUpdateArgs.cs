using System;
using CocoStudio.Core.ExtensionModel;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	public class CommandUpdateArgs : EventArgs
	{
		public MenuInfo Info { get; private set; }

		internal CommandUpdateArgs(CommandInfo cmdInfo)
		{
			this.Info = new MenuInfo(cmdInfo);
		}
	}
}
