using System;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x02000195 RID: 405
	internal class FileFormatNode : TypeExtensionNode
	{
		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x0003A513 File Offset: 0x00038713
		// (set) Token: 0x06000F9A RID: 3994 RVA: 0x0003A51B File Offset: 0x0003871B
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0003A524 File Offset: 0x00038724
		public bool CanDefault
		{
			get
			{
				return this.canDefault;
			}
		}

		// Token: 0x04000482 RID: 1154
		[NodeAttribute]
		private string name;

		// Token: 0x04000483 RID: 1155
		[NodeAttribute]
		private bool canDefault;
	}
}
