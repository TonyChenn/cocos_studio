using System;
using System.Reflection;
using Mono.PkgConfig;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000A5 RID: 165
	internal class AssemblyInfo
	{
		// Token: 0x060005A6 RID: 1446 RVA: 0x00014950 File Offset: 0x00012B50
		public AssemblyInfo()
		{
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001495F File Offset: 0x00012B5F
		public AssemblyInfo(PackageAssemblyInfo info)
		{
			this.Name = info.Name;
			this.Version = info.Version;
			this.PublicKeyToken = info.PublicKeyToken;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00014992 File Offset: 0x00012B92
		public void UpdateFromFile(string file)
		{
			this.Update(SystemAssemblyService.GetAssemblyNameObj(file));
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000149A0 File Offset: 0x00012BA0
		public void Update(AssemblyName aname)
		{
			this.Name = aname.Name;
			this.Version = aname.Version.ToString();
			this.ProcessorArchitecture = aname.ProcessorArchitecture;
			this.Culture = aname.CultureInfo.Name;
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

		// Token: 0x060005AA RID: 1450 RVA: 0x00014A2D File Offset: 0x00012C2D
		public AssemblyInfo Clone()
		{
			return (AssemblyInfo)base.MemberwiseClone();
		}

		// Token: 0x040001DD RID: 477
		[ItemProperty("name")]
		public string Name;

		// Token: 0x040001DE RID: 478
		[ItemProperty("version")]
		public string Version;

		// Token: 0x040001DF RID: 479
		[ItemProperty("publicKeyToken", DefaultValue = "null")]
		public string PublicKeyToken;

		// Token: 0x040001E0 RID: 480
		[ItemProperty("package")]
		public string Package;

		// Token: 0x040001E1 RID: 481
		[ItemProperty("culture")]
		public string Culture;

		// Token: 0x040001E2 RID: 482
		[ItemProperty("processorArchitecture")]
		public ProcessorArchitecture ProcessorArchitecture = ProcessorArchitecture.MSIL;

		// Token: 0x040001E3 RID: 483
		[ItemProperty("inGac")]
		public bool InGac;
	}
}
