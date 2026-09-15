using System;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gtk;
using MonoDevelop.Core;

namespace Modules.Communal.MutualEditor
{
	public class MutualCore
	{
		public static void Init()
		{
			if (MutualCore.mutualcore != null)
			{
				MutualCore.mutualcore.Dispose();
			}
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				MutualCore.mutualcore = new LauncherHandler();
			}
			else if (Option.CurrentApp == EnumApp.Installer)
			{
				MutualCore.mutualcore = new InstallerHandler();
			}
			else
			{
				MutualCore.mutualcore = new EditorHandler();
			}
		}

		public static IUDPHandler Instance
		{
			get
			{
				if (MutualCore.mutualcore == null)
				{
					MutualCore.Init();
				}
				return MutualCore.mutualcore;
			}
		}

		public static void ShowCurrApplication()
		{
			if (Platform.IsMac)
			{
				if (ApplicationCurrent.MainWindow != null)
				{
					ApplicationCurrent.MainWindow.Deiconify();
					ApplicationCurrent.MainWindow.Visible = true;
				}
			}
			else
			{
				Services.MainWindow.GdkWindow.Show();
				Services.MainWindow.GdkWindow.Deiconify();
			}
		}

		private static IUDPHandler mutualcore;
	}
}
