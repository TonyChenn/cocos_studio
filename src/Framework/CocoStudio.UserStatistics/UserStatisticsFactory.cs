using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using GLib;
using Gtk;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000014 RID: 20
	public static class UserStatisticsFactory
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000362C File Offset: 0x0000182C
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003643 File Offset: 0x00001843
		public static List<IUserStatistics> ListTracker
		{
			get
			{
				return UserStatisticsFactory.listTracker;
			}
			set
			{
				UserStatisticsFactory.listTracker = value;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000364C File Offset: 0x0000184C
		static UserStatisticsFactory()
		{
			UserStatisticsFactory.ListTracker.Add(new UserStatisitcsCS());
			UserStatisticsFactory.InitUnhandledException();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003670 File Offset: 0x00001870
		public static void Start(Window mainWindow)
		{
			if (mainWindow is IWindowClosed)
			{
				(mainWindow as IWindowClosed).Closed += UserStatisticsFactory.UserStatisticsFactory_Closed;
			}
			foreach (IUserStatistics userStatistics in UserStatisticsFactory.ListTracker)
			{
				userStatistics.EditorInfo = new EditorInfo(Option.CurrentApp.ToString(), Option.EditorVersion);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003708 File Offset: 0x00001908
		private static void UserStatisticsFactory_Closed(object sender, EventArgs e)
		{
			UserStatisticsFactory.ExitAll();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003714 File Offset: 0x00001914
		private static void Exit()
		{
			foreach (IUserStatistics userStatistics in UserStatisticsFactory.ListTracker)
			{
				userStatistics.Exit();
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003770 File Offset: 0x00001970
		public static void ExitAll()
		{
			foreach (IUserStatistics userStatistics in UserStatisticsFactory.ListTracker)
			{
				userStatistics.ExitAll();
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600005C RID: 92 RVA: 0x000037CC File Offset: 0x000019CC
		// (remove) Token: 0x0600005D RID: 93 RVA: 0x00003808 File Offset: 0x00001A08
		public static event Action<EventArgs> UnHandledExceptionEvent;

		// Token: 0x0600005E RID: 94 RVA: 0x00003842 File Offset: 0x00001A42
		private static void InitUnhandledException()
		{
			ExceptionManager.UnhandledException += UserStatisticsFactory.HandleUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += UserStatisticsFactory.HandleDomainUnhandledException;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000386E File Offset: 0x00001A6E
		private static void HandleDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			UserStatisticsFactory.HandledException(e.ExceptionObject as Exception);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003882 File Offset: 0x00001A82
		private static void HandleUnhandledException(UnhandledExceptionArgs args)
		{
			UserStatisticsFactory.HandledException(args.ExceptionObject as Exception);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003898 File Offset: 0x00001A98
		private static void HandledException(Exception ex)
		{
			if (UserStatisticsFactory.UnHandledExceptionEvent != null)
			{
				UserStatisticsFactory.UnHandledExceptionEvent(new EventArgs());
			}
			string message = string.Format("Unhandled exception. Exception is {0}, Stack is {1}", ex.Message, ex.GetAllStackTrace());
			LogConfig.Logger.Error(message);
			FeedBackDialog feedBackDialog = new FeedBackDialog();
			feedBackDialog.Run();
			feedBackDialog.Destroy();
			foreach (IUserStatistics userStatistics in UserStatisticsFactory.ListTracker)
			{
				userStatistics.UnHandledException(ex, feedBackDialog.FeedbackInfo);
			}
			Option.UserConfig.FeedBackEmail = string.Empty;
			Option.UserConfig.Save();
			Environment.Exit(-1);
		}

		// Token: 0x04000070 RID: 112
		private static List<IUserStatistics> listTracker = new List<IUserStatistics>();
	}
}
