using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using CocoStudio.Basic;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Ide.Codons;

namespace CocoStudio.Core.Commands
{
	// Token: 0x02000017 RID: 23
	public static class GlobalCommandHandle
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00004F3E File Offset: 0x0000313E
		public static void InitService()
		{
			GlobalCommandHandle.taskService = TaskServiceSingleton.Instance;
			GlobalCommandHandle.InitCmdBinding();
			GlobalCommandHandle.InitCmdHandleExtension();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004F58 File Offset: 0x00003158
		private static void InitCmdHandleExtension()
		{
			try
			{
				ICommandHandle[] extensionObjects = AddinManager.GetExtensionObjects<ICommandHandle>();
				if (extensionObjects != null)
				{
					foreach (ICommandHandle commandHandle in extensionObjects)
					{
						commandHandle.Initialize();
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("InitCmdHandleExtension failed.", exception);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004FCC File Offset: 0x000031CC
		private static void InitCmdBinding()
		{
			GlobalCommand.NewFileCmd.Update += GlobalCommandHandle.HasSolution_CanExecute;
			GlobalCommand.RecentProjectCmd.Execute += GlobalCommandHandle.RecentFileCmd_Execute;
			GlobalCommand.RecentProjectCmd.Update += GlobalCommandHandle.RecentFileCmd_Update;
			GlobalCommand.CloseCmd.Execute += GlobalCommandHandle.CloseCmd_Execute;
			GlobalCommand.CloseCmd.Update += GlobalCommandHandle.CloseCmd_CanExecute;
			GlobalCommand.CloseProjectCmd.Execute += GlobalCommandHandle.CloseProjectCmd_Execute;
			GlobalCommand.CloseProjectCmd.Update += GlobalCommandHandle.HasSolution_CanExecute;
			GlobalCommand.SaveCmd.Execute += GlobalCommandHandle.SaveCmd_Execute;
			GlobalCommand.SaveCmd.Update += GlobalCommandHandle.SaveCmd_CanExecute;
			GlobalCommand.SaveAllCmd.Execute += GlobalCommandHandle.SaveAllCmd_Execute;
			GlobalCommand.SaveAllCmd.Update += GlobalCommandHandle.HasSolution_CanExecute;
			GlobalCommand.SaveAsCmd.Execute += GlobalCommandHandle.SaveAsCmd_Execute;
			GlobalCommand.SaveAsCmd.Update += GlobalCommandHandle.HasSolution_CanExecute;
			GlobalCommand.ImportFileCmd.Update += GlobalCommandHandle.HasSolution_CanExecute;
			GlobalCommand.ImportDirCmd.Update += GlobalCommandHandle.HasSolution_CanExecute;
			GlobalCommand.ImportProjectCmd.Update += GlobalCommandHandle.HasSolution_CanExecute;
			GlobalCommand.QuitCmd.Execute += GlobalCommandHandle.QuitCmd_Execute;
			GlobalCommand.UndoCmd.Execute += GlobalCommandHandle.UndoCmd_Execute;
			GlobalCommand.UndoCmd.Update += GlobalCommandHandle.UndoCmd_CanExecute;
			GlobalCommand.RedoCmd.Execute += GlobalCommandHandle.RedoCmd_Execute;
			GlobalCommand.RedoCmd.Update += GlobalCommandHandle.RedoCmd_CanExecute;
			GlobalCommand.PadCmd.Execute += GlobalCommandHandle.PadCmd_Execute;
			GlobalCommand.PadCmd.Update += GlobalCommandHandle.PadCmd_Update;
			GlobalCommand.StartLauncherCmd.Execute += GlobalCommandHandle.StartLauncherCmd_Execute;
			GlobalCommand.ResetLayoutCmd.Execute += GlobalCommandHandle.ResetLayoutCmd_Execute;
			GlobalCommand.SetChineseCmd.Execute += GlobalCommandHandle.SetChineseCmd_Execute;
			GlobalCommand.SetEnglishCmd.Execute += GlobalCommandHandle.SetEnglishCmd_Execute;
			GlobalCommand.SetTraditionalChineseCmd.Execute += GlobalCommandHandle.SetTraditionalChineseCmd_Execute;
			GlobalCommand.SetChineseCmd.Update += GlobalCommandHandle.SetChineseCmd_Update;
			GlobalCommand.SetEnglishCmd.Update += GlobalCommandHandle.SetEnglishCmd_Update;
			GlobalCommand.SetTraditionalChineseCmd.Update += GlobalCommandHandle.SetTraditionalChineseCmd_Update;
			GlobalCommand.HelpCmd.Execute += GlobalCommandHandle.HelpCmd_Execute;
			GlobalCommand.AboutCmd.Execute += GlobalCommandHandle.AboutCmd_Execute;
			GlobalCommand.CloseAllCmd.Execute += GlobalCommandHandle.CloseAllCmd_Execute;
			GlobalCommand.CloseAllCmd.Update += GlobalCommandHandle.HasDocument_CanExecute;
			GlobalCommand.CloseOtherCmd.Execute += GlobalCommandHandle.CloseOtherCmd_Execute;
			GlobalCommand.CloseOtherCmd.Update += GlobalCommandHandle.HasDocument_CanExecute;
			GlobalCommand.OpenDirCmd.Execute += GlobalCommandHandle.OpenDirCmd_Execute;
			GlobalCommand.OpenDirCmd.Update += GlobalCommandHandle.HasDocument_CanExecute;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005344 File Offset: 0x00003544
		private static void HasSolution_CanExecute(object sender, CommandUpdateArgs e)
		{
			GlobalCommandHandle.PropertyPad_ReleaseFocus_BeforSaveCmd();
			if (Services.ProjectOperations.CurrentSelectedSolution != null)
			{
				e.Info.Enabled = true;
			}
			else
			{
				e.Info.Enabled = false;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005388 File Offset: 0x00003588
		private static void HasDocument_CanExecute(object sender, CommandUpdateArgs args)
		{
			GlobalCommandHandle.PropertyPad_ReleaseFocus_BeforSaveCmd();
			if (Services.ProjectOperations.CurrentSelectedSolution == null)
			{
				args.Info.Enabled = false;
			}
			else if (Services.Workbench.ActiveDocument == null)
			{
				args.Info.Enabled = false;
			}
			else
			{
				args.Info.Enabled = true;
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000053F8 File Offset: 0x000035F8
		private static void PropertyPad_ReleaseFocus_BeforSaveCmd()
		{
			Widget widget = Services.Workbench.Pads.PropertyPad.CurrentWidget();
			if (widget != null)
			{
				widget.CanFocus = true;
				widget.HasFocus = true;
				widget.HasFocus = false;
				widget.CanFocus = false;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005448 File Offset: 0x00003648
		private static void RecentFileCmd_Execute(object sender, CommandRunArgs args)
		{
			CocosItemModel cocosItemModel = args.DataItem as CocosItemModel;
			if (!File.Exists(cocosItemModel.LocalPath))
			{
				MessageBoxResult messageBoxResult = MessageBox.Show(string.Format(LanguageInfo.MessageBox266_ProjectNotFound, cocosItemModel.Name), MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
				if (messageBoxResult == MessageBoxResult.Yes)
				{
					Services.RecentFileService.RemoveCocosItem(cocosItemModel);
				}
			}
			else
			{
				GlobalCommand.OpenCmd.RaiseExecute(cocosItemModel.LocalPath);
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000054BC File Offset: 0x000036BC
		private static void RecentFileCmd_Update(object sender, CommandArrayUpdateArgs args)
		{
			ObservableCollection<CocosItemModel> observableCollection = new ObservableCollection<CocosItemModel>(Services.RecentFileService.CocosItemRecordList);
			foreach (CocosItemModel cocosItemModel in observableCollection)
			{
				MenuInfo info = new MenuInfo(cocosItemModel.LocalPath.Replace("_", "__"));
				args.Info.Add(info, cocosItemModel);
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005548 File Offset: 0x00003748
		private static void CloseCmd_Execute(object sender, CommandRunArgs e)
		{
			DocumentExtend activeDocument = Services.Workbench.ActiveDocument;
			activeDocument.Close();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005568 File Offset: 0x00003768
		private static void CloseCmd_CanExecute(object sender, CommandUpdateArgs args)
		{
			if (Services.ProjectOperations.CurrentSelectedSolution != null)
			{
				if (Services.Workbench.ActiveDocument != null)
				{
					string fileName = Path.GetFileName(Services.Workbench.ActiveDocument.Name);
					args.Info.Text = LanguageInfo.Dialog_ButtonClose + " " + fileName.Replace("_", "__");
					args.Info.Enabled = true;
					return;
				}
			}
			args.Info.Enabled = false;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000055F7 File Offset: 0x000037F7
		private static void CloseProjectCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.ProjectOperations.CloseSolution();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005605 File Offset: 0x00003805
		private static void SaveCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.Workbench.ActiveDocument.Save();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005618 File Offset: 0x00003818
		private static void SaveCmd_CanExecute(object sender, CommandUpdateArgs args)
		{
			GlobalCommandHandle.PropertyPad_ReleaseFocus_BeforSaveCmd();
			if (Services.ProjectOperations.CurrentSelectedSolution != null)
			{
				if (Services.Workbench.ActiveDocument != null)
				{
					string fileName = Path.GetFileName(Services.Workbench.ActiveDocument.Name);
					args.Info.Text = LanguageInfo.Command_Save + " " + fileName.Replace("_", "__");
					if (Services.Workbench.ActiveDocument.IsDirty)
					{
						args.Info.Enabled = true;
						return;
					}
				}
			}
			args.Info.Enabled = false;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000056C8 File Offset: 0x000038C8
		private static void SaveAllCmd_Execute(object sender, CommandRunArgs e)
		{
			IProgressMonitor consoleProgressMonitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
			Services.Workspace.Save(consoleProgressMonitor);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000056F0 File Offset: 0x000038F0
		private static void SaveAsCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.Workbench.SaveAll();
			Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
			FilePath itemDirectory = currentSelectedSolution.ItemDirectory;
			string[] fileTypes = new string[]
			{
				"*.ccs"
			};
			FilePath filePath = FileChooserDialogModel.GetSaveFilesPath(fileTypes, LanguageInfo.Dialog_SaveAs, false, currentSelectedSolution.FileName).FileName;
			FilePath filePath2 = filePath.ParentDirectory.Combine(new string[]
			{
				filePath.FileNameWithoutExtension
			});
			if (!Regex.IsMatch(filePath2, "^[\\\\*\\\\\\\\/:A-Za-z0-9,._-]+$"))
			{
				MessageBox.Show(LanguageInfo.MessageBox232_SlnDirLimit, MessageBoxImage.Other, null, null);
			}
			else if (Directory.Exists(filePath2))
			{
				MessageBox.Show(string.Format(LanguageInfo.MessageBox_Content172, filePath.FileNameWithoutExtension), MessageBoxImage.Other, null, null);
			}
			else if (filePath2.IsChildPathOf(currentSelectedSolution.BaseDirectory))
			{
				MessageBox.Show(LanguageInfo.MessageBox227_NoSolutionSubdirectory, MessageBoxImage.Other, null, null);
			}
			else
			{
				try
				{
					Directory.CreateDirectory(filePath2);
					FileService.CopyDirectory(currentSelectedSolution.BaseDirectory, filePath2);
					if (currentSelectedSolution.Name != filePath.FileNameWithoutExtension)
					{
						string oldName = filePath2.Combine(new string[]
						{
							currentSelectedSolution.FileName.FileName
						});
						string text = filePath2.Combine(new string[]
						{
							filePath.FileName
						});
						FileService.RenameFile(oldName, text);
						IProgressMonitor @default = Services.ProgressMonitors.Default;
						Services.ProjectsService.ChangeSolutionName(@default, text, currentSelectedSolution.FileName.FileNameWithoutExtension, filePath.FileNameWithoutExtension);
					}
					LogConfig.Output.Error(LanguageInfo.Output_SaveAsSucceed);
				}
				catch (Exception exception)
				{
					LogConfig.Output.Error(LanguageInfo.Output_FailedToSaveFile, exception);
				}
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005914 File Offset: 0x00003B14
		private static void QuitCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.MainWindow.Quit();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005922 File Offset: 0x00003B22
		private static void UndoCmd_Execute(object sender, CommandRunArgs e)
		{
			GlobalCommandHandle.taskService.Undo(Services.Workbench.ActiveDocument);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000593A File Offset: 0x00003B3A
		private static void UndoCmd_CanExecute(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = GlobalCommandHandle.taskService.CanUndo(Services.Workbench.ActiveDocument);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000595D File Offset: 0x00003B5D
		private static void RedoCmd_Execute(object sender, CommandRunArgs e)
		{
			GlobalCommandHandle.taskService.Redo(Services.Workbench.ActiveDocument);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005975 File Offset: 0x00003B75
		private static void RedoCmd_CanExecute(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = GlobalCommandHandle.taskService.CanRedo(Services.Workbench.ActiveDocument);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005998 File Offset: 0x00003B98
		private static void ResetLayoutCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.MainWindow.ResetDefaultLayout();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000059A8 File Offset: 0x00003BA8
		private static void PadCmd_Execute(object sender, CommandRunArgs args)
		{
			Pad pad = args.DataItem as Pad;
			PadCodon padCodon = pad.Content as PadCodon;
			if (!pad.Visible)
			{
				Services.MainWindow.ShowPad(padCodon);
				Services.MainWindow.BringToFront(padCodon);
				pad.AutoHide = false;
			}
			else
			{
				Services.MainWindow.HidePad(padCodon);
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005A0C File Offset: 0x00003C0C
		private static void PadCmd_Update(object sender, CommandArrayUpdateArgs args)
		{
			PadCollection pads = Services.Workbench.Pads;
			if (pads != null)
			{
				foreach (Pad pad in pads)
				{
					MenuInfo menuInfo = new MenuInfo(LanguageOption.GetValueBykey(pad.Title));
					menuInfo.Checked = pad.Visible;
					args.Info.Add(menuInfo, pad);
				}
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005AA0 File Offset: 0x00003CA0
		private static void StartLauncherCmd_Execute(object sender, CommandRunArgs e)
		{
			string fileName = string.Empty;
			if (Platform.IsWindows)
			{
				fileName = Path.Combine(Option.AssemblyDir, "Cocos.exe");
			}
			else if (Platform.IsMac)
			{
				fileName = "/Applications/Cocos/Cocos.app/Contents/MacOS/Cocos";
			}
			try
			{
				ProcessStartInfo startInfo = new ProcessStartInfo(fileName, "-page*3");
				Process.Start(startInfo);
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error(LanguageInfo.MessageBox226_LauncherStartFailed + "\r\n" + ex.ToString());
				MessageBox.Show(LanguageInfo.MessageBox226_LauncherStartFailed, MessageBoxImage.Other, null, null);
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005B44 File Offset: 0x00003D44
		private static void HelpCmd_Execute(object sender, CommandRunArgs e)
		{
			string urlFormat = "http://cocostudio.org/help/2.0/{0}";
			string localizedUrl = LanguageAdapter.GetLocalizedUrl(urlFormat);
			try
			{
				Uri uri = new Uri(localizedUrl, UriKind.RelativeOrAbsolute);
				Process.Start(uri.ToString());
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("打开帮助链接时出错", exception);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005BA0 File Offset: 0x00003DA0
		private static void SetChineseCmd_Execute(object sender, CommandRunArgs e)
		{
			LanguageOption.SetEditorLanguage(LanguageType.Chinese);
			GlobalCommandHandle.ShowMessageBox("语言设置成功，重启编辑器后生效", "提示", "确定");
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005BBF File Offset: 0x00003DBF
		private static void SetEnglishCmd_Execute(object sender, CommandRunArgs e)
		{
			LanguageOption.SetEditorLanguage(LanguageType.English);
			GlobalCommandHandle.ShowMessageBox("You have changed your language preferences. Restart Cocos Studio to apply the language changes.", "Info", "OK");
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005BDE File Offset: 0x00003DDE
		private static void SetTraditionalChineseCmd_Execute(object sender, CommandRunArgs e)
		{
			LanguageOption.SetEditorLanguage(LanguageType.Traditional);
			GlobalCommandHandle.ShowMessageBox("語言設置成功，重啟編輯器後生效", "提示", "確定");
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005C00 File Offset: 0x00003E00
		private static void ShowMessageBox(string info, string title, string buttonString)
		{
			ButtonText btnText = new ButtonText(buttonString, false);
			MessageBox.Show(info, btnText, MessageBoxImage.Info, null, EnumMainButton.Yes, title);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005C24 File Offset: 0x00003E24
		private static void SetEnglishCmd_Update(object sender, CommandUpdateArgs e)
		{
			if (LanguageOption.CurrentLanguage == LanguageType.English)
			{
				e.Info.Checked = true;
			}
			else
			{
				e.Info.Checked = false;
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005C60 File Offset: 0x00003E60
		private static void SetChineseCmd_Update(object sender, CommandUpdateArgs e)
		{
			if (LanguageOption.CurrentLanguage == LanguageType.Chinese)
			{
				e.Info.Checked = true;
			}
			else
			{
				e.Info.Checked = false;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005C9C File Offset: 0x00003E9C
		private static void SetTraditionalChineseCmd_Update(object sender, CommandUpdateArgs e)
		{
			if (LanguageOption.CurrentLanguage == LanguageType.Traditional)
			{
				e.Info.Checked = true;
			}
			else
			{
				e.Info.Checked = false;
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005CD8 File Offset: 0x00003ED8
		private static void AboutCmd_Execute(object sender, CommandRunArgs e)
		{
			CustomTitleWindow customTitleWindow = new CustomTitleWindow();
			customTitleWindow.InitView(LanguageInfo.Helper_About, new AboutContentWidget());
			customTitleWindow.Show();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005D04 File Offset: 0x00003F04
		private static void CloseAllCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.Workbench.CloseAll(false);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005D13 File Offset: 0x00003F13
		private static void CloseOtherCmd_Execute(object sender, CommandRunArgs e)
		{
			Services.Workbench.CloseAll(true);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005D24 File Offset: 0x00003F24
		private static void OpenDirCmd_Execute(object sender, CommandRunArgs e)
		{
			try
			{
				string fullPath = Services.Workbench.ActiveDocument.File.FullPath;
				if (Platform.IsWindows)
				{
					Process.Start("Explorer", "/select," + fullPath);
				}
				else
				{
					Process.Start("open", "-R " + string.Format("\"{0}\"", fullPath));
				}
			}
			catch
			{
				LogConfig.Output.Error(LanguageInfo.Output_FailedToOpenDir);
			}
		}

		// Token: 0x040000CF RID: 207
		private static IUndoManager taskService;
	}
}
