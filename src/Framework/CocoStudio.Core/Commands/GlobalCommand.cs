using System;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.Commands
{
	public class GlobalCommand
	{
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

		public static CommandProxy NewCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.NewCmd, LanguageInfo.Menu_File_NewProject + "...", CmdGroupEnum.File, "Control+Shift+N", "Meta+Shift+N", ActionType.Normal);

		public static CommandProxy NewFileCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.NewFileCmd, LanguageInfo.NewFile_Title + "...", CmdGroupEnum.File, "Control+N", "Meta+N", ActionType.Normal);

		public static CommandProxy OpenCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.OpenCmd, LanguageInfo.Menu_File_OpenProject + "...", CmdGroupEnum.File, "Control+O", "Meta+O", ActionType.Normal);

		public static CommandArrayProxy RecentProjectCmd { get; private set; } = CommandCreater.CreateCommandArray(CmdEnum.RecentProjectCmd, ActionType.Normal);

		public static CommandProxy CloseCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.CloseCmd, LanguageInfo.Dialog_ButtonClose, CmdGroupEnum.File, null, null, ActionType.Normal);

		public static CommandProxy CloseProjectCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.CloseProjectCmd, LanguageInfo.Menu_File_CloseProject, CmdGroupEnum.File, null, null, ActionType.Normal);

		public static CommandProxy SaveCmd { get; private set; }

		public static CommandProxy SaveAllCmd { get; private set; }

		public static CommandProxy SaveAsCmd { get; private set; }

		public static CommandProxy ImportCmd { get; private set; }

		public static CommandProxy ImportFileCmd { get; private set; }

		public static CommandProxy ImportDirCmd { get; private set; }

		public static CommandProxy ImportProjectCmd { get; private set; }

		public static CommandProxy QuitCmd { get; private set; }

		public static CommandProxy UndoCmd { get; private set; }

		public static CommandProxy RedoCmd { get; private set; }

		public static CommandProxy PreferencesCmd { get; private set; }

		public static CommandProxy RunProjectCmd { get; private set; }

		public static CommandProxy RunLastCmd { get; private set; }

		public static CommandProxy PublishPackageCmd { get; private set; }

		public static CommandProxy PublishPackageLastCmd { get; private set; }

		public static CommandProxy ProjectSettingCmd { get; private set; }

		public static CommandProxy AnchorPointCmd { get; private set; }

		public static CommandProxy RulerCmd { get; private set; }

		public static CommandProxy ContentSizeCmd { get; private set; }

		public static CommandProxy ContentScaleCmd { get; private set; }

		public static CommandProxy GuidesCmd { get; private set; }

		public static CommandProxy LockGuidesCmd { get; private set; }

		public static CommandProxy ClearGuidesCmd { get; private set; }

		public static CommandProxy NewGuidesCmd { get; private set; }

		public static CommandProxy AbsorptionGuidesCmd { get; private set; }

		public static CommandProxy ResetLayoutCmd { get; private set; }

		public static CommandArrayProxy PadCmd { get; private set; }

		public static CommandProxy HelpCmd { get; private set; }

		public static CommandProxy AboutCmd { get; private set; }

		public static CommandProxy CheckUpdateCmd { get; private set; }

		public static CommandProxy StartLauncherCmd { get; private set; }

		public static CommandProxy SetChineseCmd { get; private set; }

		public static CommandProxy SetEnglishCmd { get; private set; }

		public static CommandProxy SetTraditionalChineseCmd { get; private set; }

		public static CommandProxy CopyCmd { get; private set; }

		public static CommandProxy CutCmd { get; private set; }

		public static CommandProxy PasteCmd { get; private set; }

		public static CommandProxy DeleteCmd { get; private set; }

		public static CommandProxy DeleteCmd2 { get; private set; }

		public static CommandProxy RefreshCmd { get; private set; }

		public static CommandProxy RenameCmd { get; private set; }

		public static CommandProxy SelectAllCmd { get; private set; }

		public static CommandProxy SkeletonUnBind { get; private set; }

		public static CommandProxy CloseAllCmd { get; private set; } = CommandCreater.CreateGlobalCommand(CmdEnum.CloseAllCmd, LanguageInfo.Command_CloseAll, CmdGroupEnum.File, null, null, ActionType.Normal);

		public static CommandProxy CloseOtherCmd { get; private set; }

		public static CommandProxy OpenDirCmd { get; private set; }

		public static CommandProxy NewFolderCmd { get; private set; }

		public static CommandProxy CreateSerialFrameCmd { get; private set; }

		public static CommandProxy ResourceOpenDirCmd { get; private set; }

		public static CommandProxy DuplicateCmd { get; private set; }

		public static CommandProxy MoveUpCmd { get; private set; }

		public static CommandProxy MoveDownCmd { get; private set; }

		public static CommandProxy MoveToTopCmd { get; private set; }

		public static CommandProxy MoveToBottomCmd { get; private set; }

		public static CommandProxy PlayCmd { get; private set; }

		public static CommandProxy TweenFrameCmd { get; private set; }

		public static CommandProxy CancelTweenCmd { get; private set; }

		public static CommandProxy AddFrameCmd { get; private set; }

		public static CommandProxy MoveToLeft { get; private set; }

		public static CommandProxy MoveToRight { get; private set; }

		public static CommandProxy MoveLeft { get; private set; }

		public static CommandProxy MoveRight { get; private set; }

		public static CommandProxy ResetCanvasCmd { get; private set; }

		public static CommandProxy ResetCanvasCmd2 { get; private set; }
	}
}
