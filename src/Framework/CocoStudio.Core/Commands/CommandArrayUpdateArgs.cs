using System;
using CocoStudio.Core.ExtensionModel;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	public class CommandArrayUpdateArgs : EventArgs
	{
		public MenuArrayInfo Info { get; private set; }

		internal CommandArrayUpdateArgs(CommandArrayInfo cmdArrayInfo)
		{
			this.Info = new MenuArrayInfo(cmdArrayInfo);
		}
	}
}
