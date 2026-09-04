using System;

namespace CocoStudio.Projects.ExtensionModel
{
	// Token: 0x02000004 RID: 4
	public interface IUpgrader
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9
		Version Version { get; }

		// Token: 0x0600000A RID: 10
		bool Upgrade(string filePath);
	}
}
