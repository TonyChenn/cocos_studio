using System;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel
{
	// Token: 0x02000005 RID: 5
	[TypeExtensionPoint]
	public interface IFileUpgrader : IUpgrader
	{
		// Token: 0x0600000B RID: 11
		bool Upgrade(CocosFile file);
	}
}
