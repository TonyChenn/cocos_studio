using System;
using Gtk;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;

namespace CocoStudio.Core.Commands
{
	public class MenuCreator
	{
		public static Menu CreatePopupMenu()
		{
			Menu menu = MenuCreator.CreateMenuSet("", false, null, null).Submenu as Menu;
			menu.HasTooltip = false;
			return menu;
		}

		public static MenuItem CreateMenuItem(CommandEntry ce)
		{
			return ce.CreateMenuItem(MenuCreator.cmdManager);
		}

		public static MenuItem CreateMenuItem(Command cmd, bool autoHide = false, string label = null)
		{
			MenuCreator.CommandEntryHelper commandEntryHelper;
			if (label == null)
			{
				commandEntryHelper = new MenuCreator.CommandEntryHelper(cmd);
			}
			else
			{
				commandEntryHelper = new MenuCreator.CommandEntryHelper(cmd, label);
			}
			commandEntryHelper.DisabledVisible = !autoHide;
			MenuItem menuItem = commandEntryHelper.GetMenuItem();
			menuItem.HasTooltip = false;
			menuItem.ButtonReleaseEvent += MenuCreator.MenuItemBtnReleasedHandler;
			return menuItem;
		}

		public static MenuItem CreateDelayCloseMenuItem(Command cmd, bool autoHide = false, string label = null)
		{
			MenuCreator.CommandEntryHelper commandEntryHelper;
			if (label == null)
			{
				commandEntryHelper = new MenuCreator.CommandEntryHelper(cmd);
			}
			else
			{
				commandEntryHelper = new MenuCreator.CommandEntryHelper(cmd, label);
			}
			commandEntryHelper.DisabledVisible = !autoHide;
			MenuItem menuItem = commandEntryHelper.GetMenuItem();
			menuItem.HasTooltip = false;
			menuItem.ButtonReleaseEvent += MenuCreator.DelayHideMenuItemBtnReleasedHandler;
			return menuItem;
		}

		public static MenuItem CreateMenuSet(object id, bool autothide = false, string label = null, string icon = null)
		{
			if (id == null)
			{
				id = string.Empty;
			}
			if (label == null)
			{
				label = id.ToString();
			}
			label = StringParserService.Parse(label);
			return new MenuCreator.CommandEntrySetHelper(label, icon)
			{
				CommandId = id,
				AutoHide = autothide
			}.GetMenuItem(MenuCreator.cmdManager);
		}

		public static MenuItem CreateMenuSet(Command cmd, bool autoHide = false, string label = null)
		{
			MenuCreator.CommandEntryHelper commandEntryHelper;
			if (label == null)
			{
				commandEntryHelper = new MenuCreator.CommandEntryHelper(cmd);
			}
			else
			{
				commandEntryHelper = new MenuCreator.CommandEntryHelper(cmd, label);
			}
			commandEntryHelper.DisabledVisible = !autoHide;
			MenuItem menuItem = commandEntryHelper.GetMenuItem();
			menuItem.Submenu = MenuCreator.CreatePopupMenu();
			return menuItem;
		}

		public static MenuItem CreateMenuSeperator()
		{
			return new SeparatorMenuItem();
		}

		public static CommandEntrySet CreateMacCES(object id, bool autoHide = false, string label = null, string icon = null)
		{
			if (id == null)
			{
				id = string.Empty;
			}
			if (label == null)
			{
				label = id.ToString();
			}
			label = StringParserService.Parse(label);
			return new CommandEntrySet(label, icon)
			{
				CommandId = id,
				AutoHide = autoHide
			};
		}

		private static void MenuItemBtnReleasedHandler(object sender, ButtonReleaseEventArgs args)
		{
			args.RetVal = true;
			MenuItem menuItem = sender as MenuItem;
			Menu menu = menuItem.Parent as Menu;
			menu.Deactivate();
			menuItem.Activate();
		}

		private static void DelayHideMenuItemBtnReleasedHandler(object sender, ButtonReleaseEventArgs args)
		{
			args.RetVal = true;
			MenuItem menuItem = sender as MenuItem;
			Menu menu = menuItem.Parent as Menu;
			menuItem.Activate();
			menu.Deactivate();
		}

		private static CommandManager cmdManager = Services.CommandService;

		private class CommandEntryHelper : CommandEntry
		{
			public CommandEntryHelper(Command cmd) : base(cmd)
			{
			}

			public CommandEntryHelper(Command cmd, string label) : base(cmd.Id, label, true)
			{
			}

			public MenuItem GetMenuItem()
			{
				return this.CreateMenuItem(MenuCreator.cmdManager);
			}

			public ToolItem GetToolItem()
			{
				return this.CreateToolItem(MenuCreator.cmdManager);
			}
		}

		private class CommandEntrySetHelper : CommandEntrySet
		{
			public CommandEntrySetHelper(string name, IconId icon) : base(name, icon)
			{
			}

			public MenuItem GetMenuItem(CommandManager cmdManager)
			{
				return this.CreateMenuItem(cmdManager);
			}

			public ToolItem GetToolItem(CommandManager cmdManager)
			{
				return this.CreateToolItem(cmdManager);
			}
		}
	}
}
