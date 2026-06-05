using System;
using System.Collections.Generic;

namespace Mono.PkgConfig
{
	// Token: 0x020000DB RID: 219
	internal class LibraryPackageInfo : PackageInfo
	{
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x000203B2 File Offset: 0x0001E5B2
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x000203C9 File Offset: 0x0001E5C9
		public bool IsGacPackage
		{
			get
			{
				return base.GetData("gacPackage") != "false";
			}
			set
			{
				if (value)
				{
					base.RemoveData("gacPackage");
					return;
				}
				base.SetData("gacPackage", "false");
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x000203EA File Offset: 0x0001E5EA
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x000203F2 File Offset: 0x0001E5F2
		internal List<PackageAssemblyInfo> Assemblies { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x000203FB File Offset: 0x0001E5FB
		protected internal override bool IsValidPackage
		{
			get
			{
				return this.Assemblies != null && this.Assemblies.Count > 0;
			}
		}
	}
}
