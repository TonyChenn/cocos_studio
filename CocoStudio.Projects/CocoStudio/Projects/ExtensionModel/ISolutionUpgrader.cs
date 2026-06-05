using System;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel
{
	// Token: 0x02000009 RID: 9
	[TypeExtensionPoint]
	internal interface ISolutionUpgrader : IUpgrader
	{
		// Token: 0x0600001D RID: 29
		bool Upgrade(Solution solution);
	}
}
