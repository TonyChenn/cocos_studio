using System;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	// Token: 0x02000039 RID: 57
	[TypeExtensionPoint(Path = "CocoStudio/Ide/DisplayBuilder")]
	public interface IDisplayBuilder
	{
		// Token: 0x06000212 RID: 530
		bool CanHandle(FilePath fileName, string mimeType, CocosItem ownerProject);

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000213 RID: 531
		bool CanUseAsDefault { get; }
	}
}
