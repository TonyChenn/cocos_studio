using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core.Service;
using CocoStudio.Core.View;
using CocoStudio.UserStatistics;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	// Token: 0x02000051 RID: 81
	public static class Starter
	{
		// Token: 0x06000327 RID: 807 RVA: 0x0000E62C File Offset: 0x0000C82C
		public static void Initialize(EnumApp appType, string themeName)
		{
			Starter.SetCurrentIDE(appType);
			Platform.Initialize();
			try
			{
				GLibLogging.Log.SetLogHandler("", GLibLogging.LogLevelFlags.All, delegate(string log_domain, GLibLogging.LogLevelFlags log_level, string message)
				{
					LogConfig.Logger.Error(message);
				});
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Error initialising GLib logging.", exception);
			}
			Starter.SetupTheme(themeName);
			DispatchService.Initialize();
			Tuple<string, string> directory = Starter.GetDirectory(appType);
			CocoStudio.Core.Service.Runtime.Initialize(directory.Item1, directory.Item2);
			Starter.InitApplication();
			Services.IntinalizeCompleted += Starter.Services_IntinalizeCompleted;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
		private static Tuple<string, string> GetDirectory(EnumApp editorType)
		{
			Tuple<string, string> result;
			if (editorType == EnumApp.Studio || editorType == EnumApp.Tool)
			{
				result = new Tuple<string, string>(Option.AddinConfigFolder, Option.AddinLocationFolder);
			}
			else
			{
				result = new Tuple<string, string>(Option.LauncherAddinConfigFolder, null);
			}
			return result;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E721 File Offset: 0x0000C921
		private static void Services_IntinalizeCompleted(EventArgs obj)
		{
			Services.IntinalizeCompleted -= Starter.Services_IntinalizeCompleted;
			UserStatisticsFactory.Start(ApplicationCurrent.MainWindow);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E744 File Offset: 0x0000C944
		public static void Run()
		{
			IProgressMonitor monitor = new ConsoleProgressMonitor();
			CocoStudio.Core.View.Workbench workbench = new CocoStudio.Core.View.Workbench();
			workbench.Initialize(monitor);
			workbench.Show("Cocos Studio");
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E772 File Offset: 0x0000C972
		private static void InitApplication()
		{
			Application.Init();
			PlatformAdapter.Initialize();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E781 File Offset: 0x0000C981
		private static void SetCurrentIDE(EnumApp editorType)
		{
			Option.SetCurrentIDE(editorType);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000E78C File Offset: 0x0000C98C
		public static void SetupTheme(string themeName)
		{
			try
			{
				Rc.ParseString("gtk-theme-name = Default");
				string path;
				if (string.IsNullOrEmpty(themeName))
				{
					path = "theme_clearlooks";
				}
				else
				{
					path = themeName;
				}
				string path2;
				if (Platform.IsWindows)
				{
					path2 = "gtkrc.win32";
				}
				else
				{
					path2 = "gtkrc.mac";
				}
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				string text = Path.Combine(baseDirectory, path);
				text = Path.Combine(text, path2);
				bool flag = true;
				if (flag)
				{
					Environment.SetEnvironmentVariable("GTK2_RC_FILES", text);
				}
				else
				{
					Environment.SetEnvironmentVariable("GTK2_RC_FILES", "D:\\Repository\\CocoStudio-2.0\\CocoStudioMono\\CocoStudio.Core\\theme_clearlooks\\gtkrc.win32");
					text = "D:\\Repository\\CocoStudio-2.0\\CocoStudioMono\\CocoStudio.Core\\theme_clearlooks\\gtkrc.win32";
				}
				using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
				{
					using (TextReader textReader = new StreamReader(fileStream))
					{
						string rc_string = textReader.ReadToEnd();
						Rc.ParseString(rc_string);
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Read theme file failed,", exception);
			}
		}
	}
}
