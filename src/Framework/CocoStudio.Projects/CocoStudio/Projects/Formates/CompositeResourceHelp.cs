using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	public static class CompositeResourceHelp
	{
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

		public static string GetMatchedImage(string resourcePath, string fileSuffix = ".png")
		{
			return Path.ChangeExtension(resourcePath, fileSuffix);
		}
	}
}
