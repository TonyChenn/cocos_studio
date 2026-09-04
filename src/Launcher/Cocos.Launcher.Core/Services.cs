using System;
using Gtk;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000041 RID: 65
	public static class Services
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00009A20 File Offset: 0x00007C20
		// (set) Token: 0x06000235 RID: 565 RVA: 0x00009A27 File Offset: 0x00007C27
		public static MainWindow MainWindow
		{
			get
			{
				return Services.mainWindow;
			}
			internal set
			{
				Services.mainWindow = value;
				ApplicationCurrent.MainWindow = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00009A35 File Offset: 0x00007C35
		// (set) Token: 0x06000237 RID: 567 RVA: 0x00009A3C File Offset: 0x00007C3C
		public static ILoginService LoginService { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00009A44 File Offset: 0x00007C44
		// (set) Token: 0x06000239 RID: 569 RVA: 0x00009A4B File Offset: 0x00007C4B
		public static IUpdateService UpdateService { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00009A53 File Offset: 0x00007C53
		// (set) Token: 0x0600023B RID: 571 RVA: 0x00009A5A File Offset: 0x00007C5A
		public static IOutputService OutputService { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00009A62 File Offset: 0x00007C62
		// (set) Token: 0x0600023D RID: 573 RVA: 0x00009A69 File Offset: 0x00007C69
		public static ITabGroup TabGroupService { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00009A71 File Offset: 0x00007C71
		// (set) Token: 0x0600023F RID: 575 RVA: 0x00009A78 File Offset: 0x00007C78
		public static IDownloadService DownloadService { get; set; }

		public static CocoStudio.Core.RecentFilesService RecentFileService
		{
			get
			{
				return CocoStudio.Core.Services.RecentFileService;
			}
		}

		public static CocoStudio.Core.INetworkService NetworkService
		{
			get
			{
				return CocoStudio.Core.Services.NetworkService;
			}
		}

		public static CocoStudio.Core.ProgressMonitorManager ProgressMonitors
		{
			get
			{
				return CocoStudio.Core.Services.ProgressMonitors;
			}
		}

		public static MonoDevelop.Components.Commands.CommandManager CommandService
		{
			get
			{
				return CocoStudio.Core.Services.CommandService;
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009A80 File Offset: 0x00007C80
		public static void Intinalize()
		{
			Services.UpdateService = Cocos.Launcher.Core.UpdateService.Instance;
			Services.OutputService = Cocos.Launcher.Core.OutputService.Instance;
			Services.DownloadService = Cocos.Launcher.Core.DownloadService.Instance;
		}

		// Token: 0x040000E0 RID: 224
		private static MainWindow mainWindow;
	}
}
