using System;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x0200018E RID: 398
	[ExtensionNode(Description = "A type name.")]
	internal class DataTypeCodon : ExtensionNode
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0003A13D File Offset: 0x0003833D
		public Type Class
		{
			get
			{
				return base.Addin.GetType(this.TypeName, true);
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0003A151 File Offset: 0x00038351
		public string TypeName
		{
			get
			{
				return this.typeName ?? this.typeName2;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0003A163 File Offset: 0x00038363
		public string ItemName
		{
			get
			{
				return this.itemName;
			}
		}

		// Token: 0x04000477 RID: 1143
		[NodeAttribute("class", false)]
		private string typeName;

		// Token: 0x04000478 RID: 1144
		[NodeAttribute("type", false)]
		private string typeName2;

		// Token: 0x04000479 RID: 1145
		[NodeAttribute("name")]
		private string itemName;
	}
}
