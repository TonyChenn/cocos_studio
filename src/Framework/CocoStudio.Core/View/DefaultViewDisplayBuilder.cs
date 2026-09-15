using System;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	public class DefaultViewDisplayBuilder : IViewDisplayBuilder, IDisplayBuilder
	{
		public string Name
		{
			get
			{
				return "MainRenderViewBuilder";
			}
		}

		public bool CanUseAsDefault
		{
			get
			{
				return true;
			}
		}

		public IViewContentExtend CreateContent(FilePath fileName, string mimeType, CocosItem ownerProject)
		{
			IMainRender mainRender = MainWindowPartFactory.CreateMainRenderContent();
			mainRender.File = ownerProject;
			return mainRender;
		}

		public bool CanHandle(FilePath fileName, string mimeType, CocosItem ownerProject)
		{
			bool result;
			if (ownerProject == null)
			{
				result = false;
			}
			else
			{
				if (!ownerProject.IsInitialized)
				{
					ownerProject.Initialize(Services.ProgressMonitors.Default);
				}
				result = (ownerProject.CocosFile is GameFile);
			}
			return result;
		}
	}
}
