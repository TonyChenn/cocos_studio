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
	// Token: 0x02000004 RID: 4
	public class MenuManager
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002180 File Offset: 0x00000380
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

		// Token: 0x0600000C RID: 12 RVA: 0x000021AF File Offset: 0x000003AF
		public MenuManager()
		{
			this.InitMenu();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021BD File Offset: 0x000003BD
		private void InitMenu()
		{
			if (Platform.IsMac)
			{
				this.InitMacMenu();
				return;
			}
			this.InitWindowsMenu();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021D4 File Offset: 0x000003D4
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

		// Token: 0x0600000F RID: 15 RVA: 0x00002244 File Offset: 0x00000444
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

		// Token: 0x06000010 RID: 16 RVA: 0x00002334 File Offset: 0x00000534
		private void HandleSettingCmdExecuted(object sender, CommandRunArgs e)
		{
			PreferencesDialog preferencesDialog = new PreferencesDialog(EnumPreferenceSetting.Default);
			preferencesDialog.Run();
			preferencesDialog.Destroy();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002358 File Offset: 0x00000558
		private void HandleAboutCmdExecuted(object sender, CommandRunArgs e)
		{
			CustomTitleWindow customTitleWindow = new CustomTitleWindow();
			customTitleWindow.InitView(LanguageInfo.Menu_Launcher_AboutCocos, new AboutContentWidget());
			customTitleWindow.Show();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002381 File Offset: 0x00000581
		private void QuitCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.MainWindow.MainWindowQuit();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000238E File Offset: 0x0000058E
		private void MinimizeCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.MainWindow.Iconify();
		}

		// Token: 0x0400000A RID: 10
		private Menu windowsMenu;
	}
}
