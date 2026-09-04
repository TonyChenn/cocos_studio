using System;
using System.IO;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Projects;
using CocoStudio.UserStatistics;
using GLib;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.CocosAdapter.Platform;
using Modules.Communal.CocosCodeIDE;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Output;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.Publish
{
	// Token: 0x02000006 RID: 6
	[Extension(Type = typeof(ICommandHandle))]
	public class PublishUC : ICommandHandle
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002B94 File Offset: 0x00000D94
		void ICommandHandle.Initialize()
		{
			GlobalCommand.PublishPackageCmd.Execute += this.PublishPackageCmd_Execute;
			GlobalCommand.PublishPackageCmd.Update += this.HasSolution_CanExecute;
			GlobalCommand.PublishPackageLastCmd.Execute += this.PublishPackageLastCmd_Execute;
			GlobalCommand.PublishPackageLastCmd.Update += this.PublishPackageLastCmd_Update;
			GlobalCommand.RunProjectCmd.Execute += this.RunProjectCmd_ExecuteHandler;
			GlobalCommand.RunProjectCmd.Update += this.HasSolution_CanExecute;
			GlobalCommand.RunLastCmd.Execute += this.RunLastCmd_ExecuteHandler;
			GlobalCommand.RunLastCmd.Update += this.RunLastCmd_UpdateHandler;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002C54 File Offset: 0x00000E54
		private void PublishPackageCmd_Execute(object sender, CommandRunArgs e)
		{
			PublishPackageWindow publishPackageWindow = new PublishPackageWindow();
			publishPackageWindow.Show();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002C6D File Offset: 0x00000E6D
		private void PublishPackageLastCmd_Execute(object sender, CommandRunArgs e)
		{
			if (CocosRecentServices.Instance.IsLastPublish)
			{
				this.PublishUsingLastSettings();
				return;
			}
			this.PackageUsingLastSettings();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002C88 File Offset: 0x00000E88
		private void PublishPackageLastCmd_Update(object sender, CommandUpdateArgs e)
		{
			bool flag = PublishHelper.HasSolution();
			e.Info.Enabled = flag;
			if (flag)
			{
				if (CocosRecentServices.Instance.IsLastPublish)
				{
					e.Info.Text = LanguageInfo.Menu_Project_PublishLast;
					return;
				}
				e.Info.Text = LanguageInfo.Menu_Project_PackageLast;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002CD8 File Offset: 0x00000ED8
		private void RunProjectCmd_ExecuteHandler(object sender, CommandRunArgs e)
		{
			EnumPlatform enumPlatform;
			if (this.SelectRunType(out enumPlatform))
			{
				CocosRecentServices.Instance.LastRunType = enumPlatform;
				this.Run(enumPlatform);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002D01 File Offset: 0x00000F01
		private void RunLastCmd_ExecuteHandler(object sender, CommandRunArgs e)
		{
			this.Run(CocosRecentServices.Instance.LastRunType);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002D14 File Offset: 0x00000F14
		private void RunLastCmd_UpdateHandler(object sender, CommandUpdateArgs e)
		{
			EnumPlatform lastRunType = CocosRecentServices.Instance.LastRunType;
			IPlatform platform = null;
			foreach (IPlatform platform2 in Cocos2dxServices.PlatformServices.PlatformList)
			{
				if (platform2.PlatformType == lastRunType)
				{
					platform = platform2;
					break;
				}
			}
			if (platform == null || !platform.CanShow(EnumOperationType.Run))
			{
				e.Info.Enabled = false;
				return;
			}
			e.Info.Enabled = true;
			e.Info.Text = platform.GetDisplayName(EnumOperationType.Run);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002DB0 File Offset: 0x00000FB0
		private void HasSolution_CanExecute(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = PublishHelper.HasSolution();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002DC4 File Offset: 0x00000FC4
		private void PackageUsingLastSettings()
		{
			if (!Cocos2dxServices.SupplymentServices.Supplyment(EnumSolutionCodeType.Complete, EnumOperationType.Package))
			{
				return;
			}
			PackageParams prms = PackageServices.Instance.PackageParams;
			if (Cocos2dxServices.PlatformServices.GetPackagePlatformCount(prms) == 0)
			{
				GlobalCommand.PublishPackageCmd.RaiseExecute(null);
				return;
			}
			if (!Cocos2dxServices.PlatformServices.CheckCanPackage(prms))
			{
				return;
			}
			if (!PublishUC.PublishSolution())
			{
				return;
			}
			CocosMonitor monitor = new CocosMonitor(true);
			monitor.Finished += this.PackageFinishedHandler;
			ProcessWindow processWindow = new ProcessWindow(LanguageInfo.Dialog_PackageProject, true, true, true);
			processWindow.StartRunning(monitor);
			Task task = new Task(delegate()
			{
				Cocos2dxServices.PlatformServices.StartPackage(prms, monitor);
			});
			task.Start();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002E84 File Offset: 0x00001084
		private void PackageFinishedHandler(object sender, FinishedArgs e)
		{
			CocosMonitor cocosMonitor = sender as CocosMonitor;
			cocosMonitor.Finished -= this.PackageFinishedHandler;
			if (cocosMonitor.IsCancelled)
			{
				LogConfig.Output.Info(LanguageInfo.Output_PackageCanceled, true);
				return;
			}
			if (cocosMonitor.IsSuccessed)
			{
				LogConfig.Output.Info(LanguageInfo.Output_ProjectPackaged, true);
				return;
			}
			LogConfig.Output.Info(LanguageInfo.Output_FailedToPackage, true);
			LogConfig.OutputWithoutTip.Info(cocosMonitor.FullOutputInfo, true);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002F00 File Offset: 0x00001100
		private void PublishUsingLastSettings()
		{
			switch (CocosRecentServices.Instance.LastPublishType)
			{
			case EnumPublishType.Resource:
				this.PublishResource();
				return;
			case EnumPublishType.CodeIDE:
				this.PublishToCodeIDE();
				return;
			case EnumPublishType.VS:
				this.PublishToVisualStudio();
				return;
			case EnumPublishType.XCode:
				this.PublishToXcode();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002F4A File Offset: 0x0000114A
		private void PublishResource()
		{
			PublishUC.PublishSolution();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002F54 File Offset: 0x00001154
		private void PublishToCodeIDE()
		{
			if (!Cocos2dxServices.SupplymentServices.Supplyment(EnumSolutionCodeType.CodeIDE, EnumOperationType.Publish))
			{
				return;
			}
			bool flag = PublishUC.PublishSolution();
			if (flag)
			{
				string projectDir = Services.ProjectOperations.CurrentSelectedSolution.BaseDirectory;
				CocosCodeIDEService.Instance.OpenCocosItemWithCocosCodeIDE(projectDir, true);
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002F9B File Offset: 0x0000119B
		private void PublishToVisualStudio()
		{
			if (!Cocos2dxServices.SupplymentServices.Supplyment(EnumSolutionCodeType.Complete, EnumOperationType.Publish))
			{
				return;
			}
			if (!PublishUC.PublishSolution())
			{
				return;
			}
			PublishHelper.OpenProjectWithVisualStudio();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002FB9 File Offset: 0x000011B9
		private void PublishToXcode()
		{
			if (!Cocos2dxServices.SupplymentServices.Supplyment(EnumSolutionCodeType.Complete, EnumOperationType.Publish))
			{
				return;
			}
			if (!PublishUC.PublishSolution())
			{
				return;
			}
			PublishHelper.OpenProjectWithXcode();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002FD7 File Offset: 0x000011D7
		private static bool PublishSolution()
		{
			Services.Workbench.SaveAll();
			return PublishUC.PublishSolutionSync();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002FE8 File Offset: 0x000011E8
		private static void PublishSolutionAsync(CocosMonitor cocosMonitor)
		{
			string methodName = "";
			try
			{
				Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
				if (currentSelectedSolution == null)
				{
					cocosMonitor.Finish(false);
				}
				else
				{
					FilePath filePath = currentSelectedSolution.PublishDirectory;
					if (!Directory.Exists(filePath))
					{
						try
						{
							Directory.CreateDirectory(filePath);
						}
						catch
						{
							MessageBoxResult messageBoxResult = MessageBox.Show(string.Format(LanguageInfo.MessageBox229_CanNotPublish, filePath), MessageBoxButton.YesNo, MessageBoxImage.Error, null, EnumMainButton.Yes, null);
							if (messageBoxResult == MessageBoxResult.Yes)
							{
								GlobalCommand.ProjectSettingCmd.RaiseExecute(null);
							}
							cocosMonitor.Finish(false);
							return;
						}
					}
					IProgressMonitor consoleProgressMonitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
					PublishType publishType = Services.ProjectsService.CurrentSolution.Config.PublishType;
					methodName = "Publish" + publishType + "Failed";
					PublishInfo info = new PublishInfo(filePath, publishType);
					Services.ProjectOperations.Publish(consoleProgressMonitor, info);
					if (consoleProgressMonitor.AsyncOperation.Success)
					{
						methodName = "Publish" + publishType;
						LogConfig.Output.Info(LanguageInfo.Dialog_Publish_Success, true);
						cocosMonitor.Finish(true);
					}
					else
					{
						LogConfig.Output.Error(LanguageInfo.Dialog_Publish_Failed);
						Services.Workbench.Pads.OutputPad.BringToFront();
						cocosMonitor.Finish(false);
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error(LanguageInfo.Dialog_Publish_Failed, exception);
				GLib.Timeout.Add(0U, delegate
				{
					Services.Workbench.Pads.OutputPad.BringToFront();
					return false;
				});
				cocosMonitor.Finish(false);
			}
			finally
			{
				Tracker.Add(ViewRegions.None, "Publish", methodName, "");
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000031D4 File Offset: 0x000013D4
		private static bool PublishSolutionSync()
		{
			string methodName = "";
			bool result;
			try
			{
				Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
				if (currentSelectedSolution == null)
				{
					result = false;
				}
				else
				{
					FilePath filePath = currentSelectedSolution.PublishDirectory;
					if (!Directory.Exists(filePath))
					{
						try
						{
							Directory.CreateDirectory(filePath);
						}
						catch
						{
							MessageBoxResult messageBoxResult = MessageBox.Show(string.Format(LanguageInfo.MessageBox229_CanNotPublish, filePath), MessageBoxButton.YesNo, MessageBoxImage.Error, null, EnumMainButton.Yes, null);
							if (messageBoxResult == MessageBoxResult.Yes)
							{
								GlobalCommand.ProjectSettingCmd.RaiseExecute(null);
							}
							return false;
						}
					}
					IOutputPad service = Services.GetService<IOutputPad>();
					if (service != null)
					{
						service.Clear();
					}
					IProgressMonitor consoleProgressMonitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
					PublishType publishType = Services.ProjectsService.CurrentSolution.Config.PublishType;
					methodName = "Publish" + publishType + "Failed";
					PublishInfo info = new PublishInfo(filePath, publishType);
					Services.ProjectOperations.Publish(consoleProgressMonitor, info);
					if (consoleProgressMonitor.AsyncOperation.Success)
					{
						methodName = "Publish" + publishType;
						if (consoleProgressMonitor.AsyncOperation.SuccessWithWarnings)
						{
							Services.Workbench.Pads.OutputPad.BringToFront();
						}
						LogConfig.Output.Info(LanguageInfo.Dialog_Publish_Success, true);
						result = true;
					}
					else
					{
						LogConfig.Output.Error(LanguageInfo.Dialog_Publish_Failed);
						Services.Workbench.Pads.OutputPad.BringToFront();
						result = false;
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error(LanguageInfo.Dialog_Publish_Failed, exception);
				GLib.Timeout.Add(0U, delegate
				{
					Services.Workbench.Pads.OutputPad.BringToFront();
					return false;
				});
				result = false;
			}
			finally
			{
				Tracker.Add(ViewRegions.None, "Publish", methodName, "");
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000033E4 File Offset: 0x000015E4
		private bool SelectRunType(out EnumPlatform runType)
		{
			SelectRunTypeDialog selectRunTypeDialog = new SelectRunTypeDialog();
			int num = selectRunTypeDialog.Run();
			selectRunTypeDialog.Destroy();
			int runType2 = (int)selectRunTypeDialog.RunType;
			runType = (EnumPlatform)runType2;
			return num == -5;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003418 File Offset: 0x00001618
		private void Run(EnumPlatform runType)
		{
			if (!Cocos2dxServices.SupplymentServices.Supplyment(EnumSolutionCodeType.Complete, EnumOperationType.Run))
			{
				return;
			}
			PackageParams prms = PackageServices.Instance.PackageParams;
			prms.RunPlatform = runType;
			IPlatform platform = Cocos2dxServices.PlatformServices.CheckCanRun(prms);
			if (platform == null)
			{
				return;
			}
			if (!PublishUC.PublishSolution())
			{
				return;
			}
			CocosMonitor monitor = new CocosMonitor(true);
			monitor.Finished += this.RunFinishedHandler;
			if (!platform.IsShowConsoleWhenRun)
			{
				ProcessWindow processWindow = new ProcessWindow(LanguageInfo.Menu_Project_RunProject, true, true, true);
				processWindow.StartRunning(monitor);
			}
			Task task = new Task(delegate()
			{
				Cocos2dxServices.PlatformServices.StartRun(platform, prms, monitor);
			});
			task.Start();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000034E0 File Offset: 0x000016E0
		private void RunFinishedHandler(object sender, FinishedArgs e)
		{
			CocosMonitor cocosMonitor = sender as CocosMonitor;
			cocosMonitor.Finished -= this.RunFinishedHandler;
			if (cocosMonitor.IsCancelled)
			{
				LogConfig.Output.Info(LanguageInfo.Run_RunIsCanceled, true);
				return;
			}
			PackageParams packageParams = Cocos2dxServices.PackageServices.PackageParams;
			string text = "RunOn" + packageParams.RunPlatform;
			if (cocosMonitor.IsSuccessed)
			{
				LogConfig.Output.Info(LanguageInfo.Run_RunSuccessed, true);
			}
			else
			{
				LogConfig.Output.Info(LanguageInfo.Run_RunFinished, true);
				LogConfig.OutputWithoutTip.Info(cocosMonitor.FullOutputInfo, true);
				text += "Failed";
			}
			Tracker.Add(ViewRegions.UIMainTool, "Run", text, "");
		}
	}
}
