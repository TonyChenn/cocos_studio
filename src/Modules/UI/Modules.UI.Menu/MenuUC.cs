using System;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.UserStatistics;
using Gtk;
using Mono.Addins;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;

namespace Modules.UI.Menu
{
	[Extension(Path = "/CocoStudio/Ide/MainMenuBar")]
	public class MenuUC : EventBox, IMainWindowPart
	{
		public MenuUC()
		{
			CommandEntrySet commandEntrySet = Services.CommandService.CreateCommandEntrySet("/CocoStudio/Ide/MainMenu");
			if (Platform.IsMac)
			{
				PlatformAdapter.PlatformService.SetGlobalMenu(Services.CommandService, commandEntrySet);
			}
			else if (Platform.IsWindows)
			{
				MenuBar menuBar = new MenuBar();
				menuBar.ModifyBg(StateType.Normal, WindowStyle.WindowBgColor);
				foreach (object obj in commandEntrySet)
				{
					CommandEntry ce = (CommandEntry)obj;
					MenuItem menuItem = MenuCreator.CreateMenuItem(ce);
					(menuItem.Submenu as CommandMenu).CommandSource = ViewRegions.UIMenu;
					CustomItem customItem = menuItem.Child as CustomItem;
					if (customItem != null)
					{
						customItem.SetMenuStyle(menuBar);
					}
					menuBar.Append(menuItem);
				}
				base.Add(new VBox
				{
					menuBar
				});
				base.ShowAll();
			}
		}
	}
}
