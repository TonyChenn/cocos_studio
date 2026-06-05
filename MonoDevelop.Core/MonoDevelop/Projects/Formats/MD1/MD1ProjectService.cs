using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Formats.MD1
{
	// Token: 0x0200024C RID: 588
	internal static class MD1ProjectService
	{
		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x00057B9E File Offset: 0x00055D9E
		public static DataContext DataContext
		{
			get
			{
				if (MD1ProjectService.dataContext == null)
				{
					MD1ProjectService.dataContext = new DataContext();
					Services.ProjectService.InitializeDataContext(MD1ProjectService.dataContext);
				}
				return MD1ProjectService.dataContext;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600159F RID: 5535 RVA: 0x00057BC5 File Offset: 0x00055DC5
		public static FileFormat FileFormat
		{
			get
			{
				return Services.ProjectService.FileFormats.GetFileFormat("MD1");
			}
		}

		// Token: 0x04000687 RID: 1671
		private static DataContext dataContext;
	}
}
