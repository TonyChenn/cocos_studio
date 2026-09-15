using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	[TypeExtensionPoint]
	public interface ICocosFileContent : ICocosFile, IInitialize, ICocosItem
	{
		CocosFile CocosFile { get; set; }
	}
}
