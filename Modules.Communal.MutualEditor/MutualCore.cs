using System;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gtk;
using MonoDevelop.Core;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x02000008 RID: 8
	public class MutualCore
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000025C8 File Offset: 0x000007C8
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

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002630 File Offset: 0x00000830
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

		// Token: 0x0600001B RID: 27 RVA: 0x00002660 File Offset: 0x00000860
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

		// Token: 0x04000009 RID: 9
		private static IUDPHandler mutualcore;
	}
}
