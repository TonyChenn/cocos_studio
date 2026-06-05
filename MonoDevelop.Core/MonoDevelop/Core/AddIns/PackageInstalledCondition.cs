using System;
using Mono.Addins;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x02000095 RID: 149
	internal class PackageInstalledCondition : ConditionType
	{
		// Token: 0x060004EA RID: 1258 RVA: 0x00011094 File Offset: 0x0000F294
		public override bool Evaluate(NodeElement conditionNode)
		{
			string attribute = conditionNode.GetAttribute("name");
			SystemPackage packageInternal = Runtime.SystemAssemblyService.CurrentRuntime.RuntimeAssemblyContext.GetPackageInternal(attribute);
			if (packageInternal == null)
			{
				return false;
			}
			string attribute2 = conditionNode.GetAttribute("version");
			if (attribute2.Length > 0)
			{
				return attribute2 == packageInternal.Version;
			}
			attribute2 = conditionNode.GetAttribute("minVersion");
			if (attribute2.Length > 0)
			{
				return Addin.CompareVersions(attribute2, packageInternal.Version) >= 0;
			}
			attribute2 = conditionNode.GetAttribute("maxVersion");
			return attribute2.Length <= 0 || Addin.CompareVersions(attribute2, packageInternal.Version) <= 0;
		}
	}
}
