using System;
using System.Collections.Generic;
using System.Xml;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Projects
{
	// Token: 0x02000182 RID: 386
	[ProjectModelDataItem("DotNetProject")]
	public class DotNetAssemblyProject : DotNetProject
	{
		// Token: 0x06000F2D RID: 3885 RVA: 0x00038E6A File Offset: 0x0003706A
		public DotNetAssemblyProject()
		{
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00038E72 File Offset: 0x00037072
		public DotNetAssemblyProject(string languageName) : base(languageName)
		{
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00038E7B File Offset: 0x0003707B
		public DotNetAssemblyProject(string languageName, ProjectCreateInformation projectCreateInfo, XmlElement projectOptions) : base(languageName, projectCreateInfo, projectOptions)
		{
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00039034 File Offset: 0x00037234
		public override IEnumerable<string> GetProjectTypes()
		{
			yield return "DotNetAssembly";
			foreach (string pt in base.GetProjectTypes())
			{
				yield return pt;
			}
			yield break;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00039051 File Offset: 0x00037251
		public override bool SupportsFramework(TargetFramework framework)
		{
			return framework.CanReferenceAssembliesTargetingFramework(TargetFrameworkMoniker.NET_1_1) && base.SupportsFramework(framework);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0003906C File Offset: 0x0003726C
		public override TargetFrameworkMoniker GetDefaultTargetFrameworkForFormat(FileFormat format)
		{
			string id;
			if ((id = format.Id) != null)
			{
				if (id == "MSBuild05")
				{
					return TargetFrameworkMoniker.NET_2_0;
				}
				if (id == "MSBuild08")
				{
					return TargetFrameworkMoniker.NET_2_0;
				}
				if (id == "MSBuild10" || id == "MSBuild12")
				{
					return TargetFrameworkMoniker.NET_4_0;
				}
			}
			return Services.ProjectService.DefaultTargetFramework.Id;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x000390DC File Offset: 0x000372DC
		protected override string GetDefaultTargetPlatform(ProjectCreateInformation projectCreateInfo)
		{
			if (base.CompileTarget == CompileTarget.Library)
			{
				return string.Empty;
			}
			if (projectCreateInfo.ParentFolder != null && projectCreateInfo.ParentFolder.ParentSolution != null)
			{
				ItemConfiguration configuration = projectCreateInfo.ParentFolder.ParentSolution.GetConfiguration(projectCreateInfo.ActiveConfiguration);
				if (configuration != null)
				{
					return configuration.Platform;
				}
				string text = null;
				string id = projectCreateInfo.ActiveConfiguration.ToString();
				string b;
				string text2;
				ItemConfiguration.ParseConfigurationId(id, out b, out text2);
				foreach (ItemConfiguration itemConfiguration in projectCreateInfo.ParentFolder.ParentSolution.Configurations)
				{
					if (itemConfiguration.Platform == text2)
					{
						return text2;
					}
					if (itemConfiguration.Name == b)
					{
						text = itemConfiguration.Platform;
					}
				}
				if (text != null)
				{
					return text;
				}
			}
			return Services.ProjectService.DefaultPlatformTarget;
		}
	}
}
