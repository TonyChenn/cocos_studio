using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Projects;
using CocoStudio.UserStatistics;

namespace Modules.Communal.StartAutoRecover
{
	// Token: 0x02000002 RID: 2
	public class StartRecoverService
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002066 File Offset: 0x00000266
		public static string CocosFilePathByDoubleClick { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
		public static StartRecoverService Instance
		{
			get
			{
				if (StartRecoverService.startRecoverService == null)
				{
					StartRecoverService.startRecoverService = new StartRecoverService();
				}
				return StartRecoverService.startRecoverService;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020A0 File Offset: 0x000002A0
		public void InitializeEvent()
		{
			UserStatisticsFactory.UnHandledExceptionEvent += StartRecoverService.UnHandledExceptionEvent;
			Services.ProjectOperations.CurrentSelectedSolutionClosing += StartRecoverService.ProjectOperations_CurrentSelectedSolutionClosing;
			Services.MainWindow.Closing += StartRecoverService.MainWindow_Closing;
			Services.Workspace.LoadWorkspaceItemSuccessedEvent += StartRecoverService.Workspace_LoadWorkspaceItemSuccessedEvent;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002105 File Offset: 0x00000305
		private static void Workspace_LoadWorkspaceItemSuccessedEvent(object sender, EventArgs e)
		{
			StartRecoverService.Instance.OpenLastDocuments();
			StartRecoverService.Instance.OpenDocumentByDoubleClick();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002120 File Offset: 0x00000320
		private static void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			string lastSolutonFilePath = (Services.ProjectOperations.CurrentSelectedSolution != null) ? Services.ProjectOperations.CurrentSelectedSolution.FileName.ToString() : null;
			StartRecoverService.Instance.SaveLastSolution(lastSolutonFilePath);
			StartRecoverService.Instance.SaveOpenedDocuments();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002173 File Offset: 0x00000373
		private static void ProjectOperations_CurrentSelectedSolutionClosing(object sender, SolutionEventArgs e)
		{
			StartRecoverService.Instance.SaveLastSolution(null);
			StartRecoverService.Instance.SaveOpenedDocuments();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000218D File Offset: 0x0000038D
		private static void UnHandledExceptionEvent(EventArgs obj)
		{
			StartRecoverService.Instance.SaveLastSolution(null);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000219C File Offset: 0x0000039C
		public string LoadLastSolutionArgs()
		{
			string lastAvailableSolution = Services.RecentFileService.LastAvailableSolution;
			this.SaveLastSolution(null);
			string result;
			if (string.IsNullOrEmpty(lastAvailableSolution))
			{
				result = null;
			}
			else if (!File.Exists(lastAvailableSolution))
			{
				result = null;
			}
			else
			{
				result = lastAvailableSolution;
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021E2 File Offset: 0x000003E2
		private void SaveLastSolution(string lastSolutonFilePath)
		{
			Services.RecentFileService.LastAvailableSolution = lastSolutonFilePath;
			Services.RecentFileService.SaveRecord();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021FC File Offset: 0x000003FC
		private void SaveOpenedDocuments()
		{
			try
			{
				if (Services.ProjectOperations.CurrentSelectedSolution != null)
				{
					List<DocumentExtend> list = new List<DocumentExtend>(Services.Workbench.Documents);
					TabsInfo tabsInfo = new TabsInfo();
					CocosItem resourceFile = (Services.Workbench.ActiveDocument != null) ? Services.Workbench.ActiveDocument.File : null;
					tabsInfo.ActiveDocument = new FilePathData(resourceFile);
					tabsInfo.OpenedDocuments = new List<FilePathData>();
					if (list != null)
					{
						foreach (DocumentExtend documentExtend in list)
						{
							tabsInfo.OpenedDocuments.Add(new FilePathData(documentExtend.File));
						}
					}
					UserData userData = Services.ProjectOperations.CurrentSelectedSolution.UserData;
					userData.Properties["TabsParamsKey"] = tabsInfo;
					Services.ProjectOperations.CurrentSelectedSolution.UserData.Save();
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Exception while saving infomation opened documents! ", exception);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002340 File Offset: 0x00000540
		private void OpenLastDocuments()
		{
			try
			{
				if (Services.ProjectOperations.CurrentSelectedSolution != null)
				{
					UserData userData = Services.ProjectOperations.CurrentSelectedSolution.UserData;
					if (userData != null && userData.Properties != null)
					{
						if (userData.Properties.ContainsKey("TabsParamsKey"))
						{
							TabsInfo tabsInfo = userData.Properties["TabsParamsKey"] as TabsInfo;
							if (tabsInfo != null && tabsInfo.ActiveDocument != null && tabsInfo.OpenedDocuments != null)
							{
								userData.Properties.Remove("TabsParamsKey");
								Services.ProjectOperations.CurrentSelectedSolution.UserData.Save();
								CocosItem cocosItem = tabsInfo.ActiveDocument.File as CocosItem;
								if (cocosItem != null)
								{
									foreach (FilePathData filePathData in tabsInfo.OpenedDocuments)
									{
										if (filePathData != null)
										{
											CocosItem cocosItem2 = filePathData.File as CocosItem;
											if (cocosItem2 != null)
											{
												bool bringToFront = false;
												if (cocosItem.FileName == cocosItem2.FileName)
												{
													bringToFront = true;
												}
												Services.Workbench.OpenDocument(cocosItem2.FileName, cocosItem2, bringToFront);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Exception while open last documents! ", exception);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002528 File Offset: 0x00000728
		private void OpenDocumentByDoubleClick()
		{
			try
			{
				if (!string.IsNullOrEmpty(StartRecoverService.CocosFilePathByDoubleClick))
				{
					CocosItem cocosItem = Services.ProjectOperations.CurrentResourceGroup.FindResourceItem(StartRecoverService.CocosFilePathByDoubleClick) as CocosItem;
					if (cocosItem != null && cocosItem.FileName != null)
					{
						Services.Workbench.OpenDocument(cocosItem.FileName, cocosItem, true);
					}
				}
				StartRecoverService.CocosFilePathByDoubleClick = string.Empty;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Exception while open the document which is started by double click! ", exception);
			}
		}

		// Token: 0x04000001 RID: 1
		private static StartRecoverService startRecoverService = null;
	}
}
