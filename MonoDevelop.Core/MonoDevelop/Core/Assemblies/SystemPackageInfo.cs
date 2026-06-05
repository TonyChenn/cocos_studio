using System;
using System.Collections.Generic;
using Mono.PkgConfig;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x0200009D RID: 157
	public class SystemPackageInfo
	{
		// Token: 0x06000515 RID: 1301 RVA: 0x0001170D File Offset: 0x0000F90D
		public SystemPackageInfo()
		{
			this.IsGacPackage = true;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001171C File Offset: 0x0000F91C
		internal SystemPackageInfo(LibraryPackageInfo info)
		{
			this.Name = info.Name;
			this.IsGacPackage = info.IsGacPackage;
			this.Version = info.Version;
			this.Description = info.Description;
			this.TargetFramework = TargetFrameworkMoniker.Parse(info.GetData("targetFramework"));
			this.CustomData = info.CustomData;
			this.Requires = info.Requires;
			this.Assemblies = new List<AssemblyInfo>();
			if (info.IsValidPackage)
			{
				foreach (PackageAssemblyInfo info2 in info.Assemblies)
				{
					this.Assemblies.Add(new AssemblyInfo(info2));
				}
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x000117F0 File Offset: 0x0000F9F0
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x000117F8 File Offset: 0x0000F9F8
		public string Name { get; set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00011801 File Offset: 0x0000FA01
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x00011809 File Offset: 0x0000FA09
		public string GacRoot { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00011812 File Offset: 0x0000FA12
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x0001181A File Offset: 0x0000FA1A
		public bool IsGacPackage { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00011823 File Offset: 0x0000FA23
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x0001182B File Offset: 0x0000FA2B
		public string Version { get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00011834 File Offset: 0x0000FA34
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x0001183C File Offset: 0x0000FA3C
		public string Description { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00011845 File Offset: 0x0000FA45
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x0001184D File Offset: 0x0000FA4D
		public TargetFrameworkMoniker TargetFramework { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00011856 File Offset: 0x0000FA56
		// (set) Token: 0x06000524 RID: 1316 RVA: 0x0001185E File Offset: 0x0000FA5E
		public string Requires { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00011867 File Offset: 0x0000FA67
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x0001186F File Offset: 0x0000FA6F
		public bool IsCorePackage { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00011878 File Offset: 0x0000FA78
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x00011880 File Offset: 0x0000FA80
		public bool IsFrameworkPackage { get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00011889 File Offset: 0x0000FA89
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x00011891 File Offset: 0x0000FA91
		internal List<AssemblyInfo> Assemblies { get; set; }

		// Token: 0x040001AC RID: 428
		internal Dictionary<string, string> CustomData;
	}
}
