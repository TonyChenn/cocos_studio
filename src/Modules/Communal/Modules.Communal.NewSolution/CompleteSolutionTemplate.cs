using System;
using System.IO;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	[SolutionTemplate(true)]
	[Extension(typeof(ISolutionTemplate))]
	internal class CompleteSolutionTemplate : BaseSolutionTemplate
	{
		public override EnumTemplateGroup Group
		{
			get
			{
				return EnumTemplateGroup.Project;
			}
		}

		public CompleteSolutionTemplate()
		{
			string newSolution_CocosProject = LanguageInfo.NewSolution_CocosProject;
			string newSolution_EmptyCompleteDes = LanguageInfo.NewSolution_EmptyCompleteDes;
			EnumSolutionType enumSolutionType = EnumSolutionType.Complete;
			Xwt.Drawing.Image iconImage = base.GetIconImage(enumSolutionType);
			base.Info = new SolutionTypeInfo(newSolution_CocosProject, newSolution_EmptyCompleteDes, enumSolutionType, iconImage, true, true, true, false, null);
		}

		protected override bool OnCreateNewSolution(CreateParams prms, CocosMonitor monitor)
		{
			CreatingDialog creatingDialog = new CreatingDialog();
			Task task = new Task(delegate()
			{
				this.CreateCocosSolution(prms, monitor);
			});
			task.Start();
			creatingDialog.StartRunning(monitor);
			return monitor.IsSuccessed;
		}

		private void CreateCocosSolution(CreateParams prms, CocosMonitor monitor)
		{
			Cocos2dxServices.CreateServices.CreateCocosSolution(prms, monitor);
			if (monitor.IsProcessing)
			{
				try
				{
					string filename = Path.Combine(prms.Directory, prms.ProjName, prms.ProjName + ".ccs");
					IProgressMonitor @default = Services.ProgressMonitors.Default;
					Solution wrapperSolution = Services.ProjectsService.GetWrapperSolution(@default, filename);
					string filePath = SolutionConfig.GetFilePath(wrapperSolution);
					if (File.Exists(filePath))
					{
						File.Delete(filePath);
					}
					string filePath2 = UserData.GetFilePath(wrapperSolution.FileName);
					if (File.Exists(filePath2))
					{
						File.Delete(filePath2);
					}
					Cocos2dxServices.CreateServices.SaveStatus(prms, wrapperSolution);
					monitor.Finish(true);
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("Console创建项目已经完成，保存项目参数设置时出错", exception);
					monitor.Finish(false);
				}
			}
		}

		protected override string OnGetDefaultScenePath(CreateParams prms)
		{
			return Path.Combine(prms.Directory, prms.ProjName, "CocosStudio".ToLower(), "MainScene.csd");
		}
	}
}
