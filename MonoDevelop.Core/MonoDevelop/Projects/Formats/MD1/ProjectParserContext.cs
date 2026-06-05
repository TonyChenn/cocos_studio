using System;
using Microsoft.Build.BuildEngine;

namespace MonoDevelop.Projects.Formats.MD1
{
	// Token: 0x020001AC RID: 428
	internal class ProjectParserContext : IExpressionContext
	{
		// Token: 0x0600100B RID: 4107 RVA: 0x0003BE69 File Offset: 0x0003A069
		public ProjectParserContext(Project project, DotNetProjectConfiguration config)
		{
			this.project = project;
			this.config = config;
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x0003BE7F File Offset: 0x0003A07F
		public string FullFileName
		{
			get
			{
				return this.project.FileName;
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0003BE94 File Offset: 0x0003A094
		public string EvaluateString(string value)
		{
			return value.Replace("$(Configuration)", this.config.Name).Replace("$(Platform)", this.config.Platform);
		}

		// Token: 0x040004A9 RID: 1193
		private Project project;

		// Token: 0x040004AA RID: 1194
		private DotNetProjectConfiguration config;
	}
}
