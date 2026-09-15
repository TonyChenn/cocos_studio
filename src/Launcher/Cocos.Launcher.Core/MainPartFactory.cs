using System;
using Gtk;

namespace Cocos.Launcher.Core
{
	internal static class MainPartFactory
	{
		public static Widget GetWindowTitle(MainWindow mainWindow)
		{
			return new TitleView(mainWindow);
		}

		public static Widget GetTopContent()
		{
			return new BannerView();
		}

		public static Widget GetMainContent()
		{
			return new ContentView();
		}

		public static MenuManager GetMenuManager()
		{
			return new MenuManager();
		}
	}
}
