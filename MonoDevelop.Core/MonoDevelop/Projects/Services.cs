using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000159 RID: 345
	public class Services
	{
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0002EDE3 File Offset: 0x0002CFE3
		public static ProjectService ProjectService
		{
			get
			{
				if (Services.projectService == null)
				{
					Services.projectService = new ProjectService();
					if (Services.ProjectServiceLoaded != null)
					{
						Services.ProjectServiceLoaded(Services.projectService, EventArgs.Empty);
					}
				}
				return Services.projectService;
			}
		}

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06000CB0 RID: 3248 RVA: 0x0002EE18 File Offset: 0x0002D018
		// (remove) Token: 0x06000CB1 RID: 3249 RVA: 0x0002EE4C File Offset: 0x0002D04C
		public static event EventHandler ProjectServiceLoaded;

		// Token: 0x040003CD RID: 973
		private static ProjectService projectService;
	}
}
