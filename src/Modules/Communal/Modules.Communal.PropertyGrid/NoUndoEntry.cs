using System;
using CocoStudio.Core.Commands;
using Gtk;
using MonoDevelop.Components.Commands;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000006 RID: 6
	public class NoUndoEntry : EntryEx
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002198 File Offset: 0x00000398
		[CommandHandler(CmdEnum.UndoCmd)]
		[CommandHandler(CmdEnum.RedoCmd)]
		private void OnUndoHandle()
		{
		}
	}
}
