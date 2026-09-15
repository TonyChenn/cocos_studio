using System;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Preference;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	public class MenuManager
	{
		public Menu WindowsMenu
		{
			get
			{
				if (this.windowsMenu == null)
				{
					if (Commands.AboutCmd == null)
					{
						this.windowsMenu = new Menu();
					}
					else
					{
						this.windowsMenu = MenuCreator.CreatePopupMenu();
					}
				}
				return this.windowsMenu;
			}
		}

		public MenuManager()
		{
			this.InitMenu();
		}

		private void InitMenu()
		{
			if (Platform.IsMac)
			{
				this.InitMacMenu();
				return;
			}
			this.InitWindowsMenu();
		}

		private void InitWindowsMenu()
		{
			if (Commands.AboutCmd == null)
			{
				return;
			}
			Commands.SettingCmd.Execute += this.HandleSettingCmdExecuted;
			Commands.AboutCmd.Execute += this.HandleAboutCmdExecuted;
			this.WindowsMenu.Append(MenuCreator.CreateMenuItem(Commands.SettingCmd, false, null));
			this.WindowsMenu.Append(MenuCreator.CreateMenuItem(Commands.AboutCmd, false, null));
		}

		private void InitMacMenu()
		{
			if (Commands.AboutCmd == null)
			{
				return;
			}
			Commands.SettingCmd.Execute += this.HandleSettingCmdExecuted;
			Commands.AboutCmd.Execute += this.HandleAboutCmdExecuted;
			Commands.QuitCmd.Execute += this.QuitCmd_Execute;
			Commands.MinimizeCmd.Execute += this.MinimizeCmd_Execute;
			CommandEntrySet commandEntrySet = new CommandEntrySet();
			commandEntrySet.Add(new CommandEntrySet
			{
				new CommandEntry(Commands.AboutCmd),
				new CommandEntry(Command.Separator),
				new CommandEntry(Commands.SettingCmd),
				new CommandEntry(Command.Separator),
				new CommandEntry(Commands.MinimizeCmd),
				new CommandEntry(Commands.QuitCmd)
			});
			PlatformAdapter.PlatformService.SetGlobalMenu(Services.CommandService, commandEntrySet);
		}

		private void HandleSettingCmdExecuted(object sender, CommandRunArgs e)
		{
			PreferencesDialog preferencesDialog = new PreferencesDialog(EnumPreferenceSetting.Default);
			preferencesDialog.Run();
			preferencesDialog.Destroy();
		}

		private void HandleAboutCmdExecuted(object sender, CommandRunArgs e)
		{
			CustomTitleWindow customTitleWindow = new CustomTitleWindow();
			customTitleWindow.InitView(LanguageInfo.Menu_Launcher_AboutCocos, new AboutContentWidget());
			customTitleWindow.Show();
		}

		private void QuitCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.MainWindow.MainWindowQuit();
		}

		private void MinimizeCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.MainWindow.Iconify();
		}

		private Menu windowsMenu;
	}
}
