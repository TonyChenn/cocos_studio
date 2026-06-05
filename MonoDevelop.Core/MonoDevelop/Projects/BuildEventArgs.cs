using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x0200014E RID: 334
	public class BuildEventArgs : EventArgs
	{
		// Token: 0x06000C5B RID: 3163 RVA: 0x0002DDF2 File Offset: 0x0002BFF2
		public BuildEventArgs(IProgressMonitor monitor, bool success)
		{
			this.monitor = monitor;
			this.success = success;
			this.WarningCount = -1;
			this.ErrorCount = -1;
			this.BuildCount = -1;
			this.FailedBuildCount = -1;
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x0002DE24 File Offset: 0x0002C024
		public IProgressMonitor ProgressMonitor
		{
			get
			{
				return this.monitor;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0002DE2C File Offset: 0x0002C02C
		public bool Success
		{
			get
			{
				return this.success;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x0002DE34 File Offset: 0x0002C034
		// (set) Token: 0x06000C5F RID: 3167 RVA: 0x0002DE3C File Offset: 0x0002C03C
		public int WarningCount { get; set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x0002DE45 File Offset: 0x0002C045
		// (set) Token: 0x06000C61 RID: 3169 RVA: 0x0002DE4D File Offset: 0x0002C04D
		public int ErrorCount { get; set; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0002DE56 File Offset: 0x0002C056
		// (set) Token: 0x06000C63 RID: 3171 RVA: 0x0002DE5E File Offset: 0x0002C05E
		public int BuildCount { get; set; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x0002DE67 File Offset: 0x0002C067
		// (set) Token: 0x06000C65 RID: 3173 RVA: 0x0002DE6F File Offset: 0x0002C06F
		public int FailedBuildCount { get; set; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x0002DE78 File Offset: 0x0002C078
		// (set) Token: 0x06000C67 RID: 3175 RVA: 0x0002DE80 File Offset: 0x0002C080
		public SolutionItem SolutionItem { get; set; }

		// Token: 0x040003B0 RID: 944
		private IProgressMonitor monitor;

		// Token: 0x040003B1 RID: 945
		private bool success;
	}
}
