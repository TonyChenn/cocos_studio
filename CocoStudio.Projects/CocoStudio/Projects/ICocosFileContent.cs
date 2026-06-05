using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000062 RID: 98
	[TypeExtensionPoint]
	public interface ICocosFileContent : ICocosFile, IInitialize, ICocosItem
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002DD RID: 733
		// (set) Token: 0x060002DE RID: 734
		CocosFile CocosFile { get; set; }
	}
}
