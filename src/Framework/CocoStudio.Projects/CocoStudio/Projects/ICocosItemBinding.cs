using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	[TypeExtensionPoint]
	public interface ICocosItemBinding
	{
		bool CanCreateItem(string fileType);

		CocosItem CreateItem(CocosItemCreateInfo info);
	}
}
