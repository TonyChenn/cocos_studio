using System;
using System.Collections.Generic;
using System.Xml;

namespace MonoDevelop.Projects
{
	// Token: 0x02000154 RID: 340
	[ProjectModelDataItem]
	public class GenericProject : Project
	{
		// Token: 0x06000C9E RID: 3230 RVA: 0x0002E6E6 File Offset: 0x0002C8E6
		public GenericProject()
		{
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0002E6EE File Offset: 0x0002C8EE
		public GenericProject(ProjectCreateInformation info, XmlElement projectOptions)
		{
			base.Configurations.Add(this.CreateConfiguration("Default"));
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0002E70C File Offset: 0x0002C90C
		public override SolutionItemConfiguration CreateConfiguration(string name)
		{
			return new GenericProjectConfiguration(name);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002E7EC File Offset: 0x0002C9EC
		public override IEnumerable<string> GetProjectTypes()
		{
			yield return "GenericProject";
			yield break;
		}
	}
}
