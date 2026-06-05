using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001C6 RID: 454
	internal class SlnData
	{
		// Token: 0x0600114E RID: 4430 RVA: 0x00046458 File Offset: 0x00044658
		public void UpdateVersion(MSBuildFileFormat format)
		{
			this.VersionString = format.SlnVersion;
			this.headerComment = "# " + format.ProductDescription;
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x0004647C File Offset: 0x0004467C
		// (set) Token: 0x06001150 RID: 4432 RVA: 0x00046484 File Offset: 0x00044684
		public string HeaderComment
		{
			get
			{
				return this.headerComment;
			}
			set
			{
				this.headerComment = value;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x0004648D File Offset: 0x0004468D
		// (set) Token: 0x06001152 RID: 4434 RVA: 0x00046495 File Offset: 0x00044695
		public string VersionString { get; set; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0004649E File Offset: 0x0004469E
		// (set) Token: 0x06001154 RID: 4436 RVA: 0x000464A6 File Offset: 0x000446A6
		public Version VisualStudioVersion { get; set; }

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x000464AF File Offset: 0x000446AF
		// (set) Token: 0x06001156 RID: 4438 RVA: 0x000464B7 File Offset: 0x000446B7
		public Version MinimumVisualStudioVersion { get; set; }

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x000464C0 File Offset: 0x000446C0
		public Dictionary<SolutionConfiguration, string> ConfigStrings
		{
			get
			{
				if (this.configStrings == null)
				{
					this.configStrings = new Dictionary<SolutionConfiguration, string>();
				}
				return this.configStrings;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x000464DB File Offset: 0x000446DB
		// (set) Token: 0x06001159 RID: 4441 RVA: 0x000464E3 File Offset: 0x000446E3
		public List<string> GlobalExtra
		{
			get
			{
				return this.globalExtra;
			}
			set
			{
				this.globalExtra = value;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x000464EC File Offset: 0x000446EC
		// (set) Token: 0x0600115B RID: 4443 RVA: 0x000464F4 File Offset: 0x000446F4
		public string[] Extra
		{
			get
			{
				return this.extra;
			}
			set
			{
				this.extra = value;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x000464FD File Offset: 0x000446FD
		public List<string> UnknownProjects
		{
			get
			{
				if (this.unknownProjects == null)
				{
					this.unknownProjects = new List<string>();
				}
				return this.unknownProjects;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x00046518 File Offset: 0x00044718
		public Dictionary<string, List<string>> SectionExtras
		{
			get
			{
				if (this.sectionExtras == null)
				{
					this.sectionExtras = new Dictionary<string, List<string>>();
				}
				return this.sectionExtras;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00046533 File Offset: 0x00044733
		public Dictionary<string, SolutionEntityItem> ItemsByGuid
		{
			get
			{
				if (this.projectsByGuidTable == null)
				{
					this.projectsByGuidTable = new Dictionary<string, SolutionEntityItem>();
				}
				return this.projectsByGuidTable;
			}
		}

		// Token: 0x04000500 RID: 1280
		private string headerComment = "# MonoDevelop";

		// Token: 0x04000501 RID: 1281
		private Dictionary<SolutionConfiguration, string> configStrings;

		// Token: 0x04000502 RID: 1282
		private List<string> globalExtra;

		// Token: 0x04000503 RID: 1283
		private Dictionary<string, List<string>> sectionExtras;

		// Token: 0x04000504 RID: 1284
		private string[] extra;

		// Token: 0x04000505 RID: 1285
		private List<string> unknownProjects;

		// Token: 0x04000506 RID: 1286
		private Dictionary<string, SolutionEntityItem> projectsByGuidTable;
	}
}
