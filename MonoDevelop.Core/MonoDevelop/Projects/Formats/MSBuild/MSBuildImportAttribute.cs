using System;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001D5 RID: 469
	[AttributeUsage(AttributeTargets.Class)]
	public class MSBuildImportAttribute : Attribute
	{
		// Token: 0x060011E5 RID: 4581 RVA: 0x0004913A File Offset: 0x0004733A
		public MSBuildImportAttribute(string project)
		{
			this.Project = project;
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00049149 File Offset: 0x00047349
		// (set) Token: 0x060011E7 RID: 4583 RVA: 0x00049151 File Offset: 0x00047351
		public string Project { get; set; }
	}
}
