using System;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Projects
{
	// Token: 0x02000177 RID: 375
	public interface IAssemblyProject
	{
		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000EAB RID: 3755
		TargetFramework TargetFramework { get; }

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000EAC RID: 3756
		TargetRuntime TargetRuntime { get; }
	}
}
