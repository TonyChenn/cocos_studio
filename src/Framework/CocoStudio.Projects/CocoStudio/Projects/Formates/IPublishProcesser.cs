using System;
using System.Collections.Generic;
using CocoStudio.Model;
using Mono.Addins;

namespace CocoStudio.Projects.Formates
{
	[TypeExtensionPoint]
	public interface IPublishProcesser
	{
		bool CanProcess(ResourceData resourceData);

		HashSet<ResourceData> Process(ResourceData resourceData);
	}
}
