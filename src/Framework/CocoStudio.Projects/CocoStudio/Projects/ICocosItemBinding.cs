using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000060 RID: 96
	[TypeExtensionPoint]
	public interface ICocosItemBinding
	{
		// Token: 0x060002D6 RID: 726
		bool CanCreateItem(string fileType);

		// Token: 0x060002D7 RID: 727
		CocosItem CreateItem(CocosItemCreateInfo info);
	}
}
