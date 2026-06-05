using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Projects
{
	// Token: 0x0200023C RID: 572
	public class PortableDotNetProject : DotNetProject
	{
		// Token: 0x0600152D RID: 5421 RVA: 0x000569D0 File Offset: 0x00054BD0
		public PortableDotNetProject()
		{
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x000569D8 File Offset: 0x00054BD8
		public PortableDotNetProject(string languageName) : base(languageName)
		{
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x000569E1 File Offset: 0x00054BE1
		public PortableDotNetProject(string languageName, ProjectCreateInformation projectCreateInfo, XmlElement projectOptions) : base(languageName, projectCreateInfo, projectOptions)
		{
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00056B98 File Offset: 0x00054D98
		public override IEnumerable<string> GetProjectTypes()
		{
			yield return "PortableDotNet";
			foreach (string t in base.GetProjectTypes())
			{
				yield return t;
			}
			yield break;
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x00056BB8 File Offset: 0x00054DB8
		public override bool SupportsFormat(FileFormat format)
		{
			int num;
			return format.Id.StartsWith("MSBuild", StringComparison.Ordinal) && int.TryParse(format.Id.Substring("MSBuild".Length), out num) && num >= 10;
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00056C02 File Offset: 0x00054E02
		public override bool SupportsFramework(TargetFramework framework)
		{
			return framework.Id.Identifier == ".NETPortable";
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x00056C19 File Offset: 0x00054E19
		public override TargetFrameworkMoniker GetDefaultTargetFrameworkForFormat(FileFormat format)
		{
			return new TargetFrameworkMoniker(".NETPortable", "1.0");
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x00056C2A File Offset: 0x00054E2A
		public override TargetFrameworkMoniker GetDefaultTargetFrameworkId()
		{
			return new TargetFrameworkMoniker(".NETPortable", "4.5", "Profile78");
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x00056C58 File Offset: 0x00054E58
		protected internal override IEnumerable<string> OnGetReferencedAssemblies(ConfigurationSelector configuration, bool includeProjectReferences)
		{
			IEnumerable<string> first = base.OnGetReferencedAssemblies(configuration, includeProjectReferences);
			IEnumerable<string> second = from a in base.TargetRuntime.AssemblyContext.GetAssemblies(base.TargetFramework)
			where a.Package.IsFrameworkPackage
			select a.Location;
			return first.Concat(second).Distinct<string>();
		}
	}
}
