using System;
using CocoStudio.Core.Commands;
using Gtk;
using MonoDevelop.Components.Commands;

namespace Modules.Communal.PropertyGrid
{
	public class NoUndoEntry : EntryEx
	{
		[CommandHandler(CmdEnum.UndoCmd)]
		[CommandHandler(CmdEnum.RedoCmd)]
		private void OnUndoHandle()
		{
		}
	}
}
