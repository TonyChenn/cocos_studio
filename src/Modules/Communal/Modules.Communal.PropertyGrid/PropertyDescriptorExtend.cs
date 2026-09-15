using System;
using System.ComponentModel;
using System.Linq;

namespace Modules.Communal.PropertyGrid
{
	public static class PropertyDescriptorExtend
	{
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

		public static bool GetBrowsable(this PropertyDescriptor propDescriptor)
		{
			BrowsableAttribute browsableAttribute = propDescriptor.Attributes.OfType<BrowsableAttribute>().FirstOrDefault<BrowsableAttribute>();
			return browsableAttribute == null || browsableAttribute.Browsable;
		}
	}
}
