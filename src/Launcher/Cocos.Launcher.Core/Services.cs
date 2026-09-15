using System;
using Gtk;

namespace Cocos.Launcher.Core
{
	public static class Services
	{
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

		public static ILoginService LoginService { get; set; }

		public static IUpdateService UpdateService { get; private set; }

		public static IOutputService OutputService { get; private set; }

		public static ITabGroup TabGroupService { get; set; }

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

		public static void Intinalize()
		{
			Services.UpdateService = Cocos.Launcher.Core.UpdateService.Instance;
			Services.OutputService = Cocos.Launcher.Core.OutputService.Instance;
			Services.DownloadService = Cocos.Launcher.Core.DownloadService.Instance;
		}

		private static MainWindow mainWindow;
	}
}
