using System;
using CocoStudio.Model;
using Mono.Addins;

namespace CocoStudio.Projects
{
	[TypeExtensionPoint]
	public interface IResource
	{
		ResourceData GetResourceData();
	}
}
