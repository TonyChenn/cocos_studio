using System;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001B0 RID: 432
	internal class MSBuildFileFormatVS08 : MSBuildFileFormat
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x0003C812 File Offset: 0x0003AA12
		public override string Id
		{
			get
			{
				return "MSBuild08";
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x0003C819 File Offset: 0x0003AA19
		public override string DefaultProductVersion
		{
			get
			{
				return "9.0.21022";
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x0003C820 File Offset: 0x0003AA20
		public override string DefaultToolsVersion
		{
			get
			{
				return "3.5";
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0003C827 File Offset: 0x0003AA27
		public override string DefaultSchemaVersion
		{
			get
			{
				return "2.0";
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0003C82E File Offset: 0x0003AA2E
		public override string SlnVersion
		{
			get
			{
				return "10.00";
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x0003C835 File Offset: 0x0003AA35
		public override string ProductDescription
		{
			get
			{
				return "Visual Studio 2008";
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x0003C83C File Offset: 0x0003AA3C
		public override TargetFrameworkMoniker[] SupportedFrameworks
		{
			get
			{
				return MSBuildFileFormatVS08.supportedFrameworks;
			}
		}

		// Token: 0x040004AD RID: 1197
		private static readonly TargetFrameworkMoniker[] supportedFrameworks = new TargetFrameworkMoniker[]
		{
			TargetFrameworkMoniker.NET_2_0,
			TargetFrameworkMoniker.NET_3_0,
			TargetFrameworkMoniker.NET_3_5,
			TargetFrameworkMoniker.SL_2_0,
			TargetFrameworkMoniker.SL_3_0,
			TargetFrameworkMoniker.MONOTOUCH_1_0
		};
	}
}
