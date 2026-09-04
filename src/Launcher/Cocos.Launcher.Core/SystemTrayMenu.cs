using System;
using System.Windows.Forms;
using AppKit;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000013 RID: 19
	internal class SystemTrayMenu
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004C0B File Offset: 0x00002E0B
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00004C12 File Offset: 0x00002E12
		public static SystemTrayMenu Instance { get; private set; } = new SystemTrayMenu();

		// Token: 0x0600008F RID: 143 RVA: 0x00004C26 File Offset: 0x00002E26
		private SystemTrayMenu()
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004C30 File Offset: 0x00002E30
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

		// Token: 0x06000091 RID: 145 RVA: 0x00004CC4 File Offset: 0x00002EC4
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

		// Token: 0x06000092 RID: 146 RVA: 0x00004D44 File Offset: 0x00002F44
		private void exit_Activated(object sender, EventArgs e)
		{
			bool flag = Services.MainWindow.MainWindowQuit();
			if (flag)
			{
				SystemTrayService.Instace.Dispose();
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004D6C File Offset: 0x00002F6C
		private void aboutCocos_Activated(object sender, EventArgs e)
		{
			CustomTitleWindow customTitleWindow = new CustomTitleWindow();
			customTitleWindow.InitView(LanguageInfo.Menu_Launcher_AboutCocos, new AboutContentWidget());
			customTitleWindow.Show();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004D95 File Offset: 0x00002F95
		private void openCocos_Activated(object sender, EventArgs e)
		{
			Services.MainWindow.PresentWindow();
		}
	}
}
