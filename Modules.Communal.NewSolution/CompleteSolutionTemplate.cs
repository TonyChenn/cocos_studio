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
	// Token: 0x02000005 RID: 5
	[SolutionTemplate(true)]
	[Extension(typeof(ISolutionTemplate))]
	internal class CompleteSolutionTemplate : BaseSolutionTemplate
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000021F0 File Offset: 0x000003F0
		public override EnumTemplateGroup Group
		{
			get
			{
				return EnumTemplateGroup.Project;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021F4 File Offset: 0x000003F4
		public CompleteSolutionTemplate()
		{
			string newSolution_CocosProject = LanguageInfo.NewSolution_CocosProject;
			string newSolution_EmptyCompleteDes = LanguageInfo.NewSolution_EmptyCompleteDes;
			EnumSolutionType enumSolutionType = EnumSolutionType.Complete;
			Xwt.Drawing.Image iconImage = base.GetIconImage(enumSolutionType);
			base.Info = new SolutionTypeInfo(newSolution_CocosProject, newSolution_EmptyCompleteDes, enumSolutionType, iconImage, true, true, true, false, null);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002254 File Offset: 0x00000454
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

		// Token: 0x06000013 RID: 19 RVA: 0x000022B4 File Offset: 0x000004B4
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

		// Token: 0x06000014 RID: 20 RVA: 0x00002388 File Offset: 0x00000588
		protected override string OnGetDefaultScenePath(CreateParams prms)
		{
			return Path.Combine(prms.Directory, prms.ProjName, "CocosStudio".ToLower(), "MainScene.csd");
		}
	}
}
