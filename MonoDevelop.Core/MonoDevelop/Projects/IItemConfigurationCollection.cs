using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoDevelop.Projects
{
	// Token: 0x02000145 RID: 325
	public interface IItemConfigurationCollection : ICollection<ItemConfiguration>, IEnumerable<ItemConfiguration>, IEnumerable
	{
		// Token: 0x170002A2 RID: 674
		ItemConfiguration this[string name]
		{
			get;
		}
	}
}
