using System;
using CocoStudio.Core.Commands;
using Gtk;
using MonoDevelop.Components.Commands;

namespace Modules.Communal.PropertyGrid
{
	public class NoUndoNumEntry : EntryIntEx
	{
		[CommandHandler(CmdEnum.RedoCmd)]
		[CommandHandler(CmdEnum.UndoCmd)]
		private void OnUndoHandle()
		{
		}
	}
}
