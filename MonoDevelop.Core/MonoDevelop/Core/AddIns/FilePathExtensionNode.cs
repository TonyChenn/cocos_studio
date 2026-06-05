using System;
using Mono.Addins;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x0200023D RID: 573
	internal class FilePathExtensionNode : ExtensionNode
	{
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x00056CDD File Offset: 0x00054EDD
		public FilePath FilePath
		{
			get
			{
				return base.Addin.GetFilePath(this.path);
			}
		}

		// Token: 0x04000666 RID: 1638
		[NodeAttribute(Required = true)]
		private string path;
	}
}
