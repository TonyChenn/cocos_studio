using System;
using Mono.PkgConfig;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000A1 RID: 161
	internal class PcFileCacheContext : IPcFileCacheContext<LibraryPackageInfo>
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x000135B0 File Offset: 0x000117B0
		public void ReportError(string message, Exception ex)
		{
			LoggingService.LogError(message, ex);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000135BC File Offset: 0x000117BC
		public bool IsCustomDataComplete(string pcfile, LibraryPackageInfo pkg)
		{
			string data = pkg.GetData("targetFramework");
			return data != null && data != "Unknown";
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000135E8 File Offset: 0x000117E8
		public void StoreCustomData(PcFile pcfile, LibraryPackageInfo pinfo)
		{
			TargetFramework targetFramework = null;
			bool flag = false;
			foreach (PackageAssemblyInfo packageAssemblyInfo in pinfo.Assemblies)
			{
				TargetFrameworkMoniker targetFrameworkForAssembly = Runtime.SystemAssemblyService.GetTargetFrameworkForAssembly(Runtime.SystemAssemblyService.CurrentRuntime, packageAssemblyInfo.File);
				if (targetFramework == null)
				{
					targetFramework = Runtime.SystemAssemblyService.GetTargetFramework(targetFrameworkForAssembly);
					if (targetFramework == null)
					{
						flag = true;
					}
				}
				else if (targetFrameworkForAssembly != null)
				{
					TargetFramework targetFramework2 = Runtime.SystemAssemblyService.GetTargetFramework(targetFrameworkForAssembly);
					if (targetFramework2 == null)
					{
						flag = true;
					}
					else if (targetFramework2.CanReferenceAssembliesTargetingFramework(targetFramework))
					{
						targetFramework = targetFramework2;
					}
					else if (!targetFramework.CanReferenceAssembliesTargetingFramework(targetFramework2))
					{
						flag = true;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				LoggingService.LogError("Inconsistent target frameworks found in " + pcfile);
			}
			if (targetFramework != null)
			{
				pinfo.SetData("targetFramework", targetFramework.Id.ToString());
				return;
			}
			pinfo.SetData("targetFramework", "FxUnknown");
		}
	}
}
