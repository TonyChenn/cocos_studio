using System;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001AF RID: 431
	internal class MSBuildFileFormatVS05 : MSBuildFileFormat
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0003C7B7 File Offset: 0x0003A9B7
		public override string Id
		{
			get
			{
				return "MSBuild05";
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x0003C7BE File Offset: 0x0003A9BE
		public override string DefaultProductVersion
		{
			get
			{
				return "8.0.50727";
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x0003C7C5 File Offset: 0x0003A9C5
		public override string DefaultToolsVersion
		{
			get
			{
				return "2.0";
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x0003C7CC File Offset: 0x0003A9CC
		public override string DefaultSchemaVersion
		{
			get
			{
				return "2.0";
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x0003C7D3 File Offset: 0x0003A9D3
		public override string SlnVersion
		{
			get
			{
				return "9.00";
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x0003C7DA File Offset: 0x0003A9DA
		public override string ProductDescription
		{
			get
			{
				return "Visual Studio 2005";
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x0003C7E1 File Offset: 0x0003A9E1
		public override TargetFrameworkMoniker[] SupportedFrameworks
		{
			get
			{
				return MSBuildFileFormatVS05.supportedFrameworks;
			}
		}

		// Token: 0x040004AC RID: 1196
		private static readonly TargetFrameworkMoniker[] supportedFrameworks = new TargetFrameworkMoniker[]
		{
			TargetFrameworkMoniker.NET_2_0
		};
	}
}
