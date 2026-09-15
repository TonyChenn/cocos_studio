using System;
using System.Windows.Forms;
using AppKit;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	internal class SystemTrayMenu
	{
		public static SystemTrayMenu Instance { get; private set; } = new SystemTrayMenu();

		private SystemTrayMenu()
		{
		}

		public ContextMenu CreateWinPopupMenu()
		{
			ContextMenu contextMenu = new ContextMenu();
			System.Windows.Forms.MenuItem menuItem = new System.Windows.Forms.MenuItem(LanguageInfo.Launcher_OpenCocos);
			menuItem.Click += this.openCocos_Activated;
			System.Windows.Forms.MenuItem menuItem2 = new System.Windows.Forms.MenuItem(LanguageInfo.Menu_Launcher_AboutCocos);
			menuItem2.Click += this.aboutCocos_Activated;
			System.Windows.Forms.MenuItem menuItem3 = new System.Windows.Forms.MenuItem(LanguageInfo.Menu_File_Exit);
			menuItem3.Click += this.exit_Activated;
			contextMenu.MenuItems.Add(menuItem);
			contextMenu.MenuItems.Add(menuItem2);
			contextMenu.MenuItems.Add(menuItem3);
			return contextMenu;
		}

		public NSMenu CreateMacMenu()
		{
			NSMenu nsmenu = new NSMenu();
			NSMenuItem nsmenuItem = new NSMenuItem(LanguageInfo.Launcher_OpenCocos);
			nsmenuItem.Activated += this.openCocos_Activated;
			NSMenuItem nsmenuItem2 = new NSMenuItem(LanguageInfo.Menu_Launcher_AboutCocos);
			nsmenuItem2.Activated += this.aboutCocos_Activated;
			NSMenuItem nsmenuItem3 = new NSMenuItem(LanguageInfo.Menu_File_Exit);
			nsmenuItem3.Activated += this.exit_Activated;
			nsmenu.AddItem(nsmenuItem);
			nsmenu.AddItem(nsmenuItem2);
			nsmenu.AddItem(nsmenuItem3);
			return nsmenu;
		}

		private void exit_Activated(object sender, EventArgs e)
		{
			bool flag = Services.MainWindow.MainWindowQuit();
			if (flag)
			{
				SystemTrayService.Instace.Dispose();
			}
		}

		private void aboutCocos_Activated(object sender, EventArgs e)
		{
			CustomTitleWindow customTitleWindow = new CustomTitleWindow();
			customTitleWindow.InitView(LanguageInfo.Menu_Launcher_AboutCocos, new AboutContentWidget());
			customTitleWindow.Show();
		}

		private void openCocos_Activated(object sender, EventArgs e)
		{
			Services.MainWindow.PresentWindow();
		}
	}
}
