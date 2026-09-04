using System;
using Gtk;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000051 RID: 81
	internal static class MainPartFactory
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000ADD9 File Offset: 0x00008FD9
		public static Widget GetWindowTitle(MainWindow mainWindow)
		{
			return new TitleView(mainWindow);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000ADE1 File Offset: 0x00008FE1
		public static Widget GetTopContent()
		{
			return new BannerView();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000ADE8 File Offset: 0x00008FE8
		public static Widget GetMainContent()
		{
			return new ContentView();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000ADEF File Offset: 0x00008FEF
		public static MenuManager GetMenuManager()
		{
			return new MenuManager();
		}
	}
}
