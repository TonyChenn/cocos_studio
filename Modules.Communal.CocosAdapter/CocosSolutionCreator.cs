using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000022 RID: 34
	public class CocosSolutionCreator
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00005D9A File Offset: 0x00003F9A
		internal CocosSolutionCreator()
		{
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005DA4 File Offset: 0x00003FA4
		public void CreateCocosSolution(CreateParams prms, CocosMonitor monitor)
		{
			monitor.Start();
			if (prms == null)
			{
				monitor.SendInfo("The create params is null");
				monitor.Finish(false);
				return;
			}
			string path = Path.Combine(prms.Directory, prms.ProjName);
			if (Directory.Exists(path))
			{
				try
				{
					monitor.SendInfo("Delete exist files");
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					directoryInfo.Delete(true);
				}
				catch
				{
					monitor.SendInfo("Failed to delete exist files");
					monitor.Finish(false);
					return;
				}
			}
			foreach (ICreateStep createStep in new List<ICreateStep>
			{
				new StepCocosV3(),
				new StepX86()
			})
			{
				if (createStep.CanCreate(prms) && !createStep.Run(prms, monitor))
				{
					monitor.Finish(false);
					break;
				}
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005E98 File Offset: 0x00004098
		public void SaveStatus(CreateParams prms, Solution sln = null)
		{
			if (sln == null)
			{
				sln = this.GetSolutionByPath(prms.Directory, prms.ProjName);
			}
			if (sln == null)
			{
				LogConfig.Output.Error(LanguageInfo.Output_FailedToGetSln);
				return;
			}
			Services.ProjectsService.CurrentSolution = sln;
			CocosProperties cocosProperties = new CocosProperties();
			sln.Config.CustomProperties["CCS_CocosPropertis"] = cocosProperties;
			if (prms.EngineInfo != null)
			{
				cocosProperties.ProgramLanguage = prms.Language;
				cocosProperties.CreateFrameworkVersion = (cocosProperties.CurrentFrameworkVersion = prms.EngineInfo.VersionText);
				if (cocosProperties.CurrentFrameworkVersion.Equals("IDE cocos"))
				{
					cocosProperties.SolutionCodeType = EnumSolutionCodeType.CodeIDE;
				}
				else
				{
					cocosProperties.SolutionCodeType = EnumSolutionCodeType.Complete;
				}
				if (prms.Language == EnumProgramLanguage.cpp)
				{
					sln.Config.PublishDirectory = Cocos2dxServices.SupplymentServices.DefaultCppPublishDir;
				}
				else if (prms.Language == EnumProgramLanguage.js)
				{
					sln.Config.DefaultSerializer = "Serializer_Json";
					sln.Config.CustomSerializer = "Serializer_Json";
				}
			}
			else
			{
				cocosProperties.SolutionCodeType = EnumSolutionCodeType.Resource;
				cocosProperties.ProgramLanguage = EnumProgramLanguage.none;
			}
			if (!prms.IsHorizonScreen)
			{
				sln.Config.SolutionSize = "640 * 960";
			}
			sln.SetPublishDirectory();
			sln.SetPackageDirectory();
			sln.Config.Save();
			Services.ProjectsService.CurrentSolution = null;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005FDC File Offset: 0x000041DC
		private Solution GetSolutionByPath(string directory, string name)
		{
			string filename = Path.Combine(directory, name, name + ".ccs");
			return ProjectsService.Instance.GetWrapperSolution(ProjectsService.Instance.DefaultMonitor, filename);
		}
	}
}
