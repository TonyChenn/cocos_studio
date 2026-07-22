using System;
using System.Collections.Generic;
using CocoStudio.Model.DataModel;
using CocoStudio.Projects;
using Mono.Addins;

namespace CocoStudio.Model
{
	// Token: 0x020000C6 RID: 198
	public static class ProjectFileTemplateService
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00019974 File Offset: 0x00017B74
		public static IReadOnlyList<BaseProjectFileTemplate> ProjectFileTemplateList
		{
			get
			{
				return ProjectFileTemplateService.fileViewList;
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001998B File Offset: 0x00017B8B
		static ProjectFileTemplateService()
		{
			ProjectFileTemplateService.Initialize();
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0001999C File Offset: 0x00017B9C
		private static void Initialize()
		{
			if (!ProjectFileTemplateService.hasInitialized)
			{
				ProjectFileTemplateService.fileViewList = new List<BaseProjectFileTemplate>();
				BaseProjectFileTemplate[] extensionObjects = AddinManager.GetExtensionObjects<BaseProjectFileTemplate>();
				foreach (BaseProjectFileTemplate item in extensionObjects)
				{
					ProjectFileTemplateService.fileViewList.Add(item);
				}
				ProjectFileTemplateService.fileViewList.Sort();
				ProjectFileTemplateService.hasInitialized = true;
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00019A08 File Offset: 0x00017C08
		public static IProjectFileRenderView GetRenderView(this CocosItem project)
		{
			IProjectFileRenderView result = null;
			foreach (BaseProjectFileTemplate baseProjectFileTemplate in ProjectFileTemplateService.ProjectFileTemplateList)
			{
				if (baseProjectFileTemplate.FileType.ToString().Equals(project.ContentType))
				{
					result = baseProjectFileTemplate;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00019A90 File Offset: 0x00017C90
		public static IProjectFileCreator GetProjectFileCreator(NodeType nodeType)
		{
			IProjectFileCreator result = null;
			foreach (BaseProjectFileTemplate baseProjectFileTemplate in ProjectFileTemplateService.ProjectFileTemplateList)
			{
				if (baseProjectFileTemplate.FileType == nodeType)
				{
					result = baseProjectFileTemplate;
					break;
				}
			}
			return result;
		}

		// Token: 0x040002BE RID: 702
		private static List<BaseProjectFileTemplate> fileViewList;

		// Token: 0x040002BF RID: 703
		private static bool hasInitialized = false;
	}
}
