using System;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components.Commands;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000003 RID: 3
	internal class Commands
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
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

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000213C File Offset: 0x0000033C
		// (set) Token: 0x06000003 RID: 3 RVA: 0x00002143 File Offset: 0x00000343
		public static CommandProxy QuitCmd { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x0000214B File Offset: 0x0000034B
		// (set) Token: 0x06000005 RID: 5 RVA: 0x00002152 File Offset: 0x00000352
		public static CommandProxy MinimizeCmd { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000215A File Offset: 0x0000035A
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002161 File Offset: 0x00000361
		public static CommandProxy SettingCmd { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002169 File Offset: 0x00000369
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002170 File Offset: 0x00000370
		public static CommandProxy AboutCmd { get; private set; }
	}
}
