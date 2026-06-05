using System;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x0200018C RID: 396
	internal class ItemPropertyCodon : ExtensionNode
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0003A0B3 File Offset: 0x000382B3
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0003A0BB File Offset: 0x000382BB
		public Type PropertyType
		{
			get
			{
				if (this.type == null)
				{
					this.type = base.Addin.GetType(this.propType);
				}
				return this.type;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0003A0E8 File Offset: 0x000382E8
		public string PropertyTypeName
		{
			get
			{
				return this.propType;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0003A0F0 File Offset: 0x000382F0
		public string PropertyName
		{
			get
			{
				return this.propName;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x0003A0F8 File Offset: 0x000382F8
		public bool External
		{
			get
			{
				return this.external;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x0003A100 File Offset: 0x00038300
		public bool SkipEmpty
		{
			get
			{
				return this.skipEmpty;
			}
		}

		// Token: 0x04000470 RID: 1136
		[NodeAttribute("class", true)]
		private string typeName;

		// Token: 0x04000471 RID: 1137
		[NodeAttribute("name", true, Description = "Name of the property")]
		private string propName;

		// Token: 0x04000472 RID: 1138
		[NodeAttribute("type", true, Description = "Full name of the property type")]
		private string propType;

		// Token: 0x04000473 RID: 1139
		[NodeAttribute("external", false, Description = "Set to true if the property is an extension")]
		private bool external = true;

		// Token: 0x04000474 RID: 1140
		[NodeAttribute("skipEmpty", false, Description = "Set to true if empty elements don't have to be serialized")]
		private bool skipEmpty;

		// Token: 0x04000475 RID: 1141
		private Type type;
	}
}
