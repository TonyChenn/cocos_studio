using System;
using System.ComponentModel;
using System.Linq;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000016 RID: 22
	public static class PropertyDescriptorExtend
	{
		// Token: 0x0600008C RID: 140 RVA: 0x0000374C File Offset: 0x0000194C
		public static string GetGroup(this PropertyDescriptor propDescriptor)
		{
			CategoryAttribute categoryAttribute = propDescriptor.Attributes.OfType<CategoryAttribute>().FirstOrDefault<CategoryAttribute>();
			string result;
			if (categoryAttribute == null)
			{
				result = string.Empty;
			}
			else
			{
				result = categoryAttribute.Category;
			}
			return result;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003788 File Offset: 0x00001988
		public static bool GetBrowsable(this PropertyDescriptor propDescriptor)
		{
			BrowsableAttribute browsableAttribute = propDescriptor.Attributes.OfType<BrowsableAttribute>().FirstOrDefault<BrowsableAttribute>();
			return browsableAttribute == null || browsableAttribute.Browsable;
		}
	}
}
