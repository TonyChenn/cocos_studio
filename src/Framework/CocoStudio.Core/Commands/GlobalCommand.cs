using System;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	// Token: 0x02000016 RID: 22
	public class GlobalCommand
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00003D48 File Offset: 0x00001F48
		static GlobalCommand()
		{
			GlobalCommand.SaveCmd = CommandCreater.CreateGlobalCommand(CmdEnum.SaveCmd, LanguageInfo.Menu_File_SaveProject, CmdGroupEnum.File, "Control+S", "Meta+S", ActionType.Normal);
			GlobalCommand.SaveAllCmd = CommandCreater.CreateGlobalCommand(CmdEnum.SaveAllCmd, LanguageInfo.Command_SaveAll, CmdGroupEnum.File, "Control+Shift+S", "Shift+Meta+S", ActionType.Normal);
			GlobalCommand.SaveAsCmd = CommandCreater.CreateGlobalCommand(CmdEnum.SaveAsCmd, LanguageInfo.Menu_File_SaveAs, CmdGroupEnum.File, null, null, ActionType.Normal);
			GlobalCommand.ImportCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ImportCmd, LanguageInfo.Menu_File_ImportResources + "...", CmdGroupEnum.File, null, null, ActionType.Normal);
			GlobalCommand.ImportFileCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ImportFileCmd, LanguageInfo.Menu_File_ImportFile, CmdGroupEnum.File, null, null, ActionType.Normal);
			GlobalCommand.ImportDirCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ImportDirCmd, LanguageInfo.Menu_File_ImportFolder, CmdGroupEnum.File, null, null, ActionType.Normal);
			GlobalCommand.ImportProjectCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ImportProjectCmd, LanguageInfo.Scene_Menu_File_ImportProject + "...", CmdGroupEnum.File, null, null, ActionType.Normal);
			GlobalCommand.QuitCmd = CommandCreater.CreateGlobalCommand(CmdEnum.QuitCmd, LanguageInfo.Menu_File_Exit, CmdGroupEnum.File, "Alt+F4", "Meta+Q", ActionType.Normal);
			GlobalCommand.UndoCmd = CommandCreater.CreateGlobalCommand(CmdEnum.UndoCmd, LanguageInfo.Menu_Edit_Undo, CmdGroupEnum.Edit, "Control+Z", "Meta+Z", ActionType.Normal);
			GlobalCommand.RedoCmd = CommandCreater.CreateGlobalCommand(CmdEnum.RedoCmd, LanguageInfo.Menu_Edit_Redo, CmdGroupEnum.Edit, "Control+Y", "Meta+Y", ActionType.Normal);
			GlobalCommand.ContentSizeCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ContentSizeCmd, LanguageInfo.Menu_Edit_DragChangeSize, CmdGroupEnum.Edit, "Shift+Control+K", "Shift+Meta+K", ActionType.Radio);
			GlobalCommand.ContentScaleCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ContentScaleCmd, LanguageInfo.Menu_Edit_DragChangeScale, CmdGroupEnum.Edit, "Shift+Control+L", "Shift+Meta+L", ActionType.Radio);
			GlobalCommand.PreferencesCmd = CommandCreater.CreateGlobalCommand(CmdEnum.PreferencesCmd, LanguageInfo.Menu_Edit_Preferences + "...", CmdGroupEnum.Edit, "Control+,", "Meta+,", ActionType.Normal);
			GlobalCommand.RunProjectCmd = CommandCreater.CreateGlobalCommand(CmdEnum.RunProjectCmd, LanguageInfo.Menu_Project_RunProject + "...", CmdGroupEnum.Project, null, null, ActionType.Normal);
			GlobalCommand.RunLastCmd = CommandCreater.CreateGlobalCommand(CmdEnum.RunLastCmd, LanguageInfo.Menu_Project_RunLast, CmdGroupEnum.Project, null, null, ActionType.Normal);
			GlobalCommand.PublishPackageCmd = CommandCreater.CreateGlobalCommand(CmdEnum.PublishPackageCmd, LanguageInfo.Menu_Project_PublishPackage + "...", CmdGroupEnum.Project, null, null, ActionType.Normal);
			GlobalCommand.PublishPackageLastCmd = CommandCreater.CreateGlobalCommand(CmdEnum.PublishPackageLastCmd, LanguageInfo.Menu_Project_PublishLast, CmdGroupEnum.Project, "Control+P", "Meta+P", ActionType.Normal);
			GlobalCommand.ProjectSettingCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ProjectSettingCmd, LanguageInfo.ProjSetting + "...", CmdGroupEnum.Project, null, null, ActionType.Normal);
			GlobalCommand.AnchorPointCmd = CommandCreater.CreateGlobalCommand(CmdEnum.AnchorPointCmd, LanguageInfo.Menu_View_VisibleAnchorPoint, CmdGroupEnum.View, "Shift+Control+A", "Shift+Meta+A", ActionType.Check);
			GlobalCommand.RulerCmd = CommandCreater.CreateGlobalCommand(CmdEnum.RulerCmd, LanguageInfo.Menu_View_VisibleRuler, CmdGroupEnum.View, "Shift+Control+R", "Shift+Meta+R", ActionType.Check);
			GlobalCommand.GuidesCmd = CommandCreater.CreateGlobalCommand(CmdEnum.GuidesCmd, LanguageInfo.Menu_View_Guides, CmdGroupEnum.View, "Control+;", "Meta+;", ActionType.Check);
			GlobalCommand.GuidesCmd = CommandCreater.CreateGlobalCommand(CmdEnum.GuidesCmd, LanguageInfo.Menu_View_Guides, CmdGroupEnum.View, "Control+;", "Meta+;", ActionType.Check);
			GlobalCommand.LockGuidesCmd = CommandCreater.CreateGlobalCommand(CmdEnum.LockGuidesCmd, LanguageInfo.Menu_View_LockGuides, CmdGroupEnum.View, "Alt+Control+;", "Alt+Meta+;", ActionType.Check);
			GlobalCommand.ClearGuidesCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ClearGuidesCmd, LanguageInfo.Menu_View_ClearGuides, CmdGroupEnum.View, null, null, ActionType.Normal);
			GlobalCommand.NewGuidesCmd = CommandCreater.CreateGlobalCommand(CmdEnum.NewGuidesCmd, LanguageInfo.Menu_View_NewGuides + "...", CmdGroupEnum.View, null, null, ActionType.Normal);
			GlobalCommand.AbsorptionGuidesCmd = CommandCreater.CreateGlobalCommand(CmdEnum.AbsorptionGuidesCmd, LanguageInfo.Menu_View_AbsorptionGuides, CmdGroupEnum.View, null, null, ActionType.Check);
			GlobalCommand.StartLauncherCmd = CommandCreater.CreateGlobalCommand(CmdEnum.StartLauncherCmd, LanguageInfo.Menu_Help_StartLauncher + "...", CmdGroupEnum.Window, null, null, ActionType.Normal);
			GlobalCommand.ResetLayoutCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ResetLayoutCmd, LanguageInfo.Menu_Window_Reset, CmdGroupEnum.Window, null, null, ActionType.Normal);
			GlobalCommand.PadCmd = CommandCreater.CreateCommandArray(CmdEnum.PadCmd, ActionType.Check);
			GlobalCommand.HelpCmd = CommandCreater.CreateGlobalCommand(CmdEnum.HelpCmd, LanguageInfo.Menu_Help_ShowHelp, CmdGroupEnum.Help, "F1", null, ActionType.Normal);
			GlobalCommand.AboutCmd = CommandCreater.CreateGlobalCommand(CmdEnum.AboutCmd, LanguageInfo.Menu_Help_About, CmdGroupEnum.NoHotkey, null, null, ActionType.Normal);
			GlobalCommand.CheckUpdateCmd = CommandCreater.CreateGlobalCommand(CmdEnum.CheckUpdateCmd, LanguageInfo.Menu_Help_CheckUpdate + "...", CmdGroupEnum.Help, null, null, ActionType.Normal);
			GlobalCommand.SetChineseCmd = CommandCreater.CreateGlobalCommand(CmdEnum.SetChineseCmd, "简体中文", CmdGroupEnum.Language, null, null, ActionType.Radio);
			GlobalCommand.SetEnglishCmd = CommandCreater.CreateGlobalCommand(CmdEnum.SetEnglishCmd, "English", CmdGroupEnum.Language, null, null, ActionType.Radio);
			GlobalCommand.SetTraditionalChineseCmd = CommandCreater.CreateGlobalCommand(CmdEnum.SetTraditionalChineseCmd, "繁體中文", CmdGroupEnum.Language, null, null, ActionType.Radio);
			GlobalCommand.CopyCmd = CommandCreater.CreateLocalCommand(CmdEnum.CopyCmd, LanguageInfo.Command_Copy, CmdGroupEnum.NoHotkey, "Control+C", "Meta+C", ActionType.Normal);
			GlobalCommand.CutCmd = CommandCreater.CreateLocalCommand(CmdEnum.CutCmd, LanguageInfo.UIAnimation_MenuText_CutFrame, CmdGroupEnum.NoHotkey, "Control+X", "Meta+X", ActionType.Normal);
			GlobalCommand.PasteCmd = CommandCreater.CreateLocalCommand(CmdEnum.PasteCmd, LanguageInfo.Command_Paste, CmdGroupEnum.NoHotkey, "Control+V", "Meta+V", ActionType.Normal);
			GlobalCommand.DeleteCmd = CommandCreater.CreateLocalCommand(CmdEnum.DeleteCmd, LanguageInfo.Command_Delete, CmdGroupEnum.NoHotkey, "Delete", "BackSpace", ActionType.Normal);
			GlobalCommand.DeleteCmd2 = CommandCreater.CreateLocalCommand(CmdEnum.DeleteCmd2, LanguageInfo.Command_Delete, CmdGroupEnum.NoHotkey, "BackSpace", "Delete", ActionType.Normal);
			GlobalCommand.RefreshCmd = CommandCreater.CreateLocalCommand(CmdEnum.RefreshCmd, LanguageInfo.Command_Refresh, CmdGroupEnum.NoHotkey, null, null, ActionType.Normal);
			GlobalCommand.RenameCmd = CommandCreater.CreateLocalCommand(CmdEnum.RenameCmd, LanguageInfo.Command_Rename, CmdGroupEnum.ResourcePanel, "F2", "Meta+R", ActionType.Normal);
			GlobalCommand.SelectAllCmd = CommandCreater.CreateLocalCommand(CmdEnum.SelectAllCmd, LanguageInfo.Dialog_ButtonSelectAll, CmdGroupEnum.NoHotkey, "Control+A", "Meta+A", ActionType.Normal);
			GlobalCommand.SkeletonUnBind = CommandCreater.CreateLocalCommand(CmdEnum.SkeletonUnBind, LanguageInfo.Skeleton_Unbinding, CmdGroupEnum.NoHotkey, "Alt+U", "Alt+U", ActionType.Normal);
			GlobalCommand.CloseOtherCmd = CommandCreater.CreateGlobalCommand(CmdEnum.CloseOtherCmd, LanguageInfo.Command_CloseOther, CmdGroupEnum.PageContextMenu, null, null, ActionType.Normal);
			GlobalCommand.OpenDirCmd = CommandCreater.CreateGlobalCommand(CmdEnum.OpenDirCmd, LanguageInfo.Command_OpenDirectory, CmdGroupEnum.ResourcePanel, null, null, ActionType.Normal);
			GlobalCommand.NewFolderCmd = CommandCreater.CreateLocalCommand(CmdEnum.NewFolderCmd, LanguageInfo.Command_NewFolder, CmdGroupEnum.ResourcePanel, null, null, ActionType.Normal);
			GlobalCommand.ResourceOpenDirCmd = CommandCreater.CreateLocalCommand(CmdEnum.ResOpenDirCmd, LanguageInfo.Command_OpenDirectory, CmdGroupEnum.ResourcePanel, null, null, ActionType.Normal);
			GlobalCommand.CreateSerialFrameCmd = CommandCreater.CreateLocalCommand(CmdEnum.CreateSerialFrameCmd, LanguageInfo.Resource_Menu_CreateSerialFrames + "...", CmdGroupEnum.ResourcePanel, null, null, ActionType.Normal);
			GlobalCommand.DuplicateCmd = CommandCreater.CreateLocalCommand(CmdEnum.DuplicateCmd, LanguageInfo.Resource_Menu_DuplicateFile, CmdGroupEnum.ResourcePanel, "Control+D", "Meta+D", ActionType.Normal);
			GlobalCommand.MoveUpCmd = CommandCreater.CreateLocalCommand(CmdEnum.MoveUpCmd, LanguageInfo.Animation_BoneTreetMenu_NodeMoveUp, CmdGroupEnum.RenderContextMenu, "Control+]", "Meta+]", ActionType.Normal);
			GlobalCommand.MoveDownCmd = CommandCreater.CreateLocalCommand(CmdEnum.MoveDownCmd, LanguageInfo.Animation_BoneTreetMenu_NodeMoveDown, CmdGroupEnum.RenderContextMenu, "Control+[", "Meta+[", ActionType.Normal);
			GlobalCommand.MoveToTopCmd = CommandCreater.CreateLocalCommand(CmdEnum.MoveToTopCmd, LanguageInfo.Animation_BoneTreetMenu_NodeMoveTop, CmdGroupEnum.RenderContextMenu, "Shift+Control+]", "Shift+Meta+]", ActionType.Normal);
			GlobalCommand.MoveToBottomCmd = CommandCreater.CreateLocalCommand(CmdEnum.MoveToBottomCmd, LanguageInfo.Animation_BoneTreetMenu_NodeMoveBottom, CmdGroupEnum.RenderContextMenu, "Shift+Control+[", "Shift+Meta+[", ActionType.Normal);
			GlobalCommand.TweenFrameCmd = CommandCreater.CreateLocalCommand(CmdEnum.TweenFrameCmd, LanguageInfo.Animation_LineControl_TweenFrame, CmdGroupEnum.AnimationPanel, null, null, ActionType.Normal);
			GlobalCommand.CancelTweenCmd = CommandCreater.CreateLocalCommand(CmdEnum.CancelTweenCmd, LanguageInfo.Animation_LineControl_CancelTweenFrame, CmdGroupEnum.AnimationPanel, null, null, ActionType.Normal);
			GlobalCommand.AddFrameCmd = CommandCreater.CreateLocalCommand(CmdEnum.AddFrameCmd, LanguageInfo.UIAnimation_MenuText_AddFrame, CmdGroupEnum.AnimationPanel, "K", null, ActionType.Normal);
			GlobalCommand.MoveToLeft = CommandCreater.CreateLocalCommand(CmdEnum.MoveToLeft, LanguageInfo.UIAnimation_ToolTip_FirstFrame, CmdGroupEnum.NoHotkey, "Control+Left", "Meta+Left", ActionType.Normal);
			GlobalCommand.MoveToRight = CommandCreater.CreateLocalCommand(CmdEnum.MoveToRight, LanguageInfo.UIAnimation_ToolTip_LastFrame, CmdGroupEnum.NoHotkey, "Control+Right", "Meta+Right", ActionType.Normal);
			GlobalCommand.MoveLeft = CommandCreater.CreateLocalCommand(CmdEnum.MoveLeft, LanguageInfo.UIAnimation_ToolTip_PreviousFrame, CmdGroupEnum.NoHotkey, "Left", null, ActionType.Normal);
			GlobalCommand.MoveRight = CommandCreater.CreateLocalCommand(CmdEnum.MoveRight, LanguageInfo.UIAnimation_ToolTip_NextFrame, CmdGroupEnum.NoHotkey, "Right", null, ActionType.Normal);
			GlobalCommand.PlayCmd = CommandCreater.CreateGlobalCommand(CmdEnum.PlayCmd, LanguageInfo.UIAnimation_ToolTip_Play, CmdGroupEnum.AnimationPanel, "F5", null, ActionType.Normal);
			GlobalCommand.ResetCanvasCmd = CommandCreater.CreateGlobalCommand(CmdEnum.ResetCanvasCmd, LanguageInfo.StatusSlider_btn_Reset, CmdGroupEnum.Status, "Control+0", "Meta+0", ActionType.Normal);
			GlobalCommand.ResetCanvasCmd2 = CommandCreater.CreateGlobalCommand(CmdEnum.ResetCanvasCmd2, LanguageInfo.StatusSlider_btn_Reset, CmdGroupEnum.Status, "Control+KP_0", "Meta+KP_0", ActionType.Normal);
			GlobalCommandHandle.InitService();
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00004678 File Offset: 0x00002878
		// (set) Token: 0x06000085 RID: 133 RVA: 0x0000468E File Offset: 0x0000288E
		public static CommandProxy NewCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.NewCmd, LanguageInfo.Menu_File_NewProject + "...", CmdGroupEnum.File, "Control+Shift+N", "Meta+Shift+N", ActionType.Normal);

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00004698 File Offset: 0x00002898
		// (set) Token: 0x06000087 RID: 135 RVA: 0x000046AE File Offset: 0x000028AE
		public static CommandProxy NewFileCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.NewFileCmd, LanguageInfo.NewFile_Title + "...", CmdGroupEnum.File, "Control+N", "Meta+N", ActionType.Normal);

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000046B8 File Offset: 0x000028B8
		// (set) Token: 0x06000089 RID: 137 RVA: 0x000046CE File Offset: 0x000028CE
		public static CommandProxy OpenCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.OpenCmd, LanguageInfo.Menu_File_OpenProject + "...", CmdGroupEnum.File, "Control+O", "Meta+O", ActionType.Normal);

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000046D8 File Offset: 0x000028D8
		// (set) Token: 0x0600008B RID: 139 RVA: 0x000046EE File Offset: 0x000028EE
		public static CommandArrayProxy RecentProjectCmd { get; private set; } = CommandCreater.CreateCommandArray(CmdEnum.RecentProjectCmd, ActionType.Normal);

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000046F8 File Offset: 0x000028F8
		// (set) Token: 0x0600008D RID: 141 RVA: 0x0000470E File Offset: 0x0000290E
		public static CommandProxy CloseCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.CloseCmd, LanguageInfo.Dialog_ButtonClose, CmdGroupEnum.File, null, null, ActionType.Normal);

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004718 File Offset: 0x00002918
		// (set) Token: 0x0600008F RID: 143 RVA: 0x0000472E File Offset: 0x0000292E
		public static CommandProxy CloseProjectCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.CloseProjectCmd, LanguageInfo.Menu_File_CloseProject, CmdGroupEnum.File, null, null, ActionType.Normal);

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004738 File Offset: 0x00002938
		// (set) Token: 0x06000091 RID: 145 RVA: 0x0000474E File Offset: 0x0000294E
		public static CommandProxy SaveCmd { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004758 File Offset: 0x00002958
		// (set) Token: 0x06000093 RID: 147 RVA: 0x0000476E File Offset: 0x0000296E
		public static CommandProxy SaveAllCmd { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004778 File Offset: 0x00002978
		// (set) Token: 0x06000095 RID: 149 RVA: 0x0000478E File Offset: 0x0000298E
		public static CommandProxy SaveAsCmd { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004798 File Offset: 0x00002998
		// (set) Token: 0x06000097 RID: 151 RVA: 0x000047AE File Offset: 0x000029AE
		public static CommandProxy ImportCmd { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000047B8 File Offset: 0x000029B8
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000047CE File Offset: 0x000029CE
		public static CommandProxy ImportFileCmd { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000047D8 File Offset: 0x000029D8
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000047EE File Offset: 0x000029EE
		public static CommandProxy ImportDirCmd { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000047F8 File Offset: 0x000029F8
		// (set) Token: 0x0600009D RID: 157 RVA: 0x0000480E File Offset: 0x00002A0E
		public static CommandProxy ImportProjectCmd { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004818 File Offset: 0x00002A18
		// (set) Token: 0x0600009F RID: 159 RVA: 0x0000482E File Offset: 0x00002A2E
		public static CommandProxy QuitCmd { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004838 File Offset: 0x00002A38
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x0000484E File Offset: 0x00002A4E
		public static CommandProxy UndoCmd { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004858 File Offset: 0x00002A58
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x0000486E File Offset: 0x00002A6E
		public static CommandProxy RedoCmd { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004878 File Offset: 0x00002A78
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000488E File Offset: 0x00002A8E
		public static CommandProxy PreferencesCmd { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004898 File Offset: 0x00002A98
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x000048AE File Offset: 0x00002AAE
		public static CommandProxy RunProjectCmd { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000048B8 File Offset: 0x00002AB8
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000048CE File Offset: 0x00002ACE
		public static CommandProxy RunLastCmd { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000048D8 File Offset: 0x00002AD8
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000048EE File Offset: 0x00002AEE
		public static CommandProxy PublishPackageCmd { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000048F8 File Offset: 0x00002AF8
		// (set) Token: 0x060000AD RID: 173 RVA: 0x0000490E File Offset: 0x00002B0E
		public static CommandProxy PublishPackageLastCmd { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004918 File Offset: 0x00002B18
		// (set) Token: 0x060000AF RID: 175 RVA: 0x0000492E File Offset: 0x00002B2E
		public static CommandProxy ProjectSettingCmd { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004938 File Offset: 0x00002B38
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x0000494E File Offset: 0x00002B4E
		public static CommandProxy AnchorPointCmd { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004958 File Offset: 0x00002B58
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x0000496E File Offset: 0x00002B6E
		public static CommandProxy RulerCmd { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004978 File Offset: 0x00002B78
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x0000498E File Offset: 0x00002B8E
		public static CommandProxy ContentSizeCmd { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004998 File Offset: 0x00002B98
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x000049AE File Offset: 0x00002BAE
		public static CommandProxy ContentScaleCmd { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000049B8 File Offset: 0x00002BB8
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x000049CE File Offset: 0x00002BCE
		public static CommandProxy GuidesCmd { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000049D8 File Offset: 0x00002BD8
		// (set) Token: 0x060000BB RID: 187 RVA: 0x000049EE File Offset: 0x00002BEE
		public static CommandProxy LockGuidesCmd { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000049F8 File Offset: 0x00002BF8
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00004A0E File Offset: 0x00002C0E
		public static CommandProxy ClearGuidesCmd { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004A18 File Offset: 0x00002C18
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00004A2E File Offset: 0x00002C2E
		public static CommandProxy NewGuidesCmd { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004A38 File Offset: 0x00002C38
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00004A4E File Offset: 0x00002C4E
		public static CommandProxy AbsorptionGuidesCmd { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004A58 File Offset: 0x00002C58
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00004A6E File Offset: 0x00002C6E
		public static CommandProxy ResetLayoutCmd { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004A78 File Offset: 0x00002C78
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004A8E File Offset: 0x00002C8E
		public static CommandArrayProxy PadCmd { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004A98 File Offset: 0x00002C98
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004AAE File Offset: 0x00002CAE
		public static CommandProxy HelpCmd { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004AB8 File Offset: 0x00002CB8
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004ACE File Offset: 0x00002CCE
		public static CommandProxy AboutCmd { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004AD8 File Offset: 0x00002CD8
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004AEE File Offset: 0x00002CEE
		public static CommandProxy CheckUpdateCmd { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004AF8 File Offset: 0x00002CF8
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004B0E File Offset: 0x00002D0E
		public static CommandProxy StartLauncherCmd { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004B18 File Offset: 0x00002D18
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00004B2E File Offset: 0x00002D2E
		public static CommandProxy SetChineseCmd { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004B38 File Offset: 0x00002D38
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004B4E File Offset: 0x00002D4E
		public static CommandProxy SetEnglishCmd { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004B58 File Offset: 0x00002D58
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00004B6E File Offset: 0x00002D6E
		public static CommandProxy SetTraditionalChineseCmd { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004B78 File Offset: 0x00002D78
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00004B8E File Offset: 0x00002D8E
		public static CommandProxy CopyCmd { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004B98 File Offset: 0x00002D98
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00004BAE File Offset: 0x00002DAE
		public static CommandProxy CutCmd { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004BB8 File Offset: 0x00002DB8
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00004BCE File Offset: 0x00002DCE
		public static CommandProxy PasteCmd { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004BD8 File Offset: 0x00002DD8
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00004BEE File Offset: 0x00002DEE
		public static CommandProxy DeleteCmd { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00004BF8 File Offset: 0x00002DF8
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00004C0E File Offset: 0x00002E0E
		public static CommandProxy DeleteCmd2 { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00004C18 File Offset: 0x00002E18
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00004C2E File Offset: 0x00002E2E
		public static CommandProxy RefreshCmd { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004C38 File Offset: 0x00002E38
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00004C4E File Offset: 0x00002E4E
		public static CommandProxy RenameCmd { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004C58 File Offset: 0x00002E58
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00004C6E File Offset: 0x00002E6E
		public static CommandProxy SelectAllCmd { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004C78 File Offset: 0x00002E78
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00004C8E File Offset: 0x00002E8E
		public static CommandProxy SkeletonUnBind { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004C98 File Offset: 0x00002E98
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004CAE File Offset: 0x00002EAE
		public static CommandProxy CloseAllCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.CloseAllCmd, LanguageInfo.Command_CloseAll, CmdGroupEnum.File, null, null, ActionType.Normal);

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004CB8 File Offset: 0x00002EB8
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00004CCE File Offset: 0x00002ECE
		public static CommandProxy CloseOtherCmd { get; private set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004CD8 File Offset: 0x00002ED8
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004CEE File Offset: 0x00002EEE
		public static CommandProxy OpenDirCmd { get; private set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004CF8 File Offset: 0x00002EF8
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00004D0E File Offset: 0x00002F0E
		public static CommandProxy NewFolderCmd { get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00004D18 File Offset: 0x00002F18
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00004D2E File Offset: 0x00002F2E
		public static CommandProxy CreateSerialFrameCmd { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004D38 File Offset: 0x00002F38
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004D4E File Offset: 0x00002F4E
		public static CommandProxy ResourceOpenDirCmd { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004D58 File Offset: 0x00002F58
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004D6E File Offset: 0x00002F6E
		public static CommandProxy DuplicateCmd { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004D78 File Offset: 0x00002F78
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004D8E File Offset: 0x00002F8E
		public static CommandProxy MoveUpCmd { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004D98 File Offset: 0x00002F98
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00004DAE File Offset: 0x00002FAE
		public static CommandProxy MoveDownCmd { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004DB8 File Offset: 0x00002FB8
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00004DCE File Offset: 0x00002FCE
		public static CommandProxy MoveToTopCmd { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004DD8 File Offset: 0x00002FD8
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00004DEE File Offset: 0x00002FEE
		public static CommandProxy MoveToBottomCmd { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004DF8 File Offset: 0x00002FF8
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00004E0E File Offset: 0x0000300E
		public static CommandProxy PlayCmd { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00004E18 File Offset: 0x00003018
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00004E2E File Offset: 0x0000302E
		public static CommandProxy TweenFrameCmd { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004E38 File Offset: 0x00003038
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00004E4E File Offset: 0x0000304E
		public static CommandProxy CancelTweenCmd { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004E58 File Offset: 0x00003058
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00004E6E File Offset: 0x0000306E
		public static CommandProxy AddFrameCmd { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004E78 File Offset: 0x00003078
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00004E8E File Offset: 0x0000308E
		public static CommandProxy MoveToLeft { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00004E98 File Offset: 0x00003098
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00004EAE File Offset: 0x000030AE
		public static CommandProxy MoveToRight { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00004EB8 File Offset: 0x000030B8
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00004ECE File Offset: 0x000030CE
		public static CommandProxy MoveLeft { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00004ED8 File Offset: 0x000030D8
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00004EEE File Offset: 0x000030EE
		public static CommandProxy MoveRight { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00004EF8 File Offset: 0x000030F8
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00004F0E File Offset: 0x0000310E
		public static CommandProxy ResetCanvasCmd { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00004F18 File Offset: 0x00003118
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00004F2E File Offset: 0x0000312E
		public static CommandProxy ResetCanvasCmd2 { get; private set; }
	}
}
