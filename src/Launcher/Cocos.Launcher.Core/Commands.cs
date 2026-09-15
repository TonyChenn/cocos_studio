using System;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components.Commands;

namespace Cocos.Launcher.Core
{
	internal class Commands
	{
		static Commands()
		{
			try
			{
				Commands.QuitCmd = CommandCreater.CreateGlobalCommand(EnumCommands.QuitCmd, LanguageInfo.Menu_File_Exit, CmdGroupEnum.NoHotkey, "Control|Q", "Meta|Q", MonoDevelop.Components.Commands.ActionType.Normal);
				Services.CommandService.RegisterCommand(Commands.QuitCmd);
				Commands.MinimizeCmd = CommandCreater.CreateGlobalCommand(EnumCommands.MinimizeCmd, LanguageInfo.Menu_Launcher_Minimize, CmdGroupEnum.NoHotkey, "", "Meta|M", MonoDevelop.Components.Commands.ActionType.Normal);
				Services.CommandService.RegisterCommand(Commands.MinimizeCmd);
				Commands.SettingCmd = CommandCreater.CreateGlobalCommand(EnumCommands.SettingCmd, LanguageInfo.Dialog_Publish_Setting, CmdGroupEnum.NoHotkey, "Control|,", "Meta|,", MonoDevelop.Components.Commands.ActionType.Normal);
				Services.CommandService.RegisterCommand(Commands.SettingCmd);
				Commands.AboutCmd = CommandCreater.CreateGlobalCommand(EnumCommands.AboutCmd, LanguageInfo.Menu_Launcher_AboutCocos, CmdGroupEnum.NoHotkey, null, null, MonoDevelop.Components.Commands.ActionType.Normal);
				Services.CommandService.RegisterCommand(Commands.AboutCmd);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("初始化Launcher菜单命令出错", exception);
			}
		}

		public static CommandProxy QuitCmd { get; private set; }

		public static CommandProxy MinimizeCmd { get; private set; }

		public static CommandProxy SettingCmd { get; private set; }

		public static CommandProxy AboutCmd { get; private set; }
	}
}
