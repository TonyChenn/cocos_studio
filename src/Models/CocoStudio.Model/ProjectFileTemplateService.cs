using System;
using System.Collections.Generic;
using CocoStudio.Model.DataModel;
using CocoStudio.Projects;
using Mono.Addins;

namespace CocoStudio.Model
{
	public static class ProjectFileTemplateService
	{
		public static IReadOnlyList<BaseProjectFileTemplate> ProjectFileTemplateList
		{
			get
			{
				return ProjectFileTemplateService.fileViewList;
			}
		}

		static ProjectFileTemplateService()
		{
			ProjectFileTemplateService.Initialize();
		}

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

		private static List<BaseProjectFileTemplate> fileViewList;

		private static bool hasInitialized = false;
	}
}
