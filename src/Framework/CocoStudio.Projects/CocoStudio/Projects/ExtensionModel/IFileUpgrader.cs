using System;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel
{
	[TypeExtensionPoint]
	public interface IFileUpgrader : IUpgrader
	{
		bool Upgrade(CocosFile file);
	}
}
