using System;
using System.IO;
using Cocos.Launcher.Control;
using Cocos.Launcher.Core;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.MutualEditor;

namespace Cocos.Launcher.Start
{
	internal class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Starter.Initialize(EnumApp.Launcher, "theme_launcher");
			LanguageOption.Init();
			if (!Program.PreStartCheck())
			{
				return;
			}
			ParseArguments parseArguments = new ParseArguments(args);
			CocoStudio.Core.Services.Initialize();
			Cocos.Launcher.Core.Services.Intinalize();
			Cocos.Launcher.Core.MainWindow mainWindow = new Cocos.Launcher.Core.MainWindow();
			if (parseArguments.IsAutoStart)
			{
				mainWindow.Hide();
			}
			else
			{
				mainWindow.Show();
			}
			Cocos.Launcher.Core.Services.TabGroupService.SwitchTab(new SwitchTabInfo(parseArguments.DefaultPageIndex));
			SystemTrayService.Instace.Start();
			Application.Run();
		}

		private static bool PreStartCheck()
		{
			MutualCore.Init();
			if (File.Exists(ConstantConfig.Paths.LauncherLockPath) && SolutionLockHandler.Instance.IsFileLocked(ConstantConfig.Paths.LauncherLockPath))
			{
				MutualCore.Instance.SendMessage("", Modules.Communal.MutualEditor.Action.Show);
				MutualCore.Instance.Dispose();
				return false;
			}
			SolutionLockHandler.Instance.TryLockFile(ConstantConfig.Paths.LauncherLockPath);
			return true;
		}
	}
}
