using System;

namespace CocoStudio.Model
{
	// Token: 0x0200000F RID: 15
	public interface ILayoutSizeWithType : ILayoutSize
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000062 RID: 98
		// (set) Token: 0x06000063 RID: 99
		bool IsCustomSize { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000064 RID: 100
		ObjectSizeType SupportSizeType { get; }
	}
}
