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
	public class StartRecoverService
	{
		public static string CocosFilePathByDoubleClick { get; set; }

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

		public void InitializeEvent()
		{
			UserStatisticsFactory.UnHandledExceptionEvent += StartRecoverService.UnHandledExceptionEvent;
			Services.ProjectOperations.CurrentSelectedSolutionClosing += StartRecoverService.ProjectOperations_CurrentSelectedSolutionClosing;
			Services.MainWindow.Closing += StartRecoverService.MainWindow_Closing;
			Services.Workspace.LoadWorkspaceItemSuccessedEvent += StartRecoverService.Workspace_LoadWorkspaceItemSuccessedEvent;
		}

		private static void Workspace_LoadWorkspaceItemSuccessedEvent(object sender, EventArgs e)
		{
			StartRecoverService.Instance.OpenLastDocuments();
			StartRecoverService.Instance.OpenDocumentByDoubleClick();
		}

		private static void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			string lastSolutonFilePath = (Services.ProjectOperations.CurrentSelectedSolution != null) ? Services.ProjectOperations.CurrentSelectedSolution.FileName.ToString() : null;
			StartRecoverService.Instance.SaveLastSolution(lastSolutonFilePath);
			StartRecoverService.Instance.SaveOpenedDocuments();
		}

		private static void ProjectOperations_CurrentSelectedSolutionClosing(object sender, SolutionEventArgs e)
		{
			StartRecoverService.Instance.SaveLastSolution(null);
			StartRecoverService.Instance.SaveOpenedDocuments();
		}

		private static void UnHandledExceptionEvent(EventArgs obj)
		{
			StartRecoverService.Instance.SaveLastSolution(null);
		}

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

		private void SaveLastSolution(string lastSolutonFilePath)
		{
			Services.RecentFileService.LastAvailableSolution = lastSolutonFilePath;
			Services.RecentFileService.SaveRecord();
		}

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

		private static StartRecoverService startRecoverService = null;
	}
}
