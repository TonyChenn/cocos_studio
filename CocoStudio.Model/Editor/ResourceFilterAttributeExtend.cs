using System;
using System.IO;
using System.Linq;
using CocoStudio.Projects;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000061 RID: 97
	internal static class ResourceFilterAttributeExtend
	{
		// Token: 0x06000347 RID: 839 RVA: 0x0000DD44 File Offset: 0x0000BF44
		public static bool CheckResource(this ResourceFilterAttribute attribute, ResourceFile resourceFile)
		{
			bool result;
			if (resourceFile == null)
			{
				result = false;
			}
			else
			{
				ResourceData resourceData = resourceFile.GetResourceData();
				if (attribute.ResourceTypeFilter != null)
				{
					if (!attribute.ResourceTypeFilter.Contains(resourceData.Type))
					{
						return false;
					}
				}
				string text = Path.GetExtension(resourceData.Path);
				if (text.Length > 1)
				{
					text = text.Substring(1);
				}
				result = attribute.FileFilter.Contains(text, StringComparer.OrdinalIgnoreCase);
			}
			return result;
		}
	}
}
