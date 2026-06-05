using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x02000046 RID: 70
	public interface IInitialize
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001DB RID: 475
		bool IsAutoInitialize { get; }

		// Token: 0x060001DC RID: 476
		void Initialize(IProgressMonitor monitor);
	}
}
