using System;

namespace MonoDevelop.Projects.SharedAssetsProjects
{
	// Token: 0x02000250 RID: 592
	internal static class SharedAssetsProjectExtensions
	{
		// Token: 0x060015DF RID: 5599 RVA: 0x000587F6 File Offset: 0x000569F6
		public static string GetItemsProjectPath(this ProjectReference r)
		{
			return (string)r.ExtendedProperties["MSBuild.SharedAssetsProject"];
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x0005880D File Offset: 0x00056A0D
		public static void SetItemsProjectPath(this ProjectReference r, string path)
		{
			r.ExtendedProperties["MSBuild.SharedAssetsProject"] = path;
		}
	}
}
