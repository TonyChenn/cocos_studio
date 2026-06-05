using System;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	// Token: 0x0200003A RID: 58
	public interface IViewDisplayBuilder : IDisplayBuilder
	{
		// Token: 0x06000214 RID: 532
		IViewContentExtend CreateContent(FilePath fileName, string mimeType, CocosItem ownerProject);

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000215 RID: 533
		string Name { get; }
	}
}
