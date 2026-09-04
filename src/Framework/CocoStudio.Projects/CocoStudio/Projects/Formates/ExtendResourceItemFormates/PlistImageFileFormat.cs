using System;
using System.Collections.Generic;
using CocoStudio.Model;
using Mono.Addins;

namespace CocoStudio.Projects.Formates.ExtendResourceItemFormates
{
	// Token: 0x0200002C RID: 44
	[Extension(typeof(IPublishProcesser))]
	internal class PlistImageFileFormat : IPublishProcesser
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x00004D1D File Offset: 0x00002F1D
		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			return resourceData.Type == EnumResourceType.PlistSubImage;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004D2C File Offset: 0x00002F2C
		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			ResourceData resourceData2 = new ResourceData(EnumResourceType.Normal, resourceData.Plist);
			IPublishProcesser publishProcesser = ProjectsService.Instance.ProcesserManager.GetPublishProcesser<PlistImageFolderFormat>();
			return publishProcesser.Process(resourceData2);
		}
	}
}
