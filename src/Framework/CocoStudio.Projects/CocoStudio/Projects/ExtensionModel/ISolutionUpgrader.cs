using System;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel
{
	[TypeExtensionPoint]
	internal interface ISolutionUpgrader : IUpgrader
	{
		bool Upgrade(Solution solution);
	}
}
