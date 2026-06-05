using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000175 RID: 373
	public static class BuildAction
	{
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00035F60 File Offset: 0x00034160
		public static string[] StandardActions
		{
			get
			{
				return new string[]
				{
					"None",
					"Compile"
				};
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x00035F88 File Offset: 0x00034188
		public static string[] DotNetCommonActions
		{
			get
			{
				return new string[]
				{
					"None",
					"Compile",
					"EmbeddedResource"
				};
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00035FB8 File Offset: 0x000341B8
		public static string[] DotNetActions
		{
			get
			{
				return new string[]
				{
					"None",
					"Compile",
					"Content",
					"EmbeddedResource"
				};
			}
		}

		// Token: 0x04000433 RID: 1075
		public const string None = "None";

		// Token: 0x04000434 RID: 1076
		public const string Compile = "Compile";

		// Token: 0x04000435 RID: 1077
		public const string EmbeddedResource = "EmbeddedResource";

		// Token: 0x04000436 RID: 1078
		public const string Content = "Content";

		// Token: 0x04000437 RID: 1079
		public const string ApplicationDefinition = "ApplicationDefinition";

		// Token: 0x04000438 RID: 1080
		public const string Page = "Page";

		// Token: 0x04000439 RID: 1081
		public const string InterfaceDefinition = "InterfaceDefinition";

		// Token: 0x0400043A RID: 1082
		public const string BundleResource = "BundleResource";

		// Token: 0x0400043B RID: 1083
		public const string AtlasResource = "AtlasResource";

		// Token: 0x0400043C RID: 1084
		public const string Resource = "Resource";

		// Token: 0x0400043D RID: 1085
		public const string SplashScreen = "SplashScreen";

		// Token: 0x0400043E RID: 1086
		public const string EntityDeploy = "EntityDeploy";
	}
}
