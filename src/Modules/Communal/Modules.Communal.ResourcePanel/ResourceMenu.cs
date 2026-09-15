using System;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.UserStatistics;
using Gtk;
using MonoDevelop.Components.Commands;

namespace Modules.Communal.ResourcePanel
{
	public class ResourceMenu
	{
		public static Menu ContextMenu { get; private set; }

		public static Menu AdditionMenu { get; private set; }

		static ResourceMenu()
		{
			ResourceMenu.InitMenu();
		}

		public static void InitMenu()
		{
			if (ResourceMenu.ContextMenu == null)
			{
				CommandEntrySet ce = Services.CommandService.CreateCommandEntrySet("/CocoStudio/Ide/ResourcePanel/Menu/Context");
				ResourceMenu.ContextMenu = (MenuCreator.CreateMenuItem(ce).Submenu as Menu);
				(ResourceMenu.ContextMenu as CommandMenu).CommandSource = ViewRegions.ResourcePanel;
			}
			if (ResourceMenu.AdditionMenu == null)
			{
				CommandEntrySet ce2 = Services.CommandService.CreateCommandEntrySet("/CocoStudio/Ide/ResourcePanel/Menu/Addition");
				ResourceMenu.AdditionMenu = (MenuCreator.CreateMenuItem(ce2).Submenu as Menu);
				(ResourceMenu.AdditionMenu as CommandMenu).CommandSource = ViewRegions.ResourcePanelAdditionMenu;
			}
		}
	}
}
