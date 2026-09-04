using System;
using Gtk;

namespace CocoStudio.Core.Commands
{
	// Token: 0x02000014 RID: 20
	internal class FileTabCommand
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003C68 File Offset: 0x00001E68
		public static Menu TabPopupMenu
		{
			get
			{
				if (FileTabCommand.tabPopupMenu == null)
				{
					FileTabCommand.tabPopupMenu = FileTabCommand.InitPopupMenu();
				}
				return FileTabCommand.tabPopupMenu;
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003C9C File Offset: 0x00001E9C
		private static Menu InitPopupMenu()
		{
			Menu menu = MenuCreator.CreatePopupMenu();
			menu.Append(MenuCreator.CreateMenuItem(GlobalCommand.CloseCmd, false, null));
			menu.Append(MenuCreator.CreateMenuItem(GlobalCommand.CloseAllCmd, false, null));
			menu.Append(MenuCreator.CreateMenuItem(GlobalCommand.CloseOtherCmd, false, null));
			menu.Append(MenuCreator.CreateMenuSeperator());
			menu.Append(MenuCreator.CreateMenuItem(GlobalCommand.SaveCmd, false, null));
			menu.Append(MenuCreator.CreateMenuItem(GlobalCommand.SaveAllCmd, false, null));
			menu.Append(MenuCreator.CreateMenuSeperator());
			menu.Append(MenuCreator.CreateMenuItem(GlobalCommand.OpenDirCmd, false, null));
			return menu;
		}

		// Token: 0x04000085 RID: 133
		private static Menu tabPopupMenu;

		// Token: 0x02000015 RID: 21
		public enum FileTabCommands
		{
			// Token: 0x04000087 RID: 135
			CloseAll,
			// Token: 0x04000088 RID: 136
			CloseAllButThis
		}
	}
}
