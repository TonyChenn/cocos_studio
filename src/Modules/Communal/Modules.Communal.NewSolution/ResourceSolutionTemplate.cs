using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using CocoStudio.Projects.Visiter;
using Gdk;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000006 RID: 6
	[Extension(typeof(ISolutionTemplate))]
	[SolutionTemplate(true)]
	internal class ResourceSolutionTemplate : BaseSolutionTemplate
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000023AA File Offset: 0x000005AA
		public override EnumTemplateGroup Group
		{
			get
			{
				return EnumTemplateGroup.Project;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023B0 File Offset: 0x000005B0
		public ResourceSolutionTemplate()
		{
			string newSolution_EmptyResource = LanguageInfo.NewSolution_EmptyResource;
			string newSolution_EmptyResourceDes = LanguageInfo.NewSolution_EmptyResourceDes;
			EnumSolutionType enumSolutionType = EnumSolutionType.Resource;
			Xwt.Drawing.Image iconImage = base.GetIconImage(enumSolutionType);
			base.Info = new SolutionTypeInfo(newSolution_EmptyResource, newSolution_EmptyResourceDes, enumSolutionType, iconImage, false, false, false, false, null);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002418 File Offset: 0x00000618
		protected override bool OnCreateNewSolution(CreateParams prms, CocosMonitor monitor)
		{
			Solution solution = Services.ProjectsService.CreateSolution(prms.Directory, prms.ProjName);
			float width = (float)(prms.IsHorizonScreen ? 960 : 640);
			float height = (float)(prms.IsHorizonScreen ? 640 : 960);
			solution.SetSceneSize(new Size((int)width, (int)height));
			ResolutionConfig resolutionConfig = Option.UserConfig.ResolutionList.Find((ResolutionConfig w) => (float)w.Width == width && (float)w.Height == height);
			if (resolutionConfig != null)
			{
				solution.SetResolutionName(resolutionConfig.Name);
			}
			else
			{
				solution.SetResolutionName("Default");
			}
			string name = this.OnGetDefaultScenePath(prms);
			CocosItemCreateInfo info = new CocosItemCreateInfo(name, null, width, height);
			CocosItem cocosItem = Services.ProjectsService.CreateCocosItem("Scene", info);
			((ResourceGroup)solution.RootFolder.Items[0]).RootFolder.Items.Add(cocosItem);
			IProgressMonitor @default = Services.ProgressMonitors.Default;
			cocosItem.Save(@default);
			cocosItem.Initialize(@default);
			Services.ProjectsService.CurrentSolution = solution;
			solution.Save(@default);
			Cocos2dxServices.CreateServices.SaveStatus(prms, solution);
			Services.ProjectsService.CurrentSolution = null;
			return true;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002574 File Offset: 0x00000774
		public override bool Enable
		{
			get
			{
				string path = Path.Combine(Option.UserCustomerConfigFolder, "EnableEmptyResourceProject.xml");
				return File.Exists(path) || (FrameworkHelper.EnabledVersions.Count == 0 && Cocos2dxInfo.GetSimplifiedConsole() == null);
			}
		}
	}
}
