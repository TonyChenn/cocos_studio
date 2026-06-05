using System;
using Mono.Addins;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x02000042 RID: 66
	[ExtensionNode("Assembly")]
	internal class AssemblyExtensionNode : TypeExtensionNode
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00008DD0 File Offset: 0x00006FD0
		public string FileName
		{
			get
			{
				return this.file;
			}
		}

		// Token: 0x040000C3 RID: 195
		[NodeAttribute("file", Required = true)]
		private string file;
	}
}
