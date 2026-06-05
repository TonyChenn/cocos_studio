using System;
using System.Globalization;
using System.Reflection;

namespace Mono.PkgConfig
{
	// Token: 0x020000DC RID: 220
	internal class PackageAssemblyInfo
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0002041D File Offset: 0x0001E61D
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x00020425 File Offset: 0x0001E625
		public string File { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00020430 File Offset: 0x0001E630
		public string FullName
		{
			get
			{
				string text = this.Name + ", Version=" + this.Version;
				if (!string.IsNullOrEmpty(this.Culture))
				{
					text = text + ", Culture=" + this.Culture;
				}
				if (!string.IsNullOrEmpty(this.PublicKeyToken))
				{
					text = text + ", PublicKeyToken=" + this.PublicKeyToken;
				}
				return text;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00020493 File Offset: 0x0001E693
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0002049B File Offset: 0x0001E69B
		public LibraryPackageInfo ParentPackage { get; set; }

		// Token: 0x060007DB RID: 2011 RVA: 0x000204A4 File Offset: 0x0001E6A4
		public void UpdateFromFile(string file)
		{
			this.Update(AssemblyName.GetAssemblyName(file));
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x000204B4 File Offset: 0x0001E6B4
		public void Update(AssemblyName aname)
		{
			this.Name = aname.Name;
			this.Version = aname.Version.ToString();
			if (aname.CultureInfo != null)
			{
				if (aname.CultureInfo.LCID == CultureInfo.InvariantCulture.LCID)
				{
					this.Culture = "neutral";
				}
				else
				{
					this.Culture = aname.CultureInfo.Name;
				}
			}
			string text = aname.ToString();
			string text2 = "publickeytoken=";
			int num = text.ToLower().IndexOf(text2) + text2.Length;
			int num2 = text.IndexOf(',', num);
			if (num2 == -1)
			{
				num2 = text.Length;
			}
			this.PublicKeyToken = text.Substring(num, num2 - num);
		}

		// Token: 0x0400027F RID: 639
		public string Name;

		// Token: 0x04000280 RID: 640
		public string Version;

		// Token: 0x04000281 RID: 641
		public string Culture;

		// Token: 0x04000282 RID: 642
		public string PublicKeyToken;
	}
}
