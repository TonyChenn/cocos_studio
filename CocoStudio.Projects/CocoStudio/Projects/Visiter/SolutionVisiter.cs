using System;
using CocoStudio.Model;
using Gdk;

namespace CocoStudio.Projects.Visiter
{
	// Token: 0x0200008B RID: 139
	public static class SolutionVisiter
	{
		// Token: 0x06000447 RID: 1095 RVA: 0x0000E04C File Offset: 0x0000C24C
		public static SizeF GetSceneSize(this Solution solution)
		{
			string solutionSize = solution.Config.SolutionSize;
			if (!string.IsNullOrWhiteSpace(solutionSize))
			{
				return solutionSize.ConvertToSize();
			}
			return SizeF.Empty;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000E079 File Offset: 0x0000C279
		public static string GetSceneSizeString(this Solution solution)
		{
			return solution.Config.SolutionSize;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000E086 File Offset: 0x0000C286
		public static void SetSceneSize(this Solution solution, Size size)
		{
			solution.Config.SolutionSize = SolutionVisiter.ConvertToString(size);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000E099 File Offset: 0x0000C299
		public static void SetSceneSize(this Solution solution, string size)
		{
			solution.Config.SolutionSize = size;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000E0A8 File Offset: 0x0000C2A8
		private static SizeF ConvertToSize(this string sizeStr)
		{
			string[] array = sizeStr.Split(new char[]
			{
				'*'
			});
			float width = Convert.ToSingle(array[0]);
			float height = Convert.ToSingle(array[1]);
			return new SizeF(width, height);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000E0E2 File Offset: 0x0000C2E2
		private static string ConvertToString(Size size)
		{
			return string.Format("{0}*{1}", size.Width, size.Height);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000E106 File Offset: 0x0000C306
		public static string GetResolutionName(this Solution solution)
		{
			return solution.Config.ResolutionName;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000E113 File Offset: 0x0000C313
		public static void SetResolutionName(this Solution solution, string name)
		{
			solution.Config.ResolutionName = name;
		}
	}
}
