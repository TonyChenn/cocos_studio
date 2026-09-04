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
	// Token: 0x02000049 RID: 73
	public static class Services
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000B3A0 File Offset: 0x000095A0
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000B3B6 File Offset: 0x000095B6
		public static ProjectsService ProjectsService { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000B3C0 File Offset: 0x000095C0
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000B3D6 File Offset: 0x000095D6
		public static ProjectsOperations ProjectOperations { get; private set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000B3E0 File Offset: 0x000095E0
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0000B3F6 File Offset: 0x000095F6
		public static IEventAggregator EventsService { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000B400 File Offset: 0x00009600
		// (set) Token: 0x0600028A RID: 650 RVA: 0x0000B416 File Offset: 0x00009616
		public static RootWorkspace Workspace { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000B420 File Offset: 0x00009620
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000B436 File Offset: 0x00009636
		public static MainWindow MainWindow { get; internal set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000B440 File Offset: 0x00009640
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000B456 File Offset: 0x00009656
		public static IAutoSaveManager AutoSaveService { get; private set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000B460 File Offset: 0x00009660
		public static IUndoManager TaskService
		{
			get
			{
				return TaskServiceSingleton.Instance;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000B478 File Offset: 0x00009678
		// (set) Token: 0x06000291 RID: 657 RVA: 0x0000B48E File Offset: 0x0000968E
		public static RecentFilesService RecentFileService { get; private set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000B498 File Offset: 0x00009698
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000B4AE File Offset: 0x000096AE
		public static INetworkService NetworkService { get; private set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000B4B8 File Offset: 0x000096B8
		// (set) Token: 0x06000295 RID: 661 RVA: 0x0000B4CE File Offset: 0x000096CE
		public static Workbench Workbench { get; internal set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000B4D8 File Offset: 0x000096D8
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000B4EE File Offset: 0x000096EE
		public static ProgressMonitorManager ProgressMonitors { get; private set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000B4F8 File Offset: 0x000096F8
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000B50E File Offset: 0x0000970E
		public static RemindService RemindService { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000B518 File Offset: 0x00009718
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000B52E File Offset: 0x0000972E
		public static CommandManager CommandService { get; private set; }

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600029C RID: 668 RVA: 0x0000B538 File Offset: 0x00009738
		// (remove) Token: 0x0600029D RID: 669 RVA: 0x0000B574 File Offset: 0x00009774
		public static event Action<EventArgs> IntinalizeCompleted;

		// Token: 0x0600029F RID: 671 RVA: 0x0000B5BC File Offset: 0x000097BC
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

		// Token: 0x060002A0 RID: 672 RVA: 0x0000B62C File Offset: 0x0000982C
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

		// Token: 0x060002A1 RID: 673 RVA: 0x0000B6ED File Offset: 0x000098ED
		internal static void TestIntinalize()
		{
			Services.InternalIntinalize();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000B6F8 File Offset: 0x000098F8
		public static void RegisterService<T>(T service) where T : class, IService
		{
			if (Services.servicesCollection.ContainsKey(typeof(T)))
			{
				throw new ArgumentException("Already rigisted service.");
			}
			Services.servicesCollection[typeof(T)] = service;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000B748 File Offset: 0x00009948
		public static T GetService<T>() where T : class, IService
		{
			IService service;
			Services.servicesCollection.TryGetValue(typeof(T), out service);
			return service as T;
		}

		// Token: 0x04000137 RID: 311
		private static Dictionary<Type, IService> servicesCollection = new Dictionary<Type, IService>();

		// Token: 0x0200004A RID: 74
		public class CompletedEventArgs : EventArgs
		{
		}
	}
}
