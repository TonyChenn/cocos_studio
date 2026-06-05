using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000D6 RID: 214
	public class ComposedAssemblyContext : IAssemblyContext
	{
		// Token: 0x0600076D RID: 1901 RVA: 0x0001C8C4 File Offset: 0x0001AAC4
		public void Add(IAssemblyContext ctx)
		{
			this.sources.Add(ctx);
			ctx.Changed += this.CtxChanged;
			this.CtxChanged(null, null);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001C8EC File Offset: 0x0001AAEC
		public void Remove(IAssemblyContext ctx)
		{
			if (this.sources.Remove(ctx))
			{
				ctx.Changed -= this.CtxChanged;
				this.CtxChanged(null, null);
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001C918 File Offset: 0x0001AB18
		public void Replace(IAssemblyContext oldCtx, IAssemblyContext newCtx)
		{
			int num = this.sources.IndexOf(oldCtx);
			if (num != -1)
			{
				this.sources[num] = newCtx;
				this.CtxChanged(null, null);
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001C94C File Offset: 0x0001AB4C
		public void Dispose()
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				assemblyContext.Changed -= this.CtxChanged;
			}
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0001C9AC File Offset: 0x0001ABAC
		private void CtxChanged(object sender, EventArgs e)
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06000772 RID: 1906 RVA: 0x0001C9C8 File Offset: 0x0001ABC8
		// (remove) Token: 0x06000773 RID: 1907 RVA: 0x0001CA00 File Offset: 0x0001AC00
		public event EventHandler Changed;

		// Token: 0x06000774 RID: 1908 RVA: 0x0001CC48 File Offset: 0x0001AE48
		public IEnumerable<SystemPackage> GetPackages()
		{
			foreach (IAssemblyContext ctx in this.sources)
			{
				foreach (SystemPackage p in ctx.GetPackages())
				{
					yield return p;
				}
			}
			yield break;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001CE8C File Offset: 0x0001B08C
		public IEnumerable<SystemPackage> GetPackages(TargetFramework fx)
		{
			foreach (IAssemblyContext ctx in this.sources)
			{
				foreach (SystemPackage p in ctx.GetPackages(fx))
				{
					yield return p;
				}
			}
			yield break;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001CEB0 File Offset: 0x0001B0B0
		public SystemAssembly[] GetAssembliesFromFullName(string fullname)
		{
			List<SystemAssembly> list = new List<SystemAssembly>();
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				foreach (SystemAssembly item in assemblyContext.GetAssembliesFromFullName(fullname))
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0001D144 File Offset: 0x0001B344
		public IEnumerable<SystemAssembly> GetAssemblies()
		{
			foreach (IAssemblyContext ctx in this.sources)
			{
				foreach (SystemAssembly sa in ctx.GetAssemblies())
				{
					yield return sa;
				}
			}
			yield break;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001D388 File Offset: 0x0001B588
		public IEnumerable<SystemAssembly> GetAssemblies(TargetFramework fx)
		{
			foreach (IAssemblyContext ctx in this.sources)
			{
				foreach (SystemAssembly sa in ctx.GetAssemblies(fx))
				{
					yield return sa;
				}
			}
			yield break;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001D3AC File Offset: 0x0001B5AC
		public SystemAssembly GetAssemblyFromFullName(string fullname, string package, TargetFramework fx)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				SystemAssembly assemblyFromFullName = assemblyContext.GetAssemblyFromFullName(fullname, package, fx);
				if (assemblyFromFullName != null)
				{
					return assemblyFromFullName;
				}
			}
			return null;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001D40C File Offset: 0x0001B60C
		public SystemPackage[] GetPackagesFromFullName(string fullname)
		{
			List<SystemPackage> list = new List<SystemPackage>();
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				foreach (SystemPackage item in assemblyContext.GetPackagesFromFullName(fullname))
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001D490 File Offset: 0x0001B690
		public SystemPackage GetPackage(string name)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				SystemPackage package = assemblyContext.GetPackage(name);
				if (package != null)
				{
					return package;
				}
			}
			return null;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001D4F0 File Offset: 0x0001B6F0
		public SystemPackage GetPackage(string name, string version)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				SystemPackage package = assemblyContext.GetPackage(name, version);
				if (package != null)
				{
					return package;
				}
			}
			return null;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001D550 File Offset: 0x0001B750
		public SystemPackage GetPackageFromPath(string path)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				SystemPackage packageFromPath = assemblyContext.GetPackageFromPath(path);
				if (packageFromPath != null)
				{
					return packageFromPath;
				}
			}
			return null;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001D5B0 File Offset: 0x0001B7B0
		public string FindInstalledAssembly(string fullname, string package, TargetFramework fx)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				string text = assemblyContext.FindInstalledAssembly(fullname, package, fx);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001D610 File Offset: 0x0001B810
		public string GetAssemblyLocation(string assemblyName, TargetFramework fx)
		{
			return this.GetAssemblyLocation(assemblyName, null, fx);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001D61C File Offset: 0x0001B81C
		public string GetAssemblyLocation(string assemblyName, string package, TargetFramework fx)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				string assemblyLocation = assemblyContext.GetAssemblyLocation(assemblyName, package, fx);
				if (assemblyLocation != null)
				{
					return assemblyLocation;
				}
			}
			return null;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001D67C File Offset: 0x0001B87C
		public bool AssemblyIsInGac(string aname)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				if (assemblyContext.AssemblyIsInGac(aname))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001D6D8 File Offset: 0x0001B8D8
		public string GetAssemblyNameForVersion(string fullName, TargetFramework fx)
		{
			return this.GetAssemblyNameForVersion(fullName, null, fx);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001D6E4 File Offset: 0x0001B8E4
		public string GetAssemblyNameForVersion(string fullName, string packageName, TargetFramework fx)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				string assemblyNameForVersion = assemblyContext.GetAssemblyNameForVersion(fullName, packageName, fx);
				if (assemblyNameForVersion != null)
				{
					return assemblyNameForVersion;
				}
			}
			return null;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001D744 File Offset: 0x0001B944
		public SystemAssembly GetAssemblyForVersion(string fullName, string packageName, TargetFramework fx)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				SystemAssembly assemblyForVersion = assemblyContext.GetAssemblyForVersion(fullName, packageName, fx);
				if (assemblyForVersion != null)
				{
					return assemblyForVersion;
				}
			}
			return null;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001D7A4 File Offset: 0x0001B9A4
		public string GetAssemblyFullName(string assemblyName, TargetFramework fx)
		{
			foreach (IAssemblyContext assemblyContext in this.sources)
			{
				string assemblyFullName = assemblyContext.GetAssemblyFullName(assemblyName, fx);
				if (assemblyFullName != null)
				{
					return assemblyFullName;
				}
			}
			return null;
		}

		// Token: 0x0400026D RID: 621
		private List<IAssemblyContext> sources = new List<IAssemblyContext>();
	}
}
