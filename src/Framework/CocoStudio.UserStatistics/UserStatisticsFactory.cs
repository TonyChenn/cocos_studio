using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using GLib;
using Gtk;

namespace CocoStudio.UserStatistics
{
	public static class UserStatisticsFactory
	{
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

		static UserStatisticsFactory()
		{
			UserStatisticsFactory.ListTracker.Add(new UserStatisitcsCS());
			UserStatisticsFactory.InitUnhandledException();
		}

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

		private static void UserStatisticsFactory_Closed(object sender, EventArgs e)
		{
			UserStatisticsFactory.ExitAll();
		}

		private static void Exit()
		{
			foreach (IUserStatistics userStatistics in UserStatisticsFactory.ListTracker)
			{
				userStatistics.Exit();
			}
		}

		public static void ExitAll()
		{
			foreach (IUserStatistics userStatistics in UserStatisticsFactory.ListTracker)
			{
				userStatistics.ExitAll();
			}
		}

		public static event Action<EventArgs> UnHandledExceptionEvent;

		private static void InitUnhandledException()
		{
			ExceptionManager.UnhandledException += UserStatisticsFactory.HandleUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += UserStatisticsFactory.HandleDomainUnhandledException;
		}

		private static void HandleDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			UserStatisticsFactory.HandledException(e.ExceptionObject as Exception);
		}

		private static void HandleUnhandledException(UnhandledExceptionArgs args)
		{
			UserStatisticsFactory.HandledException(args.ExceptionObject as Exception);
		}

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

		private static List<IUserStatistics> listTracker = new List<IUserStatistics>();
	}
}
