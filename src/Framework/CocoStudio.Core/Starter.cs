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
	public static class Starter
	{
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

		private static Tuple<string, string> GetDirectory(EnumApp editorType)
		{
			string text;
			string item;
			if (editorType == EnumApp.Studio || editorType == EnumApp.Tool)
			{
				text = Option.AddinConfigFolder;
				item = Option.AddinLocationFolder;
			}
			else
			{
				text = Option.LauncherAddinConfigFolder;
				item = null;
			}
			if (!Starter.IsInstalledLocation())
			{
				text = Path.Combine(Option.AssemblyDir, "AddinConfig", editorType.ToString());
			}
			return new Tuple<string, string>(text, item);
		}

		private static bool IsInstalledLocation()
		{
			if (string.IsNullOrWhiteSpace(Option.AssemblyDir) || string.IsNullOrWhiteSpace(Option.CocosInstallDir))
			{
				return false;
			}
			string text = Path.GetFullPath(Option.AssemblyDir).TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			string text2 = Path.GetFullPath(Option.CocosInstallDir).TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			return text.Equals(text2, StringComparison.OrdinalIgnoreCase) || text.StartsWith(text2 + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
		}

		private static void Services_IntinalizeCompleted(EventArgs obj)
		{
			Services.IntinalizeCompleted -= Starter.Services_IntinalizeCompleted;
			UserStatisticsFactory.Start(ApplicationCurrent.MainWindow);
		}

		public static void Run()
		{
			IProgressMonitor monitor = new ConsoleProgressMonitor();
			CocoStudio.Core.View.Workbench workbench = new CocoStudio.Core.View.Workbench();
			workbench.Initialize(monitor);
			workbench.Show("Cocos Studio");
		}

		private static void InitApplication()
		{
			Application.Init();
			PlatformAdapter.Initialize();
		}

		private static void SetCurrentIDE(EnumApp editorType)
		{
			Option.SetCurrentIDE(editorType);
		}

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
