using System;
using System.Collections.Generic;
using CocoStudio.Model;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x02000047 RID: 71
	public interface ICocosFile : IInitialize
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001DD RID: 477
		bool IsLoaded { get; }

		// Token: 0x060001DE RID: 478
		void Load(IProgressMonitor monitor);

		// Token: 0x060001DF RID: 479
		void Save(IProgressMonitor monitor);

		// Token: 0x060001E0 RID: 480
		void UnLoad(IProgressMonitor monitor);

		// Token: 0x060001E1 RID: 481
		HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor);

		// Token: 0x060001E2 RID: 482
		bool UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourceCollection);
	}
}
