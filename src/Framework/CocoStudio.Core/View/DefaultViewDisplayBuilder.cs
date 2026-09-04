using System;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	// Token: 0x0200003B RID: 59
	public class DefaultViewDisplayBuilder : IViewDisplayBuilder, IDisplayBuilder
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00009FEC File Offset: 0x000081EC
		public string Name
		{
			get
			{
				return "MainRenderViewBuilder";
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000A004 File Offset: 0x00008204
		public bool CanUseAsDefault
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A018 File Offset: 0x00008218
		public IViewContentExtend CreateContent(FilePath fileName, string mimeType, CocosItem ownerProject)
		{
			IMainRender mainRender = MainWindowPartFactory.CreateMainRenderContent();
			mainRender.File = ownerProject;
			return mainRender;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000A03C File Offset: 0x0000823C
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
