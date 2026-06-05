using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x02000240 RID: 576
	internal class AssemblyInstalledCondition : ConditionType
	{
		// Token: 0x06001542 RID: 5442 RVA: 0x00056E30 File Offset: 0x00055030
		public override bool Evaluate(NodeElement conditionNode)
		{
			string name = conditionNode.GetAttribute("name");
			List<SystemAssembly> list = (from asm in Runtime.SystemAssemblyService.CurrentRuntime.RuntimeAssemblyContext.GetAssemblies()
			where asm.Name == name
			select asm).ToList<SystemAssembly>();
			if (list.Count == 0)
			{
				return false;
			}
			string version = conditionNode.GetAttribute("version");
			if (!string.IsNullOrEmpty(version))
			{
				return list.Any((SystemAssembly asm) => asm.Version == version);
			}
			string minVersion = conditionNode.GetAttribute("minVersion");
			if (!string.IsNullOrEmpty(minVersion))
			{
				if (!list.Any((SystemAssembly asm) => Addin.CompareVersions(minVersion, asm.Version) >= 0))
				{
					return false;
				}
			}
			string maxVersion = conditionNode.GetAttribute("maxVersion");
			if (!string.IsNullOrEmpty(maxVersion))
			{
				if (list.Any((SystemAssembly asm) => Addin.CompareVersions(maxVersion, asm.Version) > 0))
				{
					return false;
				}
			}
			return true;
		}
	}
}
