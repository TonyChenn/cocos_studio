using System;
using System.Collections.Generic;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x0200009A RID: 154
	public interface ITargetRuntimeFactory
	{
		// Token: 0x060004F4 RID: 1268
		IEnumerable<TargetRuntime> CreateRuntimes();
	}
}
