using System;
using System.Collections.Generic;
using CocoStudio.Model;
using Mono.Addins;

namespace CocoStudio.Projects.Formates.ExtendResourceItemFormates
{
	[Extension(typeof(IPublishProcesser))]
	internal class PlistImageFileFormat : IPublishProcesser
	{
		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			return resourceData.Type == EnumResourceType.PlistSubImage;
		}

		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			ResourceData resourceData2 = new ResourceData(EnumResourceType.Normal, resourceData.Plist);
			IPublishProcesser publishProcesser = ProjectsService.Instance.ProcesserManager.GetPublishProcesser<PlistImageFolderFormat>();
			return publishProcesser.Process(resourceData2);
		}
	}
}
