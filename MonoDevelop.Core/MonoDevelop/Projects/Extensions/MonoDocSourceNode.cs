using System;
using System.IO;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x020001A2 RID: 418
	internal class MonoDocSourceNode : ExtensionNode
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0003AFCE File Offset: 0x000391CE
		public string Directory
		{
			get
			{
				if (this.relative || !System.IO.Path.IsPathRooted(this.directory))
				{
					return base.Addin.GetFilePath(this.directory);
				}
				return this.directory;
			}
		}

		// Token: 0x040004A3 RID: 1187
		[NodeAttribute(Required = true)]
		protected string directory;

		// Token: 0x040004A4 RID: 1188
		[NodeAttribute]
		protected bool relative;
	}
}
