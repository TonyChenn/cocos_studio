using System;
using Gtk;

namespace CocoStudio.Core.Commands
{
	internal class FileTabCommand
	{
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

		private static Menu tabPopupMenu;

		public enum FileTabCommands
		{
			CloseAll,
			CloseAllButThis
		}
	}
}
