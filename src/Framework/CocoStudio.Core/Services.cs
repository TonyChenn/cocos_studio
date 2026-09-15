using System;
using System.Collections.Generic;
using System.Linq;
using Cocos.Launcher.Control;
using CocoStudio.Core.ModulesInterface;
using CocoStudio.Core.View;
using CocoStudio.Lib.Prism;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace CocoStudio.Core
{
	public static class Services
	{
		public static ProjectsService ProjectsService { get; private set; }

		public static ProjectsOperations ProjectOperations { get; private set; }

		public static IEventAggregator EventsService { get; private set; }

		public static RootWorkspace Workspace { get; private set; }

		public static MainWindow MainWindow { get; internal set; }

		public static IAutoSaveManager AutoSaveService { get; private set; }

		public static IUndoManager TaskService
		{
			get
			{
				return TaskServiceSingleton.Instance;
			}
		}

		public static RecentFilesService RecentFileService { get; private set; }

		public static INetworkService NetworkService { get; private set; }

		public static Workbench Workbench { get; internal set; }

		public static ProgressMonitorManager ProgressMonitors { get; private set; }

		public static RemindService RemindService { get; private set; }

		public static CommandManager CommandService { get; private set; }

		public static event Action<EventArgs> IntinalizeCompleted;

		public static void Initialize()
		{
			Services.InternalIntinalize();
			Services.CommandService = new CommandManager();
			IdeApp.CommandService = Services.CommandService;
			if (Services.MainWindow != null)
			{
				MessageService.RootWindow = Services.MainWindow;
				Services.CommandService.SetRootWindow(Services.MainWindow);
			}
			if (Services.IntinalizeCompleted != null)
			{
				Services.IntinalizeCompleted(new Services.CompletedEventArgs());
			}
		}

		private static void InternalIntinalize()
		{
			LanguageOption.Init();
			Services.ProjectsService = ProjectsService.Instance;
			Services.EventsService = EventAggregator.Instance;
			Services.ProjectOperations = new ProjectsOperations();
			Services.Workspace = new RootWorkspace();
			Services.RecentFileService = new RecentFilesService();
			Services.AutoSaveService = AddinManager.GetExtensionObjects<IAutoSaveManager>().FirstOrDefault<IAutoSaveManager>();
			if (Services.AutoSaveService != null)
			{
				Services.AutoSaveService.StartAutoSave();
			}
			Services.ProgressMonitors = new ProgressMonitorManager();
			Services.RemindService = RemindService.Instance;
			Services.TaskService.Enable = true;
			Services.NetworkService = CocoStudio.Core.NetworkService.Instance;
			string requestCheckUrl = ConstantConfig.Constant.RequestCheckUrl;
			Services.NetworkService.Intinalize(requestCheckUrl, null);
		}

		internal static void TestIntinalize()
		{
			Services.InternalIntinalize();
		}

		public static void RegisterService<T>(T service) where T : class, IService
		{
			if (Services.servicesCollection.ContainsKey(typeof(T)))
			{
				throw new ArgumentException("Already rigisted service.");
			}
			Services.servicesCollection[typeof(T)] = service;
		}

		public static T GetService<T>() where T : class, IService
		{
			IService service;
			Services.servicesCollection.TryGetValue(typeof(T), out service);
			return service as T;
		}

		private static Dictionary<Type, IService> servicesCollection = new Dictionary<Type, IService>();

		public class CompletedEventArgs : EventArgs
		{
		}
	}
}
