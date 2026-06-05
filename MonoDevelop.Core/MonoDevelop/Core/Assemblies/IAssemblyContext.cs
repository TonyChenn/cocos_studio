using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000D5 RID: 213
	public interface IAssemblyContext
	{
		// Token: 0x06000759 RID: 1881
		[Obsolete("Avoid use of SystemPackage")]
		IEnumerable<SystemPackage> GetPackages();

		// Token: 0x0600075A RID: 1882
		[Obsolete("Avoid use of SystemPackage")]
		IEnumerable<SystemPackage> GetPackages(TargetFramework fx);

		// Token: 0x0600075B RID: 1883
		SystemPackage GetPackage(string name);

		// Token: 0x0600075C RID: 1884
		SystemPackage GetPackage(string name, string version);

		// Token: 0x0600075D RID: 1885
		SystemPackage GetPackageFromPath(string path);

		// Token: 0x0600075E RID: 1886
		SystemPackage[] GetPackagesFromFullName(string fullname);

		// Token: 0x0600075F RID: 1887
		IEnumerable<SystemAssembly> GetAssemblies();

		// Token: 0x06000760 RID: 1888
		IEnumerable<SystemAssembly> GetAssemblies(TargetFramework fx);

		// Token: 0x06000761 RID: 1889
		SystemAssembly[] GetAssembliesFromFullName(string fullname);

		// Token: 0x06000762 RID: 1890
		SystemAssembly GetAssemblyFromFullName(string fullname, string package, TargetFramework fx);

		// Token: 0x06000763 RID: 1891
		string FindInstalledAssembly(string fullname, string package, TargetFramework fx);

		// Token: 0x06000764 RID: 1892
		string GetAssemblyLocation(string assemblyName, TargetFramework fx);

		// Token: 0x06000765 RID: 1893
		string GetAssemblyLocation(string assemblyName, string package, TargetFramework fx);

		// Token: 0x06000766 RID: 1894
		string GetAssemblyNameForVersion(string fullName, TargetFramework fx);

		// Token: 0x06000767 RID: 1895
		string GetAssemblyNameForVersion(string fullName, string packageName, TargetFramework fx);

		// Token: 0x06000768 RID: 1896
		SystemAssembly GetAssemblyForVersion(string fullName, string packageName, TargetFramework fx);

		// Token: 0x06000769 RID: 1897
		string GetAssemblyFullName(string assemblyName, TargetFramework fx);

		// Token: 0x0600076A RID: 1898
		bool AssemblyIsInGac(string aname);

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x0600076B RID: 1899
		// (remove) Token: 0x0600076C RID: 1900
		event EventHandler Changed;
	}
}
