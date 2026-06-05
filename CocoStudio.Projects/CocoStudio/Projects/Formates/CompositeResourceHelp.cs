using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x0200002B RID: 43
	public static class CompositeResourceHelp
	{
		// Token: 0x060000F3 RID: 243 RVA: 0x00004C60 File Offset: 0x00002E60
		public static HashSet<ResourceData> GetResourcesIncludeImage(ResourceData resourceData, List<string> imageFiles)
		{
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			hashSet.Add(resourceData);
			if (imageFiles != null)
			{
				foreach (string name in imageFiles)
				{
					FilePath filePath = name;
					if (resourceData.Type == EnumResourceType.Default)
					{
						filePath = filePath.ToRelative(Option.EditorDefaultResourcePath);
					}
					else
					{
						filePath = filePath.ToRelative(ProjectsService.Instance.CurrentSolution.ItemDirectory);
					}
					ResourceData item = new ResourceData(resourceData.Type, filePath);
					hashSet.Add(item);
				}
			}
			return hashSet;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004D14 File Offset: 0x00002F14
		public static string GetMatchedImage(string resourcePath, string fileSuffix = ".png")
		{
			return Path.ChangeExtension(resourcePath, fileSuffix);
		}
	}
}
