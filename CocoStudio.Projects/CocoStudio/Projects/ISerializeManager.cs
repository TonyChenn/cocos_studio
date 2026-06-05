using System;
using System.Collections.Generic;

namespace CocoStudio.Projects
{
	// Token: 0x0200005C RID: 92
	public interface ISerializeManager
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600029F RID: 671
		// (set) Token: 0x060002A0 RID: 672
		IGameFileSerializer CurrentSerializer { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002A1 RID: 673
		IGameFileSerializer DefaultSerializer { get; }

		// Token: 0x060002A2 RID: 674
		IEnumerable<IGameFileSerializer> GetSerializerList();
	}
}
