using System;
using CocoStudio.Core.Commands;
using Gtk;
using MonoDevelop.Components.Commands;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000007 RID: 7
	public class NoUndoNumEntry : EntryIntEx
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000021A3 File Offset: 0x000003A3
		[CommandHandler(CmdEnum.RedoCmd)]
		[CommandHandler(CmdEnum.UndoCmd)]
		private void OnUndoHandle()
		{
		}
	}
}
