using System;
using System.IO;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x020001A5 RID: 421
	internal class DefaultResourceHandler : IResourceHandler
	{
		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003B08A File Offset: 0x0003928A
		public string GetDefaultResourceId(ProjectFile file)
		{
			return Path.GetFileName(file.Name);
		}

		// Token: 0x040004A7 RID: 1191
		public static readonly DefaultResourceHandler Instance = new DefaultResourceHandler();
	}
}
