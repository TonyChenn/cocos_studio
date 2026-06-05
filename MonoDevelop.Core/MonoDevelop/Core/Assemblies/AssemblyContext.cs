using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.PkgConfig;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000D7 RID: 215
	public class AssemblyContext : IAssemblyContext
	{
		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000787 RID: 1927 RVA: 0x0001D818 File Offset: 0x0001BA18
		// (remove) Token: 0x06000788 RID: 1928 RVA: 0x0001D850 File Offset: 0x0001BA50
		public event EventHandler Changed;

		// Token: 0x06000789 RID: 1929 RVA: 0x0001D885 File Offset: 0x0001BA85
		public ICollection<string> GetAssemblyFullNames()
		{
			return this.assemblyFullNameToAsm.Keys;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001D892 File Offset: 0x0001BA92
		internal SystemPackage RegisterPackage(LibraryPackageInfo pinfo, bool isInternal)
		{
			return this.RegisterPackage(new SystemPackageInfo(pinfo), isInternal, pinfo.Assemblies.ToArray());
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001D8AC File Offset: 0x0001BAAC
		protected internal SystemPackage RegisterPackage(SystemPackageInfo pinfo, bool isInternal, params string[] assemblyFiles)
		{
			List<PackageAssemblyInfo> list = new List<PackageAssemblyInfo>(assemblyFiles.Length);
			foreach (string file in assemblyFiles)
			{
				try
				{
					PackageAssemblyInfo packageAssemblyInfo = new PackageAssemblyInfo();
					packageAssemblyInfo.File = file;
					packageAssemblyInfo.Update(SystemAssemblyService.GetAssemblyNameObj(packageAssemblyInfo.File));
					list.Add(packageAssemblyInfo);
				}
				catch
				{
				}
			}
			return this.RegisterPackage(pinfo, isInternal, list.ToArray());
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001D93C File Offset: 0x0001BB3C
		private SystemPackage RegisterPackage(SystemPackageInfo pinfo, bool isInternal, PackageAssemblyInfo[] assemblyFiles)
		{
			SystemPackage systemPackage;
			if (this.packagesHash.TryGetValue(pinfo.Name, out systemPackage))
			{
				if (pinfo.IsFrameworkPackage)
				{
					if (!systemPackage.IsFrameworkPackage)
					{
						this.ForceUnregisterPackage(systemPackage);
					}
				}
				else if (systemPackage.IsFrameworkPackage)
				{
					return systemPackage;
				}
			}
			SystemPackage systemPackage2 = new SystemPackage();
			List<SystemAssembly> list = new List<SystemAssembly>();
			int i = 0;
			while (i < assemblyFiles.Length)
			{
				PackageAssemblyInfo packageAssemblyInfo = assemblyFiles[i];
				if (pinfo.IsFrameworkPackage)
				{
					goto IL_8B;
				}
				if (!this.GetAssembliesFromFullNameInternal(packageAssemblyInfo.FullName, false).Any((SystemAssembly a) => a.Package != null && a.Package.IsFrameworkPackage))
				{
					goto IL_8B;
				}
				IL_A4:
				i++;
				continue;
				IL_8B:
				list.Add(this.AddAssembly(packageAssemblyInfo.File, new AssemblyInfo(packageAssemblyInfo), systemPackage2));
				goto IL_A4;
			}
			systemPackage2.Initialize(pinfo, list, isInternal);
			this.packages.Add(systemPackage2);
			this.packagesHash[pinfo.Name] = systemPackage2;
			this.NotifyChanged();
			return systemPackage2;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001DA2C File Offset: 0x0001BC2C
		protected internal void UnregisterPackage(string name, string version)
		{
			SystemPackage package = this.GetPackage(name, version);
			this.UnregisterPackage(package);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0001DA49 File Offset: 0x0001BC49
		protected internal void UnregisterPackage(SystemPackage p)
		{
			if (!p.IsInternalPackage)
			{
				throw new InvalidOperationException("Only internal packages can be unregistered");
			}
			this.ForceUnregisterPackage(p);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0001DA68 File Offset: 0x0001BC68
		private void ForceUnregisterPackage(SystemPackage p)
		{
			foreach (SystemAssembly asm in p.Assemblies)
			{
				this.RemoveAssembly(asm);
			}
			this.packages.Remove(p);
			this.packagesHash.Remove(p.Name);
			this.NotifyChanged();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001DADC File Offset: 0x0001BCDC
		protected void NotifyChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001DAF7 File Offset: 0x0001BCF7
		public IEnumerable<SystemPackage> GetPackages()
		{
			return this.packages;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001DAFF File Offset: 0x0001BCFF
		[Obsolete("Cannot de-duplicate framework assemblies")]
		public IEnumerable<SystemPackage> GetPackages(TargetFramework fx)
		{
			return this.GetPackagesInternal(fx);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001DD08 File Offset: 0x0001BF08
		private IEnumerable<SystemPackage> GetPackagesInternal(TargetFramework fx)
		{
			foreach (SystemPackage pkg in this.packages)
			{
				if (pkg.IsFrameworkPackage)
				{
					if (fx.IncludesFramework(pkg.TargetFramework))
					{
						yield return pkg;
					}
				}
				else if (fx.CanReferenceAssembliesTargetingFramework(pkg.TargetFramework))
				{
					yield return pkg;
				}
			}
			yield break;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001DD2C File Offset: 0x0001BF2C
		public SystemAssembly[] GetAssembliesFromFullName(string fullname)
		{
			List<SystemAssembly> list = new List<SystemAssembly>(this.GetAssembliesFromFullNameInternal(fullname));
			return list.ToArray();
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001DD4C File Offset: 0x0001BF4C
		private IEnumerable<SystemAssembly> GetAssembliesFromFullNameInternal(string fullname)
		{
			return this.GetAssembliesFromFullNameInternal(fullname, true);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001DE98 File Offset: 0x0001C098
		private IEnumerable<SystemAssembly> GetAssembliesFromFullNameInternal(string fullname, bool initialize)
		{
			if (initialize)
			{
				this.Initialize();
			}
			fullname = AssemblyContext.NormalizeAsmName(fullname);
			SystemAssembly asm;
			if (this.assemblyFullNameToAsm.TryGetValue(fullname, out asm))
			{
				while (asm != null)
				{
					yield return asm;
					asm = asm.NextSameName;
				}
			}
			yield break;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001E0E0 File Offset: 0x0001C2E0
		public IEnumerable<SystemAssembly> GetAssemblies()
		{
			this.Initialize();
			foreach (SystemPackage pkg in this.packages)
			{
				foreach (SystemAssembly asm in pkg.Assemblies)
				{
					yield return asm;
				}
			}
			yield break;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001E624 File Offset: 0x0001C824
		public IEnumerable<SystemAssembly> GetAssemblies(TargetFramework fx)
		{
			this.Initialize();
			if (fx == null)
			{
				foreach (SystemPackage pkg in this.packages)
				{
					if (!pkg.IsFrameworkPackage)
					{
						foreach (SystemAssembly asm in pkg.Assemblies)
						{
							yield return asm;
						}
					}
				}
			}
			else
			{
				Dictionary<string, List<SystemAssembly>> fxGroups = new Dictionary<string, List<SystemAssembly>>();
				foreach (SystemPackage pkg2 in this.GetPackagesInternal(fx))
				{
					if (pkg2.IsFrameworkPackage)
					{
						using (IEnumerator<SystemAssembly> enumerator4 = pkg2.Assemblies.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								SystemAssembly systemAssembly = enumerator4.Current;
								List<SystemAssembly> list;
								if (!fxGroups.TryGetValue(systemAssembly.FullName, out list))
								{
									list = (fxGroups[systemAssembly.FullName] = new List<SystemAssembly>());
								}
								list.Add(systemAssembly);
							}
							continue;
						}
					}
					foreach (SystemAssembly asm2 in pkg2.Assemblies)
					{
						yield return asm2;
					}
				}
				foreach (KeyValuePair<string, List<SystemAssembly>> g in fxGroups)
				{
					KeyValuePair<string, List<SystemAssembly>> keyValuePair = g;
					SystemAssembly a = AssemblyContext.BestFrameworkAssembly(keyValuePair.Value);
					if (a != null)
					{
						yield return a;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001E658 File Offset: 0x0001C858
		public SystemAssembly GetAssemblyFromFullName(string fullname, string package, TargetFramework fx)
		{
			if (package == null)
			{
				List<SystemAssembly> list = this.GetAssembliesFromFullNameInternal(fullname).ToList<SystemAssembly>();
				SystemAssembly result;
				if ((result = AssemblyContext.BestFrameworkAssembly(list, fx)) == null)
				{
					result = (list.FirstOrDefault((SystemAssembly a) => a.Package.IsGacPackage) ?? list.FirstOrDefault<SystemAssembly>());
				}
				return result;
			}
			foreach (SystemAssembly systemAssembly in this.GetAssembliesFromFullNameInternal(fullname))
			{
				if (package == systemAssembly.Package.Name)
				{
					return systemAssembly;
				}
			}
			return null;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001E704 File Offset: 0x0001C904
		public SystemPackage[] GetPackagesFromFullName(string fullname)
		{
			List<SystemPackage> list = new List<SystemPackage>();
			foreach (SystemAssembly systemAssembly in this.GetAssembliesFromFullNameInternal(fullname))
			{
				list.Add(systemAssembly.Package);
			}
			return list.ToArray();
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001E764 File Offset: 0x0001C964
		public SystemPackage GetPackage(string name)
		{
			this.Initialize();
			return this.GetPackageInternal(name);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001E774 File Offset: 0x0001C974
		protected internal SystemPackage GetPackageInternal(string name)
		{
			SystemPackage result;
			this.packagesHash.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0001E791 File Offset: 0x0001C991
		public SystemPackage GetPackage(string name, string version)
		{
			this.Initialize();
			return this.GetPackageInternal(name, version);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001E7A4 File Offset: 0x0001C9A4
		internal SystemPackage GetPackageInternal(string name, string version)
		{
			foreach (SystemPackage systemPackage in this.packages)
			{
				if (systemPackage.Name == name && systemPackage.Version == version)
				{
					return systemPackage;
				}
			}
			return null;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001E814 File Offset: 0x0001CA14
		public SystemPackage GetPackageFromPath(string path)
		{
			this.Initialize();
			if (!this.assemblyPathToPackage.ContainsKey(path))
			{
				return null;
			}
			return this.assemblyPathToPackage[path];
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001E838 File Offset: 0x0001CA38
		public static string NormalizeAsmName(string name)
		{
			int num = name.ToLower().IndexOf(", publickeytoken=null", StringComparison.Ordinal);
			if (num != -1)
			{
				name = name.Substring(0, num).Trim();
			}
			num = name.ToLower().IndexOf(", processorarchitecture=", StringComparison.Ordinal);
			if (num != -1)
			{
				name = name.Substring(0, num).Trim();
			}
			return name;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001E890 File Offset: 0x0001CA90
		public string FindInstalledAssembly(string fullname, string package, TargetFramework fx)
		{
			this.Initialize();
			fullname = AssemblyContext.NormalizeAsmName(fullname);
			SystemAssembly assemblyFromFullName = this.GetAssemblyFromFullName(fullname, package, fx);
			if (assemblyFromFullName != null)
			{
				return fullname;
			}
			if (fx == null)
			{
				string result = null;
				foreach (SystemAssembly systemAssembly in this.FindNewerAssembliesSameName(fullname))
				{
					if (package == null || systemAssembly.Package.Name == package)
					{
						if (systemAssembly.Package.IsFrameworkPackage)
						{
							return systemAssembly.FullName;
						}
						result = systemAssembly.FullName;
					}
				}
				return result;
			}
			List<SystemAssembly> list = this.FindNewerAssembliesSameName(fullname).ToList<SystemAssembly>();
			if (fx != null)
			{
				SystemAssembly systemAssembly2 = AssemblyContext.BestFrameworkAssembly(list, fx);
				if (systemAssembly2 != null)
				{
					return systemAssembly2.FullName;
				}
			}
			string result2 = null;
			foreach (SystemAssembly systemAssembly3 in list)
			{
				if (fx.CanReferenceAssembliesTargetingFramework(systemAssembly3.Package.TargetFramework))
				{
					if (package != null && systemAssembly3.Package.Name == package)
					{
						return systemAssembly3.FullName;
					}
					result2 = systemAssembly3.FullName;
				}
			}
			return result2;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001EA14 File Offset: 0x0001CC14
		private static SystemAssembly BestFrameworkAssembly(IEnumerable<SystemAssembly> assemblies, TargetFramework fx)
		{
			if (fx == null)
			{
				return null;
			}
			return AssemblyContext.BestFrameworkAssembly((from a in assemblies
			where a.Package != null && a.Package.IsFrameworkPackage && fx.IncludesFramework(a.Package.TargetFramework)
			select a).ToList<SystemAssembly>());
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001EA54 File Offset: 0x0001CC54
		private static SystemAssembly BestFrameworkAssembly(List<SystemAssembly> list)
		{
			if (list.Count == 0)
			{
				return null;
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			SystemAssembly systemAssembly = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				SystemAssembly systemAssembly2 = list[i];
				TargetFramework targetFramework = Runtime.SystemAssemblyService.GetTargetFramework(systemAssembly2.Package.TargetFramework);
				if (targetFramework.IncludesFramework(systemAssembly.Package.TargetFramework))
				{
					systemAssembly = systemAssembly2;
				}
			}
			return systemAssembly;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001ED18 File Offset: 0x0001CF18
		private IEnumerable<SystemAssembly> FindNewerAssembliesSameName(string fullname)
		{
			AssemblyName reqName = AssemblyContext.ParseAssemblyName(fullname);
			foreach (KeyValuePair<string, SystemAssembly> pair in this.assemblyFullNameToAsm)
			{
				KeyValuePair<string, SystemAssembly> keyValuePair = pair;
				AssemblyName foundName = keyValuePair.Value.AssemblyName;
				if (reqName.Name == foundName.Name && (reqName.Version == null || reqName.Version.CompareTo(foundName.Version) < 0))
				{
					KeyValuePair<string, SystemAssembly> keyValuePair2 = pair;
					for (SystemAssembly asm = keyValuePair2.Value; asm != null; asm = asm.NextSameName)
					{
						yield return asm;
					}
				}
			}
			yield break;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001ED3C File Offset: 0x0001CF3C
		public string GetAssemblyLocation(string assemblyName, TargetFramework fx)
		{
			return this.GetAssemblyLocation(assemblyName, null, fx);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0001ED48 File Offset: 0x0001CF48
		public virtual string GetAssemblyLocation(string assemblyName, string package, TargetFramework fx)
		{
			this.Initialize();
			assemblyName = AssemblyContext.NormalizeAsmName(assemblyName);
			SystemAssembly assemblyFromFullName = this.GetAssemblyFromFullName(assemblyName, package, fx);
			if (assemblyFromFullName != null)
			{
				return assemblyFromFullName.Location;
			}
			return null;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0001ED78 File Offset: 0x0001CF78
		public virtual bool AssemblyIsInGac(string aname)
		{
			return false;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0001EE1C File Offset: 0x0001D01C
		protected virtual IEnumerable<string> GetAssemblyDirectories()
		{
			yield break;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0001EE3C File Offset: 0x0001D03C
		public static void ParseAssemblyName(string assemblyName, out string name, out string version, out string culture, out string token)
		{
			string text;
			token = (text = null);
			string text2;
			culture = (text2 = text);
			string text3;
			version = (text3 = text2);
			name = text3;
			string[] array = assemblyName.Split(new char[]
			{
				','
			});
			if (array.Length < 1)
			{
				return;
			}
			name = array[0].Trim();
			if (array.Length < 2)
			{
				return;
			}
			int num = array[1].IndexOf('=');
			version = ((num != -1) ? array[1].Substring(num + 1).Trim() : array[1].Trim());
			if (array.Length < 3)
			{
				return;
			}
			num = array[2].IndexOf('=');
			culture = ((num != -1) ? array[2].Substring(num + 1).Trim() : array[2].Trim());
			if (culture == "neutral")
			{
				culture = "";
			}
			if (array.Length < 4)
			{
				return;
			}
			num = array[3].IndexOf('=');
			token = ((num != -1) ? array[3].Substring(num + 1).Trim() : array[3].Trim());
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0001EF32 File Offset: 0x0001D132
		public string GetAssemblyNameForVersion(string fullName, TargetFramework fx)
		{
			return this.GetAssemblyNameForVersion(fullName, null, fx);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0001EF40 File Offset: 0x0001D140
		public string GetAssemblyNameForVersion(string fullName, string packageName, TargetFramework fx)
		{
			SystemAssembly assemblyForVersion = this.GetAssemblyForVersion(fullName, packageName, fx);
			if (assemblyForVersion != null)
			{
				return assemblyForVersion.FullName;
			}
			return null;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001EFB4 File Offset: 0x0001D1B4
		public SystemAssembly GetAssemblyForVersion(string fullName, string packageName, TargetFramework fx)
		{
			this.Initialize();
			fullName = AssemblyContext.NormalizeAsmName(fullName);
			SystemAssembly assemblyFromFullName = this.GetAssemblyFromFullName(fullName, packageName, null);
			if (assemblyFromFullName == null)
			{
				return null;
			}
			List<SystemAssembly> list = (from a in assemblyFromFullName.AllSameName()
			where a.Package.IsFrameworkPackage
			select a).ToList<SystemAssembly>();
			if (!list.Any<SystemAssembly>())
			{
				if (fx.CanReferenceAssembliesTargetingFramework(assemblyFromFullName.Package.TargetFramework))
				{
					return assemblyFromFullName;
				}
				return null;
			}
			else
			{
				SystemAssembly systemAssembly = AssemblyContext.BestFrameworkAssembly(list, fx);
				if (systemAssembly != null)
				{
					return systemAssembly;
				}
				string fname = Path.GetFileName(list.First<SystemAssembly>().Location);
				List<SystemAssembly> list2 = (from a in (from p in this.packages
				where p.IsFrameworkPackage && fx.IncludesFramework(p.TargetFramework)
				select p).SelectMany((SystemPackage p) => p.Assemblies)
				where Path.GetFileName(a.Location) == fname
				select a).ToList<SystemAssembly>();
				return AssemblyContext.BestFrameworkAssembly(list2);
			}
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001F0C4 File Offset: 0x0001D2C4
		public string GetAssemblyFullName(string assemblyName, TargetFramework fx)
		{
			this.Initialize();
			assemblyName = AssemblyContext.NormalizeAsmName(assemblyName);
			if (this.assemblyFullNameToAsm.ContainsKey(assemblyName))
			{
				return assemblyName;
			}
			foreach (SystemAssembly systemAssembly in this.GetAssemblies(fx))
			{
				if (systemAssembly.Package.IsGacPackage && systemAssembly.Name == assemblyName)
				{
					return systemAssembly.FullName;
				}
			}
			if (File.Exists(assemblyName))
			{
				return SystemAssemblyService.GetAssemblyName(assemblyName);
			}
			string assemblyLocation = this.GetAssemblyLocation(assemblyName, fx);
			if (assemblyLocation != null)
			{
				return SystemAssemblyService.GetAssemblyName(assemblyLocation);
			}
			return null;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001F174 File Offset: 0x0001D374
		protected virtual void Initialize()
		{
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0001F178 File Offset: 0x0001D378
		internal SystemAssembly AddAssembly(string assemblyfile, AssemblyInfo ainfo, SystemPackage package)
		{
			if (!File.Exists(assemblyfile))
			{
				return null;
			}
			SystemAssembly result;
			try
			{
				SystemAssembly systemAssembly = SystemAssembly.FromFile(assemblyfile, ainfo);
				SystemAssembly systemAssembly2;
				if (this.assemblyFullNameToAsm.TryGetValue(systemAssembly.FullName, out systemAssembly2))
				{
					systemAssembly.NextSameName = systemAssembly2.NextSameName;
					systemAssembly2.NextSameName = systemAssembly;
				}
				else
				{
					this.assemblyFullNameToAsm[systemAssembly.FullName] = systemAssembly;
				}
				this.assemblyPathToPackage[assemblyfile] = package;
				result = systemAssembly;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001F1FC File Offset: 0x0001D3FC
		private void RemoveAssembly(SystemAssembly asm)
		{
			SystemAssembly nextSameName;
			if (!this.assemblyFullNameToAsm.TryGetValue(asm.FullName, out nextSameName))
			{
				return;
			}
			this.assemblyPathToPackage.Remove(asm.Location);
			SystemAssembly systemAssembly = null;
			while (nextSameName != asm)
			{
				systemAssembly = nextSameName;
				nextSameName = nextSameName.NextSameName;
				if (nextSameName == null)
				{
					return;
				}
			}
			if (systemAssembly != null)
			{
				systemAssembly.NextSameName = nextSameName.NextSameName;
				return;
			}
			if (nextSameName.NextSameName != null)
			{
				this.assemblyFullNameToAsm[asm.FullName] = nextSameName.NextSameName;
				return;
			}
			this.assemblyFullNameToAsm.Remove(asm.FullName);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001F288 File Offset: 0x0001D488
		internal void InternalAddPackage(SystemPackage package)
		{
			SystemPackage systemPackage;
			if (package.IsFrameworkPackage && !string.IsNullOrEmpty(package.Name) && this.packagesHash.TryGetValue(package.Name, out systemPackage) && !systemPackage.IsFrameworkPackage)
			{
				this.ForceUnregisterPackage(systemPackage);
			}
			this.packagesHash[package.Name] = package;
			this.packages.Add(package);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001F2EC File Offset: 0x0001D4EC
		public static AssemblyName ParseAssemblyName(string fullname)
		{
			AssemblyName assemblyName = new AssemblyName();
			int num = fullname.IndexOf(',');
			if (num == -1)
			{
				assemblyName.Name = fullname.Trim();
				return assemblyName;
			}
			assemblyName.Name = fullname.Substring(0, num).Trim();
			num = fullname.IndexOf("Version", num + 1);
			if (num == -1)
			{
				return assemblyName;
			}
			num = fullname.IndexOf('=', num);
			if (num == -1)
			{
				return assemblyName;
			}
			int num2 = fullname.IndexOf(',', num);
			if (num2 == -1)
			{
				assemblyName.Version = new Version(fullname.Substring(num + 1).Trim());
			}
			else
			{
				assemblyName.Version = new Version(fullname.Substring(num + 1, num2 - num - 1).Trim());
			}
			return assemblyName;
		}

		// Token: 0x0400026F RID: 623
		private Dictionary<string, SystemPackage> assemblyPathToPackage = new Dictionary<string, SystemPackage>();

		// Token: 0x04000270 RID: 624
		private Dictionary<string, SystemAssembly> assemblyFullNameToAsm = new Dictionary<string, SystemAssembly>();

		// Token: 0x04000271 RID: 625
		private Dictionary<string, SystemPackage> packagesHash = new Dictionary<string, SystemPackage>();

		// Token: 0x04000272 RID: 626
		private List<SystemPackage> packages = new List<SystemPackage>();
	}
}
