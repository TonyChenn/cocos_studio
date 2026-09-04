using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x02000059 RID: 89
	public interface ICocosItem
	{
		// Token: 0x06000277 RID: 631
		void ReloadReferencedItem(IProgressMonitor monitor);

		// Token: 0x06000278 RID: 632
		bool HasReferencedItem(CocosItem item);
	}
}
