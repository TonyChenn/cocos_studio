using System;
using Gtk;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;

namespace CocoStudio.Core.Commands
{
	// Token: 0x02000018 RID: 24
	public class MenuCreator
	{
		// Token: 0x06000136 RID: 310 RVA: 0x00005DC8 File Offset: 0x00003FC8
		public static Menu CreatePopupMenu()
		{
			Menu menu = MenuCreator.CreateMenuSet("", false, null, null).Submenu as Menu;
			menu.HasTooltip = false;
			return menu;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005DFC File Offset: 0x00003FFC
		public static MenuItem CreateMenuItem(CommandEntry ce)
		{
			return ce.CreateMenuItem(MenuCreator.cmdManager);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005E1C File Offset: 0x0000401C
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

		// Token: 0x06000139 RID: 313 RVA: 0x00005E7C File Offset: 0x0000407C
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

		// Token: 0x0600013A RID: 314 RVA: 0x00005EDC File Offset: 0x000040DC
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

		// Token: 0x0600013B RID: 315 RVA: 0x00005F48 File Offset: 0x00004148
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

		// Token: 0x0600013C RID: 316 RVA: 0x00005F9C File Offset: 0x0000419C
		public static MenuItem CreateMenuSeperator()
		{
			return new SeparatorMenuItem();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005FB4 File Offset: 0x000041B4
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

		// Token: 0x0600013E RID: 318 RVA: 0x00006018 File Offset: 0x00004218
		private static void MenuItemBtnReleasedHandler(object sender, ButtonReleaseEventArgs args)
		{
			args.RetVal = true;
			MenuItem menuItem = sender as MenuItem;
			Menu menu = menuItem.Parent as Menu;
			menu.Deactivate();
			menuItem.Activate();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006054 File Offset: 0x00004254
		private static void DelayHideMenuItemBtnReleasedHandler(object sender, ButtonReleaseEventArgs args)
		{
			args.RetVal = true;
			MenuItem menuItem = sender as MenuItem;
			Menu menu = menuItem.Parent as Menu;
			menuItem.Activate();
			menu.Deactivate();
		}

		// Token: 0x040000D0 RID: 208
		private static CommandManager cmdManager = Services.CommandService;

		// Token: 0x02000019 RID: 25
		private class CommandEntryHelper : CommandEntry
		{
			// Token: 0x06000141 RID: 321 RVA: 0x00006098 File Offset: 0x00004298
			public CommandEntryHelper(Command cmd) : base(cmd)
			{
			}

			// Token: 0x06000142 RID: 322 RVA: 0x000060A4 File Offset: 0x000042A4
			public CommandEntryHelper(Command cmd, string label) : base(cmd.Id, label, true)
			{
			}

			// Token: 0x06000143 RID: 323 RVA: 0x000060B8 File Offset: 0x000042B8
			public MenuItem GetMenuItem()
			{
				return this.CreateMenuItem(MenuCreator.cmdManager);
			}

			// Token: 0x06000144 RID: 324 RVA: 0x000060D8 File Offset: 0x000042D8
			public ToolItem GetToolItem()
			{
				return this.CreateToolItem(MenuCreator.cmdManager);
			}
		}

		// Token: 0x0200001A RID: 26
		private class CommandEntrySetHelper : CommandEntrySet
		{
			// Token: 0x06000145 RID: 325 RVA: 0x000060F5 File Offset: 0x000042F5
			public CommandEntrySetHelper(string name, IconId icon) : base(name, icon)
			{
			}

			// Token: 0x06000146 RID: 326 RVA: 0x00006104 File Offset: 0x00004304
			public MenuItem GetMenuItem(CommandManager cmdManager)
			{
				return this.CreateMenuItem(cmdManager);
			}

			// Token: 0x06000147 RID: 327 RVA: 0x00006120 File Offset: 0x00004320
			public ToolItem GetToolItem(CommandManager cmdManager)
			{
				return this.CreateToolItem(cmdManager);
			}
		}
	}
}
