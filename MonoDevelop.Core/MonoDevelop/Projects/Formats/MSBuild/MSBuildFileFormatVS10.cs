using System;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001B1 RID: 433
	internal class MSBuildFileFormatVS10 : MSBuildFileFormat
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0003C896 File Offset: 0x0003AA96
		public override string Id
		{
			get
			{
				return "MSBuild10";
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x0003C89D File Offset: 0x0003AA9D
		public override string DefaultProductVersion
		{
			get
			{
				return "8.0.30703";
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x0003C8A4 File Offset: 0x0003AAA4
		public override string DefaultSchemaVersion
		{
			get
			{
				return "2.0";
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x0003C8AB File Offset: 0x0003AAAB
		public override string DefaultToolsVersion
		{
			get
			{
				return "4.0";
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x0003C8B2 File Offset: 0x0003AAB2
		public override string SlnVersion
		{
			get
			{
				return "11.00";
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x0003C8B9 File Offset: 0x0003AAB9
		public override string ProductDescription
		{
			get
			{
				return "Visual Studio 2010";
			}
		}
	}
}
