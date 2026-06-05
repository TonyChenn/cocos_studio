using System;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001B2 RID: 434
	internal class MSBuildFileFormatVS12 : MSBuildFileFormat
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x0003C8C8 File Offset: 0x0003AAC8
		public override string Id
		{
			get
			{
				return "MSBuild12";
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x0003C8CF File Offset: 0x0003AACF
		public override string DefaultToolsVersion
		{
			get
			{
				return "4.0";
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x0003C8D6 File Offset: 0x0003AAD6
		public override string SlnVersion
		{
			get
			{
				return "12.00";
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x0003C8DD File Offset: 0x0003AADD
		public override string ProductDescription
		{
			get
			{
				return "Visual Studio 2012";
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0003C8E4 File Offset: 0x0003AAE4
		protected override bool SupportsToolsVersion(string version)
		{
			return version == "4.0" || version == "12.0";
		}
	}
}
