using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x0200009C RID: 156
	public class SystemPackage
	{
		// Token: 0x06000502 RID: 1282 RVA: 0x0001143D File Offset: 0x0000F63D
		internal SystemPackage()
		{
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00011448 File Offset: 0x0000F648
		internal void Initialize(SystemPackageInfo info, IEnumerable<SystemAssembly> assemblies, bool isInternal)
		{
			this.isInternal = isInternal;
			this.name = (info.Name ?? string.Empty);
			this.version = (info.Version ?? string.Empty);
			this.description = (info.Description ?? string.Empty);
			this.targetFramework = info.TargetFramework;
			this.gacRoot = info.GacRoot;
			this.gacPackage = info.IsGacPackage;
			this.IsFrameworkPackage = info.IsFrameworkPackage;
			this.IsCorePackage = info.IsCorePackage;
			this.Requires = info.Requires;
			SystemAssembly systemAssembly = null;
			foreach (SystemAssembly systemAssembly2 in assemblies)
			{
				if (systemAssembly2 != null)
				{
					systemAssembly2.Package = this;
					if (this.assemblies == null)
					{
						this.assemblies = systemAssembly2;
					}
					else
					{
						systemAssembly.NextSamePackage = systemAssembly2;
					}
					systemAssembly = systemAssembly2;
				}
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00011540 File Offset: 0x0000F740
		public string GetDisplayName()
		{
			if (!this.IsFrameworkPackage && !string.IsNullOrEmpty(this.version))
			{
				return this.name + " " + this.version;
			}
			return this.Name;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00011574 File Offset: 0x0000F774
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x0001157C File Offset: 0x0000F77C
		public string GacRoot
		{
			get
			{
				return this.gacRoot;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00011584 File Offset: 0x0000F784
		public bool IsGacPackage
		{
			get
			{
				return this.gacPackage;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0001158C File Offset: 0x0000F78C
		public string Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00011594 File Offset: 0x0000F794
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0001159C File Offset: 0x0000F79C
		public TargetFrameworkMoniker TargetFramework
		{
			get
			{
				return this.targetFramework ?? TargetFrameworkMoniker.NET_1_1;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x000115AD File Offset: 0x0000F7AD
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x000115B5 File Offset: 0x0000F7B5
		public string Requires { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x000115BE File Offset: 0x0000F7BE
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x000115C6 File Offset: 0x0000F7C6
		internal string BaseTargetFramework { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x000115CF File Offset: 0x0000F7CF
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x000115D7 File Offset: 0x0000F7D7
		public bool IsCorePackage { get; internal set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x000115E0 File Offset: 0x0000F7E0
		public bool IsInternalPackage
		{
			get
			{
				return this.isInternal;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x000115E8 File Offset: 0x0000F7E8
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x000115F0 File Offset: 0x0000F7F0
		public bool IsFrameworkPackage { get; internal set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x000116F0 File Offset: 0x0000F8F0
		public IEnumerable<SystemAssembly> Assemblies
		{
			get
			{
				for (SystemAssembly asm = this.assemblies; asm != null; asm = asm.NextSamePackage)
				{
					yield return asm;
				}
				yield break;
			}
		}

		// Token: 0x040001A0 RID: 416
		private string name;

		// Token: 0x040001A1 RID: 417
		private string version;

		// Token: 0x040001A2 RID: 418
		private string description;

		// Token: 0x040001A3 RID: 419
		private SystemAssembly assemblies;

		// Token: 0x040001A4 RID: 420
		private bool isInternal;

		// Token: 0x040001A5 RID: 421
		private TargetFrameworkMoniker targetFramework;

		// Token: 0x040001A6 RID: 422
		private string gacRoot;

		// Token: 0x040001A7 RID: 423
		private bool gacPackage;
	}
}
