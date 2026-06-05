using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects
{
	// Token: 0x0200013E RID: 318
	internal interface IItemListHandler
	{
		// Token: 0x06000BEF RID: 3055
		void InternalAdd(IEnumerable<ProjectItem> items, bool comesFromParent);

		// Token: 0x06000BF0 RID: 3056
		void InternalRemove(IEnumerable<ProjectItem> items, bool comesFromParent);

		// Token: 0x06000BF1 RID: 3057
		bool CanHandle(ProjectItem item);
	}
}
